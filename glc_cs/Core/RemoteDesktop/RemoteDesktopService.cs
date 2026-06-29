using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace glc_cs.Core.RemoteDesktop
{
    internal class RemoteDesktopService : IDisposable
    {
        private SimpleWsServer _server;
        private ScreenCapture _capture;
        private AudioCapture _audio;
        private CancellationTokenSource _cts;
        private Task _captureTask;
        private Task _audioTask;
        private string _viewerHtml;

        public int Port { get; set; } = 8090;
        public string Password { get; set; } = "";
        public int Fps { get; set; } = 30;
        public int Quality { get; set; } = 100;
        public float Scale { get; set; } = 1.0f;
        public bool AudioEnabled { get; set; } = true;
        public bool IsRunning { get; private set; }

        public event Action<string> OnLog;
        public event Action<int> OnClientCountChanged;

        public void Start()
        {
            if (IsRunning) return;

            LoadViewerHtml();

            _capture = new ScreenCapture();
            _capture.Quality = Quality;
            _capture.Scale = Scale;

            _server = new SimpleWsServer();
            _server.OnClientConnected += c =>
            {
                var host = string.IsNullOrEmpty(c.HostName) ? "" : $" ({c.HostName})";
                Log($"Client connected: {c.Id} [{c.RemoteIP}]{host} {c.Browser}");
                c.SendText(BuildScreenInfo().ToString());
                OnClientCountChanged?.Invoke(_server.ClientCount);
            };
            _server.OnClientDisconnected += c =>
            {
                Log($"Client disconnected: {c.Id} [{c.RemoteIP}]");
                OnClientCountChanged?.Invoke(_server.ClientCount);
            };
            _server.OnTextMessage += HandleMessage;

            _server.Start(Port, Password, () => _viewerHtml);
            IsRunning = true;

            _cts = new CancellationTokenSource();
            _captureTask = Task.Run(() => CaptureLoop(_cts.Token));

            if (AudioEnabled)
            {
                try
                {
                    _audio = new AudioCapture();
                    _audio.Start();
                    _audioTask = Task.Run(() => AudioLoop(_cts.Token));
                    Log("Audio capture started");
                }
                catch (Exception ex)
                {
                    Log($"Audio capture failed: {ex.Message}");
                    _audio?.Dispose();
                    _audio = null;
                }
            }

            Log($"Remote Desktop started on port {Port} (scale={Scale:P0}, quality={Quality}, fps={Fps}, audio={AudioEnabled})");
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;

            _cts?.Cancel();

            _server?.Stop();
            _server = null;

            try { _captureTask?.Wait(5000); } catch { }
            _captureTask = null;

            try { _audioTask?.Wait(3000); } catch { }
            _audioTask = null;

            _audio?.Dispose();
            _audio = null;

            _capture?.Dispose();
            _capture = null;

            _cts?.Dispose();
            _cts = null;

            Log("Remote Desktop stopped");
        }

        private async Task CaptureLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && IsRunning)
            {
                int intervalMs = 1000 / Math.Max(1, Fps);
                try
                {
                    var server = _server;
                    var capture = _capture;
                    if (server == null || capture == null) break;

                    if (server.ClientCount > 0)
                    {
                        var frame = capture.CaptureFrameIfChanged();
                        if (frame != null)
                        {
                            // 先頭に0x00マーカー = 映像データ
                            var packet = new byte[frame.Length + 1];
                            packet[0] = 0x00;
                            Buffer.BlockCopy(frame, 0, packet, 1, frame.Length);
                            server.Broadcast(packet);
                        }
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex)
                {
                    if (ct.IsCancellationRequested) break;
                    Log($"Capture error: {ex.Message}");
                }

                try { await Task.Delay(intervalMs, ct); }
                catch (TaskCanceledException) { break; }
            }
        }

        private async Task AudioLoop(CancellationToken ct)
        {
            // 音声は50msごとにフラッシュ（約20回/秒）
            while (!ct.IsCancellationRequested && IsRunning)
            {
                try
                {
                    var server = _server;
                    var audio = _audio;
                    if (server == null || audio == null) break;

                    if (server.ClientCount > 0)
                    {
                        var pcm = audio.Flush();
                        if (pcm != null && pcm.Length > 0)
                        {
                            // 先頭に0x01マーカーを付けて音声データと区別
                            var packet = new byte[pcm.Length + 1];
                            packet[0] = 0x01; // audio marker
                            Buffer.BlockCopy(pcm, 0, packet, 1, pcm.Length);
                            server.Broadcast(packet);
                        }
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex)
                {
                    if (ct.IsCancellationRequested) break;
                    Log($"Audio error: {ex.Message}");
                }

                try { await Task.Delay(50, ct); }
                catch (TaskCanceledException) { break; }
            }
        }

        private void HandleMessage(WsClient client, string message)
        {
            try
            {
                var json = JObject.Parse(message);
                string type = json["type"]?.ToString();

                switch (type)
                {
                    case "mouse_move":
                        HandleMouseMove(json);
                        break;
                    case "mouse_click":
                        HandleMouseClick(json);
                        break;
                    case "mouse_down":
                        HandleMouseDown(json);
                        break;
                    case "mouse_up":
                        HandleMouseUp(json);
                        break;
                    case "mouse_dblclick":
                        HandleMouseDblClick(json);
                        break;
                    case "mouse_scroll":
                        HandleMouseScroll(json);
                        break;
                    case "key_down":
                        HandleKeyDown(json);
                        break;
                    case "key_up":
                        HandleKeyUp(json);
                        break;
                    case "set_quality":
                        int q = json["quality"]?.Value<int>() ?? 60;
                        Quality = q;
                        if (_capture != null) _capture.Quality = q;
                        break;
                    case "set_fps":
                        Fps = json["fps"]?.Value<int>() ?? 15;
                        break;
                    case "set_scale":
                        float s = json["scale"]?.Value<float>() ?? 0.75f;
                        Scale = s;
                        if (_capture != null) { _capture.Scale = s; BroadcastScreenInfo(); }
                        break;
                    case "set_monitor":
                        int monIdx = json["monitor"]?.Value<int>() ?? 0;
                        if (_capture != null) { _capture.SetMonitor(monIdx); BroadcastScreenInfo(); }
                        Log($"Monitor changed to index {monIdx}");
                        break;
                    case "type_text":
                        string txt = json["text"]?.ToString();
                        if (!string.IsNullOrEmpty(txt)) InputSimulator.TypeText(txt);
                        break;
                    case "ping":
                        long pingTs = json["ts"]?.Value<long>() ?? 0;
                        client.SendText(new JObject { ["type"] = "pong", ["ts"] = pingTs }.ToString());
                        break;
                    case "key_combo":
                        var comboKeys = json["keys"]?.ToObject<ushort[]>();
                        if (comboKeys != null && comboKeys.Length > 0) InputSimulator.KeyCombo(comboKeys);
                        break;
                    case "clipboard_get":
                        string clipText = InputSimulator.GetClipboardText();
                        client.SendText(new JObject { ["type"] = "clipboard_data", ["text"] = clipText }.ToString());
                        break;
                    case "clipboard_set":
                        string newClipText = json["text"]?.ToString();
                        if (newClipText != null) InputSimulator.SetClipboardText(newClipText);
                        break;
                }
            }
            catch { }
        }

        private JObject BuildScreenInfo()
        {
            var capture = _capture;
            if (capture == null) return new JObject { ["type"] = "screen_info" };

            var monitors = ScreenCapture.GetMonitors();
            var monArray = new JArray();
            foreach (var m in monitors)
            {
                monArray.Add(new JObject
                {
                    ["index"] = m.Index,
                    ["name"] = m.Name,
                    ["x"] = m.X,
                    ["y"] = m.Y,
                    ["width"] = m.Width,
                    ["height"] = m.Height,
                    ["isPrimary"] = m.IsPrimary
                });
            }

            return new JObject
            {
                ["type"] = "screen_info",
                ["width"] = capture.OutputWidth,
                ["height"] = capture.OutputHeight,
                ["screenWidth"] = capture.ScreenWidth,
                ["screenHeight"] = capture.ScreenHeight,
                ["fps"] = Fps,
                ["quality"] = Quality,
                ["scale"] = Scale,
                ["audioEnabled"] = AudioEnabled,
                ["audioSampleRate"] = AudioEnabled && _audio != null ? _audio.SampleRate : 0,
                ["audioChannels"] = AudioEnabled && _audio != null ? _audio.Channels : 1,
                ["monitors"] = monArray,
                ["currentMonitor"] = capture.MonitorIndex
            };
        }

        private void BroadcastScreenInfo()
        {
            var server = _server;
            if (server == null) return;
            server.BroadcastText(BuildScreenInfo().ToString());
        }

        private void HandleMouseMove(JObject json)
        {
            var pos = MapToScreen(json);
            InputSimulator.MoveMouse(pos.Item1, pos.Item2);
        }

        private void HandleMouseClick(JObject json)
        {
            var pos = MapToScreen(json);
            string button = json["button"]?.ToString() ?? "left";
            InputSimulator.MouseClick(pos.Item1, pos.Item2, button);
        }

        private void HandleMouseDown(JObject json)
        {
            var pos = MapToScreen(json);
            string button = json["button"]?.ToString() ?? "left";
            InputSimulator.MouseDown(pos.Item1, pos.Item2, button);
        }

        private void HandleMouseUp(JObject json)
        {
            var pos = MapToScreen(json);
            string button = json["button"]?.ToString() ?? "left";
            InputSimulator.MouseUp(pos.Item1, pos.Item2, button);
        }

        private void HandleMouseDblClick(JObject json)
        {
            var pos = MapToScreen(json);
            string button = json["button"]?.ToString() ?? "left";
            InputSimulator.MouseDoubleClick(pos.Item1, pos.Item2, button);
        }

        private void HandleMouseScroll(JObject json)
        {
            var pos = MapToScreen(json);
            int delta = json["delta"]?.Value<int>() ?? 0;
            InputSimulator.MouseScroll(pos.Item1, pos.Item2, delta);
        }

        private void HandleKeyDown(JObject json)
        {
            ushort vk = json["vk"]?.Value<ushort>() ?? 0;
            if (vk > 0) InputSimulator.KeyDown(vk);
        }

        private void HandleKeyUp(JObject json)
        {
            ushort vk = json["vk"]?.Value<ushort>() ?? 0;
            if (vk > 0) InputSimulator.KeyUp(vk);
        }

        private Tuple<int, int> MapToScreen(JObject json)
        {
            double x = json["x"]?.Value<double>() ?? 0;
            double y = json["y"]?.Value<double>() ?? 0;
            var capture = _capture;
            if (capture == null) return Tuple.Create(0, 0);
            int screenX = (int)(x / capture.OutputWidth * capture.ScreenWidth) + capture.CaptureX;
            int screenY = (int)(y / capture.OutputHeight * capture.ScreenHeight) + capture.CaptureY;
            screenX = Math.Max(capture.CaptureX, Math.Min(capture.CaptureX + capture.ScreenWidth - 1, screenX));
            screenY = Math.Max(capture.CaptureY, Math.Min(capture.CaptureY + capture.ScreenHeight - 1, screenY));
            return Tuple.Create(screenX, screenY);
        }

        private void LoadViewerHtml()
        {
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string htmlPath = Path.Combine(exeDir, "RemoteUI", "viewer.html");
            if (File.Exists(htmlPath))
            {
                _viewerHtml = File.ReadAllText(htmlPath, System.Text.Encoding.UTF8);
            }
            else
            {
                _viewerHtml = "<html><body><h1>viewer.html not found</h1><p>Expected at: " + htmlPath + "</p></body></html>";
            }
        }

        private void Log(string message)
        {
            OnLog?.Invoke($"[RemoteDesktop] {message}");
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
