using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace glc_cs.Core.RemoteDesktop
{
    internal class MonitorInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsPrimary { get; set; }
    }

    internal class ScreenCapture : IDisposable
    {
        private Bitmap _buffer;
        private Graphics _graphics;
        private readonly ImageCodecInfo _jpegCodec;
        private readonly EncoderParameters _encoderParams;
        private readonly object _lock = new object();
        private int _quality = 60;
        private float _scale = 1.0f;
        private byte[] _lastHash;
        private Rectangle _captureBounds;

        public int CaptureX => _captureBounds.X;
        public int CaptureY => _captureBounds.Y;
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public int OutputWidth { get; private set; }
        public int OutputHeight { get; private set; }
        public int MonitorIndex { get; private set; }

        public int Quality
        {
            get => _quality;
            set
            {
                _quality = Math.Max(10, Math.Min(100, value));
                _encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)_quality);
            }
        }

        public float Scale
        {
            get => _scale;
            set
            {
                _scale = Math.Max(0.1f, Math.Min(1.0f, value));
                UpdateDimensions();
            }
        }

        public ScreenCapture()
        {
            _jpegCodec = GetJpegCodec();
            _encoderParams = new EncoderParameters(1);
            _encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)_quality);
            SetMonitor(0);
        }

        public static List<MonitorInfo> GetMonitors()
        {
            var list = new List<MonitorInfo>();
            var screens = Screen.AllScreens.OrderBy(s => s.Bounds.X).ThenBy(s => s.Bounds.Y).ToArray();
            for (int i = 0; i < screens.Length; i++)
            {
                var s = screens[i];
                list.Add(new MonitorInfo
                {
                    Index = i,
                    Name = s.Primary ? $"モニター {i + 1} (メイン)" : $"モニター {i + 1}",
                    X = s.Bounds.X,
                    Y = s.Bounds.Y,
                    Width = s.Bounds.Width,
                    Height = s.Bounds.Height,
                    IsPrimary = s.Primary
                });
            }

            if (screens.Length > 1)
            {
                var all = GetAllMonitorsBounds(screens);
                list.Add(new MonitorInfo
                {
                    Index = -1,
                    Name = "全画面 (すべてのモニター)",
                    X = all.X,
                    Y = all.Y,
                    Width = all.Width,
                    Height = all.Height,
                    IsPrimary = false
                });
            }

            return list;
        }

        private static Rectangle GetAllMonitorsBounds(Screen[] screens)
        {
            int minX = screens.Min(s => s.Bounds.Left);
            int minY = screens.Min(s => s.Bounds.Top);
            int maxX = screens.Max(s => s.Bounds.Right);
            int maxY = screens.Max(s => s.Bounds.Bottom);
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public void SetMonitor(int index)
        {
            MonitorIndex = index;
            var screens = Screen.AllScreens.OrderBy(s => s.Bounds.X).ThenBy(s => s.Bounds.Y).ToArray();

            if (index == -1)
            {
                _captureBounds = GetAllMonitorsBounds(screens);
            }
            else if (index >= 0 && index < screens.Length)
            {
                _captureBounds = screens[index].Bounds;
            }
            else
            {
                _captureBounds = Screen.PrimaryScreen.Bounds;
            }

            UpdateDimensions();
        }

        private void UpdateDimensions()
        {
            lock (_lock)
            {
                ScreenWidth = _captureBounds.Width;
                ScreenHeight = _captureBounds.Height;
                OutputWidth = (int)(ScreenWidth * _scale);
                OutputHeight = (int)(ScreenHeight * _scale);

                _graphics?.Dispose();
                _buffer?.Dispose();
                _buffer = new Bitmap(OutputWidth, OutputHeight, PixelFormat.Format24bppRgb);
                _graphics = Graphics.FromImage(_buffer);
                _graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                _graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                _lastHash = null;
            }
        }

        public byte[] CaptureFrame()
        {
            lock (_lock)
            {
                if (Math.Abs(_scale - 1.0f) < 0.01f)
                {
                    _graphics.CopyFromScreen(_captureBounds.Location, Point.Empty, _captureBounds.Size, CopyPixelOperation.SourceCopy);
                }
                else
                {
                    using (var full = new Bitmap(_captureBounds.Width, _captureBounds.Height, PixelFormat.Format24bppRgb))
                    using (var g = Graphics.FromImage(full))
                    {
                        g.CopyFromScreen(_captureBounds.Location, Point.Empty, _captureBounds.Size, CopyPixelOperation.SourceCopy);
                        _graphics.DrawImage(full, 0, 0, OutputWidth, OutputHeight);
                    }
                }

                using (var ms = new MemoryStream())
                {
                    _buffer.Save(ms, _jpegCodec, _encoderParams);
                    return ms.ToArray();
                }
            }
        }

        public byte[] CaptureFrameIfChanged()
        {
            var frame = CaptureFrame();

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                int step = Math.Max(1, frame.Length / 1024);
                var sample = new byte[Math.Min(1024, frame.Length)];
                for (int i = 0; i < sample.Length; i++)
                    sample[i] = frame[i * step % frame.Length];

                var hash = md5.ComputeHash(sample);
                if (_lastHash != null && CompareBytes(hash, _lastHash))
                    return null;
                _lastHash = hash;
            }

            return frame;
        }

        private static bool CompareBytes(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }

        private static ImageCodecInfo GetJpegCodec()
        {
            foreach (var codec in ImageCodecInfo.GetImageEncoders())
            {
                if (codec.MimeType == "image/jpeg") return codec;
            }
            return null;
        }

        public void Dispose()
        {
            _graphics?.Dispose();
            _buffer?.Dispose();
            _encoderParams?.Dispose();
        }
    }
}
