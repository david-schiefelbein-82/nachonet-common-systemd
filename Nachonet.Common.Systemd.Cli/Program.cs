namespace Nachonet.Common.Systemd.Cli
{

    public class Program
    {
        private static async Task Main(string[] args)
        {
            var stopping = new CancellationTokenSource();
            Console.WriteLine("Systemd ServiceManager");

            var sm = new SystemdServiceManager();
            await sm.LoadAsync(stopping.Token);

            sm.StateChanged += (s, ev) =>
            {
                Console.WriteLine($"{ev.Service.Name} ActiveState {ev.OldActiveState}->{ev.ActiveState}, SubState: {ev.OldSubState}->{ev.SubState}");
            };

            Console.WriteLine($"{"Service Name",-80} {"ActiveState",-20} SubState");
            foreach (var service in sm.Services)
            {
                Console.WriteLine($"{service.Name,-80} {service.ActiveState,-20} {service.SubState}");
            }

            var app = new ConsoleApp(sm);
            await app.RunAsync();
            stopping.Cancel();
        }
    }
}