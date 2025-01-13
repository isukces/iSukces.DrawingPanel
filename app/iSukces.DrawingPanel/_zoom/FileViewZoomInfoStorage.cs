#if SAMPLES
using System;
using System.IO;
using System.Timers;
using iSukces.DrawingPanel.Interfaces;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace iSukces.DrawingPanel
{
    public sealed class FileViewZoomInfoStorage : IDrawingPanelZoomStorage
    {
        public FileViewZoomInfoStorage(FileInfo file)
        {
            _file  = file.FullName;
            _timer = CreateTimer(Timeout);
        }

        public FileViewZoomInfoStorage(DirectoryInfo file, string identifier)
        {
            string storageDir;
            {
                const int spreadPerDir = 256;
                if (Guid.TryParse(identifier, out var g))
                {
                    var st = g.ToString("N");
                    // identifier = st.Substring(4);
                    storageDir = Path.Combine(file.FullName, st.Substring(0, 2), st.Substring(2, 2));
                }
                else
                {
                    int q;
                    q = identifier.GetHashCode() % (spreadPerDir * spreadPerDir);
                    var level2 = (q % spreadPerDir).ToString("x2");
                    var level1 = (q / spreadPerDir).ToString("x2");
                    storageDir = Path.Combine(file.FullName, level1, level2);
                }
            }

            _file  = Path.Combine(storageDir, identifier + ".json");
            _timer = CreateTimer(Timeout);
        }

        private static Timer CreateTimer(ElapsedEventHandler OnTimedEvent)
        {
            var timer = new Timer(3000);
            timer.Elapsed   += OnTimedEvent;
            timer.AutoReset =  false;
            timer.Enabled   =  false;
            return timer;
        }

        public void Flush()
        {
            if (_lastNotSaved.HasValue)
                Write(_lastNotSaved.Value, true);
        }

        private void Timeout(object? sender, ElapsedEventArgs e)
        {
            lock(_lock)
            {
                Flush();
                _timerIsRunning = false;
            }
        }

        public bool TryRead(out DrawingPanelZoomStorageData data)
        {
            lock(_lock)
            {
                try
                {
                    if (File.Exists(_file))
                    {
                        var json = File.ReadAllText(_file);
                        if (!string.IsNullOrEmpty(json))
                        {
                            data = JsonConvert.DeserializeObject<DrawingPanelZoomStorageData>(json);
                            return true;
                        }
                    }
                }
                catch
                {
                }

                data = new DrawingPanelZoomStorageData();
                return false;
            }
        }

        public void Write(DrawingPanelZoomStorageData data)
        {
            Write(data, false);
        }

        private void Write(DrawingPanelZoomStorageData data, bool force)
        {
            lock(_lock)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    _lastNotSaved = data;
                    if (!force)
                    {
                        if (_timerIsRunning)
                            return;
                        _timerIsRunning = true;
                        _timer.Enabled  = true;
                    }

                    _lastWrite = now;
                    var fi = new FileInfo(_file);
                    fi.Directory?.Create();
                    var json = JsonConvert.SerializeObject(data);
                    File.WriteAllText(_file, json);
                    _lastNotSaved = null;
                }
                catch
                {
                }
            }
        }

        #region Fields

        private readonly object _lock = new object();

        private readonly string _file;

        private DateTime _lastWrite = DateTime.MinValue;
        private DrawingPanelZoomStorageData? _lastNotSaved;
        private readonly Timer _timer;
        private bool _timerIsRunning;

        #endregion
    }
}

#endif

