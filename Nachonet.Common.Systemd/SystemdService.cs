using systemd1.DBus;
using Tmds.DBus;

namespace Nachonet.Common.Systemd
{

    public class SystemdService : IDisposable
    {
        private bool _disposed;
        private SystemdServiceManager _sm;
        private IUnit _unit;

        public event EventHandler<SystemdServiceStateChangedEventArgs>? StateChanged;

        public string Name { get; }

        public string Description { get; }

        public SystemdServiceActiveState ActiveState { get; private set; }

        public SystemdServiceSubState SubState { get; private set; }

        private IDisposable? _watcher;

        private static SystemdServiceActiveState ParseState(string stateStr)
        {
            if (Enum.TryParse<SystemdServiceActiveState>(stateStr, true, out var st))
            {
                return st;
            }
            else
            {
                return SystemdServiceActiveState.Unknown;
            }
        }

        public async Task StartAsync()
        {
            await _sm.StartUnitAsync(Name, "replace");
        }

        public async Task StopAsync()
        {
            await _sm.StopUnitAsync(Name, "replace");
        }

        internal SystemdService(SystemdServiceManager sm, IUnit unit, string name, string description, string stateStr, string subStateStr)
        {
            _disposed = false;
            _sm = sm;
            _unit = unit;
            Name = name;
            Description = description;
            ActiveState = ParseState(stateStr);
            SubState = SystemdServiceSubStateExtensions.Parse(subStateStr);
        }

        internal async Task InitAsync()
        {
            _watcher = await _unit.WatchPropertiesAsync(Watcher);
        }

        private void Watcher(PropertyChanges changes)
        {
            var activeStateKvp = changes.Changed.FirstOrDefault(y => y.Key == "ActiveState");
            var subStateKvp = changes.Changed.FirstOrDefault(y => y.Key == "SubState");

            bool changed = false;
            var oldState = ActiveState;
            var newState = ActiveState;
            var oldSubState = SubState;
            var newSubState = SubState;
            if (!string.IsNullOrEmpty(activeStateKvp.Key))
            {
                if (activeStateKvp.Value is string stateStr)
                {
                    newState = ParseState(stateStr);
                    if (newState != oldState)
                    {
                        ActiveState = newState;
                        changed = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(subStateKvp.Key))
            {
                if (subStateKvp.Value is string subStateStr)
                {
                    newSubState = SystemdServiceSubStateExtensions.Parse(subStateStr);
                    if (newSubState != SystemdServiceSubState.Unknown && newSubState != oldSubState)
                    {
                        SubState = newSubState;
                        changed = true;
                    }
                }
            }

            if (changed)
                StateChanged?.Invoke(this, new SystemdServiceStateChangedEventArgs(this, oldState, newState, oldSubState, newSubState));
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
                _watcher?.Dispose();
            }

            // Free unmanaged resources.
            _disposed = true;
        }
    }
}