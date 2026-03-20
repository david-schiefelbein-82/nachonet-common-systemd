using systemd1.DBus;
using Tmds.DBus;

namespace Nachonet.Common.Systemd
{
    public class SystemdServiceManager : IDisposable
    {

        private bool _disposed;
        private IManager _systemd;

        public event EventHandler<SystemdServiceStateChangedEventArgs>? StateChanged;

        public SystemdServiceManager()
        {
            _disposed = false;
            _services = [];
            var systemConnection = Connection.System;
            _systemd = systemConnection.CreateProxy<IManager>("org.freedesktop.systemd1", "/org/freedesktop/systemd1");
        }

        private List<SystemdService> _services;

        public List<SystemdService> Services => _services;

        public async Task StartUnitAsync(string name, string mode)
        {
            await _systemd.StartUnitAsync(name, mode);
        }

        public async Task StopUnitAsync(string name, string mode)
        {
            await _systemd.StopUnitAsync(name, mode);
        }

        protected virtual void OnServiceChanged(object? sender, SystemdServiceStateChangedEventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            try
            {
                var units = await _systemd.ListUnitsAsync();
                foreach (var info in units)
                {
                    var unit = info.Item7;
                    string name = info.Item1;
                    if (name == "cron.service")
                    {
                        Console.WriteLine("test");
                    }
                    string desc = info.Item2;
                    string stateStr = info.Item4;
                    string subStateStr = info.Item5;
                    var service = new SystemdService(this, unit, name, info.Item2, stateStr, subStateStr);
                    await service.InitAsync();
                    service.StateChanged += OnServiceChanged;
                    _services.Add(service);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // dispose managed resources.
                SystemdService[] services = [ .. _services];
                _services.Clear();
                foreach (var service in services)
                {
                    service.Dispose();
                }
            }

            // Free unmanaged resources.
            _disposed = true;
        }
    }
}