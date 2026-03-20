namespace Nachonet.Common.Systemd
{
    public class SystemdServiceStateChangedEventArgs(SystemdService service,
            SystemdServiceActiveState oldActiveState, SystemdServiceActiveState activeState,
            SystemdServiceSubState oldSubState, SystemdServiceSubState subState) : EventArgs
    {
        public SystemdService Service { get; } = service;
        public SystemdServiceActiveState OldActiveState { get; } = oldActiveState;
        public SystemdServiceActiveState ActiveState { get; } = activeState;
        public SystemdServiceSubState OldSubState { get; } = oldSubState;
        public SystemdServiceSubState SubState { get; } = subState;

        public override string ToString()
        {
            return string.Format("{0} ActiveState {1}->{2}, SubState: {3}->{4}", Service.Name, OldActiveState, ActiveState, OldSubState, SubState);
        }
    }
}