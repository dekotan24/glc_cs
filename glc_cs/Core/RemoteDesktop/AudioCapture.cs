using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.IO;

namespace glc_cs.Core.RemoteDesktop
{
    internal class AudioCapture : IDisposable
    {
        private WasapiLoopbackCapture _capture;
        private WaveFormat _captureFormat;
        private MemoryStream _buffer;
        private readonly object _lock = new object();
        private bool _disposed;

        public int SampleRate { get; set; } = 48000;
        public int Channels { get; set; } = 2;
        public int BitsPerSample { get; } = 16;

        public bool IsCapturing { get; private set; }

        public void Start()
        {
            if (IsCapturing) return;

            _capture = new WasapiLoopbackCapture();
            _captureFormat = _capture.WaveFormat;
            _buffer = new MemoryStream();

            _capture.DataAvailable += OnDataAvailable;
            _capture.RecordingStopped += (s, e) => IsCapturing = false;
            _capture.StartRecording();
            IsCapturing = true;
        }

        public void Stop()
        {
            if (!IsCapturing) return;
            IsCapturing = false;
            try { _capture?.StopRecording(); } catch { }
            try { _capture?.Dispose(); } catch { }
            _capture = null;
            lock (_lock)
            {
                _buffer?.Dispose();
                _buffer = null;
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (!IsCapturing || e.BytesRecorded == 0) return;

            byte[] resampled = ResampleToTarget(e.Buffer, e.BytesRecorded, _captureFormat);
            if (resampled == null || resampled.Length == 0) return;

            lock (_lock)
            {
                if (_buffer == null) return;
                // バッファが1MB超えたら古いデータ破棄
                if (_buffer.Length > 1024 * 1024)
                    _buffer.SetLength(0);
                _buffer.Write(resampled, 0, resampled.Length);
            }
        }

        public byte[] Flush()
        {
            lock (_lock)
            {
                if (_buffer == null || _buffer.Length == 0) return null;
                byte[] data = _buffer.ToArray();
                _buffer.SetLength(0);
                return data;
            }
        }

        private byte[] ResampleToTarget(byte[] buffer, int bytesRecorded, WaveFormat srcFormat)
        {
            int srcChannels = srcFormat.Channels;
            int srcSampleRate = srcFormat.SampleRate;
            int srcBytesPerSample = srcFormat.BitsPerSample / 8;
            int srcBlockAlign = srcChannels * srcBytesPerSample;

            int totalSrcSamples = bytesRecorded / srcBlockAlign;
            if (totalSrcSamples == 0) return null;

            double ratio = (double)SampleRate / srcSampleRate;
            int outputSamples = (int)(totalSrcSamples * ratio);
            if (outputSamples == 0) return null;

            int outChannels = Channels;
            int outBytesPerFrame = outChannels * 2;
            byte[] output = new byte[outputSamples * outBytesPerFrame];

            for (int i = 0; i < outputSamples; i++)
            {
                double srcPos = i / ratio;
                int srcIdx = (int)srcPos;
                if (srcIdx >= totalSrcSamples) srcIdx = totalSrcSamples - 1;

                int offset = srcIdx * srcBlockAlign;

                double[] samples = new double[srcChannels];
                for (int ch = 0; ch < srcChannels; ch++)
                {
                    int chOffset = offset + ch * srcBytesPerSample;
                    if (chOffset + srcBytesPerSample > bytesRecorded) break;

                    if (srcFormat.Encoding == WaveFormatEncoding.IeeeFloat && srcBytesPerSample == 4)
                        samples[ch] = BitConverter.ToSingle(buffer, chOffset);
                    else if (srcBytesPerSample == 2)
                        samples[ch] = BitConverter.ToInt16(buffer, chOffset) / 32768.0;
                    else if (srcBytesPerSample == 4)
                        samples[ch] = BitConverter.ToInt32(buffer, chOffset) / 2147483648.0;
                }

                for (int outCh = 0; outCh < outChannels; outCh++)
                {
                    double val;
                    if (outChannels == 1)
                    {
                        val = 0;
                        for (int ch = 0; ch < srcChannels; ch++) val += samples[ch];
                        val /= srcChannels;
                    }
                    else
                    {
                        int srcCh = outCh < srcChannels ? outCh : 0;
                        val = samples[srcCh];
                    }

                    if (val > 1.0) val = 1.0;
                    if (val < -1.0) val = -1.0;
                    short pcm16 = (short)(val * 32767);
                    int pos = i * outBytesPerFrame + outCh * 2;
                    output[pos] = (byte)(pcm16 & 0xFF);
                    output[pos + 1] = (byte)((pcm16 >> 8) & 0xFF);
                }
            }

            return output;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Stop();
        }
    }
}
