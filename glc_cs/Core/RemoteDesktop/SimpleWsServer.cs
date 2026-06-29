using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace glc_cs.Core.RemoteDesktop
{
    internal class WsClient
    {
        private readonly WebSocket _ws;
        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);

        public string Id { get; }
        public string RemoteIP { get; }
        public string HostName { get; private set; }
        public string Browser { get; }
        public bool Authenticated { get; set; }

        public WsClient(WebSocket ws, IPEndPoint remoteEndPoint, string userAgent)
        {
            _ws = ws;
            Id = Guid.NewGuid().ToString("N").Substring(0, 8);

            if (remoteEndPoint != null)
            {
                RemoteIP = remoteEndPoint.Address.ToString();
                HostName = "";
                var addr = remoteEndPoint.Address;
                Task.Run(() =>
                {
                    try { HostName = Dns.GetHostEntry(addr).HostName; } catch { }
                });
            }
            else
            {
                RemoteIP = "unknown";
                HostName = "";
            }

            Browser = ParseBrowser(userAgent ?? "");
        }

        private static string ParseBrowser(string ua)
        {
            if (string.IsNullOrEmpty(ua)) return "unknown";

            // Edge must be checked before Chrome (UA contains both)
            if (ua.Contains("Edg/"))
                return ExtractToken(ua, "Edg/");
            if (ua.Contains("OPR/"))
                return "Opera/" + ExtractVersion(ua, "OPR/");
            if (ua.Contains("Vivaldi/"))
                return ExtractToken(ua, "Vivaldi/");
            if (ua.Contains("Firefox/"))
                return ExtractToken(ua, "Firefox/");
            if (ua.Contains("Chrome/"))
                return ExtractToken(ua, "Chrome/");
            if (ua.Contains("Safari/") && ua.Contains("Version/"))
                return "Safari/" + ExtractVersion(ua, "Version/");

            return ua.Length > 60 ? ua.Substring(0, 60) + "…" : ua;
        }

        private static string ExtractToken(string ua, string key)
        {
            int idx = ua.IndexOf(key, StringComparison.Ordinal);
            if (idx < 0) return key.TrimEnd('/');
            int end = ua.IndexOfAny(new[] { ' ', ';', ')' }, idx + key.Length);
            return end < 0 ? ua.Substring(idx) : ua.Substring(idx, end - idx);
        }

        private static string ExtractVersion(string ua, string key)
        {
            int idx = ua.IndexOf(key, StringComparison.Ordinal);
            if (idx < 0) return "?";
            idx += key.Length;
            int end = ua.IndexOfAny(new[] { ' ', ';', ')' }, idx);
            return end < 0 ? ua.Substring(idx) : ua.Substring(idx, end - idx);
        }

        public void SendBinary(byte[] data)
        {
            _writeLock.Wait();
            try
            {
                if (_ws.State == WebSocketState.Open)
                    _ws.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, CancellationToken.None).Wait();
            }
            catch { }
            finally { _writeLock.Release(); }
        }

        public void SendText(string text)
        {
            _writeLock.Wait();
            try
            {
                if (_ws.State == WebSocketState.Open)
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    _ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                }
            }
            catch { }
            finally { _writeLock.Release(); }
        }

        public void SendClose()
        {
            try
            {
                if (_ws.State == WebSocketState.Open)
                    _ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).Wait();
            }
            catch { }
        }

        public void Close()
        {
            try { _ws.Dispose(); } catch { }
        }
    }

    internal class SimpleWsServer
    {
        private HttpListener _listener;
        private CancellationTokenSource _cts;
        private readonly ConcurrentDictionary<string, WsClient> _clients = new ConcurrentDictionary<string, WsClient>();
        private Func<string> _htmlProvider;
        private string _password;

        public event Action<WsClient> OnClientConnected;
        public event Action<WsClient> OnClientDisconnected;
        public event Action<WsClient, string> OnTextMessage;

        public int ClientCount => _clients.Count;
        public IEnumerable<WsClient> Clients => _clients.Values;

        public void Start(int port, string password, Func<string> htmlProvider)
        {
            _password = password ?? "";
            _htmlProvider = htmlProvider;
            _cts = new CancellationTokenSource();

            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://+:{port}/");
            try
            {
                _listener.Start();
            }
            catch
            {
                _listener.Close();
                try { EnsureUrlAcl(port); } catch { }
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://+:{port}/");
                try
                {
                    _listener.Start();
                }
                catch
                {
                    _listener.Close();
                    _listener = new HttpListener();
                    _listener.Prefixes.Add($"http://localhost:{port}/");
                    _listener.Start();
                }
            }

            Task.Run(() => AcceptLoop(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
            try { _listener?.Stop(); } catch { }
            try { _listener?.Close(); } catch { }
            foreach (var c in _clients.Values)
            {
                try { c.SendClose(); } catch { }
                c.Close();
            }
            _clients.Clear();
            _cts?.Dispose();
            _cts = null;
            _listener = null;
        }

        public void Broadcast(byte[] data)
        {
            foreach (var c in _clients.Values.Where(c => c.Authenticated))
            {
                c.SendBinary(data);
            }
        }

        public void BroadcastText(string text)
        {
            foreach (var c in _clients.Values.Where(c => c.Authenticated))
            {
                c.SendText(text);
            }
        }

        private async Task AcceptLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        var _ = Task.Run(() => HandleWebSocket(context, ct));
                    }
                    else
                    {
                        HandleHttp(context);
                    }
                }
                catch (ObjectDisposedException) { break; }
                catch (HttpListenerException) { break; }
                catch { }
            }
        }

        private void HandleHttp(HttpListenerContext context)
        {
            try
            {
                string path = context.Request.Url.AbsolutePath;
                byte[] bodyBytes;
                string contentType;

                if (path == "/" || path == "/index.html")
                {
                    var body = _htmlProvider?.Invoke() ?? "<html><body>Remote Desktop</body></html>";
                    bodyBytes = Encoding.UTF8.GetBytes(body);
                    contentType = "text/html; charset=utf-8";
                }
                else
                {
                    bodyBytes = Encoding.UTF8.GetBytes("404 Not Found");
                    contentType = "text/plain";
                    context.Response.StatusCode = 404;
                }

                context.Response.ContentType = contentType;
                context.Response.ContentLength64 = bodyBytes.Length;
                context.Response.OutputStream.Write(bodyBytes, 0, bodyBytes.Length);
                context.Response.Close();
            }
            catch
            {
                try { context.Response.Close(); } catch { }
            }
        }

        private async Task HandleWebSocket(HttpListenerContext context, CancellationToken ct)
        {
            HttpListenerWebSocketContext wsContext;
            try
            {
                wsContext = await context.AcceptWebSocketAsync(null);
            }
            catch { return; }

            var ws = wsContext.WebSocket;
            var client = new WsClient(ws, context.Request.RemoteEndPoint, context.Request.UserAgent);

            if (string.IsNullOrEmpty(_password))
            {
                client.Authenticated = true;
            }

            if (!client.Authenticated)
            {
                client.SendText("{\"type\":\"auth_required\"}");
            }
            else
            {
                client.SendText("{\"type\":\"auth_ok\"}");
            }

            _clients.TryAdd(client.Id, client);
            OnClientConnected?.Invoke(client);

            try
            {
                var buffer = new byte[65536];
                while (!ct.IsCancellationRequested && ws.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result;
                    try
                    {
                        result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                    }
                    catch (OperationCanceledException) { break; }
                    catch (WebSocketException) { break; }
                    catch { break; }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        try { await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None); }
                        catch { }
                        break;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string text;
                        using (var ms = new MemoryStream())
                        {
                            ms.Write(buffer, 0, result.Count);
                            while (!result.EndOfMessage)
                            {
                                result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                                ms.Write(buffer, 0, result.Count);
                            }
                            text = Encoding.UTF8.GetString(ms.ToArray());
                        }

                        if (!client.Authenticated)
                        {
                            if (text == _password)
                            {
                                client.Authenticated = true;
                                client.SendText("{\"type\":\"auth_ok\"}");
                            }
                            else
                            {
                                client.SendText("{\"type\":\"auth_fail\"}");
                            }
                        }
                        else
                        {
                            OnTextMessage?.Invoke(client, text);
                        }
                    }
                }
            }
            finally
            {
                WsClient removed;
                _clients.TryRemove(client.Id, out removed);
                OnClientDisconnected?.Invoke(client);
                client.Close();
            }
        }

        private static void EnsureUrlAcl(int port)
        {
            string url = $"http://+:{port}/";
            var identity = WindowsIdentity.GetCurrent();
            string user = identity.Name;

            var psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = $"http add urlacl url={url} user=\"{user}\"",
                Verb = "runas",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            var proc = Process.Start(psi);
            proc?.WaitForExit(10000);
        }
    }
}
