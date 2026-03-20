namespace Nachonet.Common.Systemd
{
    public enum SystemdServiceActiveState
    {
        Unknown,
        /// <summary>
        /// Started, bound, plugged in, …, depending on the unit type.
        /// </summary>
        Active,

        /// <summary>
        /// 	Stopped, unbound, unplugged, …, depending on the unit type.
        /// </summary>
        Inactive,

        /// <summary>
        /// Similar to inactive, but the unit failed in some way (process returned error code on exit, crashed, an operation timed out, or after too many restarts).
        /// </summary>
        Failed,

        /// <summary>
        /// Changing from inactive to active.
        /// </summary>
        Activating,

        /// <summary>
        /// Changing from active to inactive.
        /// </summary>
        Deactivating,

        /// <summary>
        /// Unit is inactive and a maintenance operation is in progress.
        /// </summary>
        Maintenance,

        /// <summary>
        /// Unit is active and it is reloading its configuration.
        /// </summary>
        Reloading,

        /// <summary>
        /// 	Unit is active and a new mount is being activated in its namespace.
        /// </summary>
        Refreshing,
    }
}