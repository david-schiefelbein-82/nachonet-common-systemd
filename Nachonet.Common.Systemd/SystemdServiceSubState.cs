namespace Nachonet.Common.Systemd
{
    public enum SystemdServiceSubState
    {
        Unknown,

        Running,

        StopSigterm,

        Dead,

        Waiting,

        StartPre

    }

    public static class SystemdServiceSubStateExtensions
    {
        private static Dictionary<string, SystemdServiceSubState> _map = new Dictionary<string, SystemdServiceSubState>()
            {
                { "running", SystemdServiceSubState.Running },
                { "stop-sigterm", SystemdServiceSubState.StopSigterm },
                { "dead", SystemdServiceSubState.Dead },
                { "waiting", SystemdServiceSubState.Waiting },
                { "start-pre", SystemdServiceSubState.StartPre }
            };

        public static SystemdServiceSubState Parse(string s)
        {

            foreach (var kvp in _map)
            {
                if (string.Equals(s, kvp.Key))
                {
                    return kvp.Value;
                }
            }

            return SystemdServiceSubState.Unknown;
        }


        public static string Print(this SystemdServiceSubState value)
        {

            foreach (var kvp in _map)
            {
                if (value == kvp.Value)
                {
                    return kvp.Key;
                }
            }

            return value.ToString();
        }
    }
}