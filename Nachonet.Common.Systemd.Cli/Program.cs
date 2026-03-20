using System.Net;
using Tmds.DBus;

namespace Nachonet.Common.Systemd.Cli
{

    public class Program
    {
        private static async Task Main(string[] args)
        {
            CancellationTokenSource src = new CancellationTokenSource();
            Console.WriteLine("Systemd ServiceManager");

            var sm = new SystemdServiceManager();
            await sm.LoadAsync(src.Token);

            sm.StateChanged += (s, ev) =>
                {
                    Console.WriteLine("{0}", ev);
                };

            ConsoleApp app = new ConsoleApp(sm);
            await app.RunAsync();
            
            Console.WriteLine("Finalising program....");
            src.Cancel();
        }
    }
}