namespace Nachonet.Common.Systemd.Cli
{
    public class ConsoleApp(SystemdServiceManager sem)
    {
        private TaskCompletionSource _tcs = new TaskCompletionSource();
        private Thread? _th;

        private SystemdServiceManager _sm = sem;

        public async Task RunAsync()
        {
            _th = new Thread(ConsoleThread);
            _th.Start();
            await _tcs.Task;
        }

        private void ConsoleThread(object? obj)
        {
            while (true)
            {
                Console.Write("Command: ");
                string? cmd = Console.ReadLine();
                if (cmd == null)
                    break;
                else if (cmd == "quit" || cmd == "q")
                {
                    break;
                }
                else if (cmd.StartsWith("start "))
                {
                    var serviceName = cmd.Substring("start ".Length).Trim();
                    try
                    {
                        var cron = _sm.Services.FirstOrDefault(x => x.Name == serviceName) ?? throw new Exception("no " + serviceName + "found");
                        Console.WriteLine("Starting " + serviceName + "...");
                        var startTask = cron.StartAsync();
                        startTask.ContinueWith(tt =>
                        {
                             Console.WriteLine("Started " + serviceName + ".");
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("start " + serviceName + ". " + ex.GetType().Name + ": " + ex.Message);
                    }
                }
                else if (cmd.StartsWith("stop "))
                {
                    var serviceName = cmd.Substring("stop ".Length).Trim();
                    try
                    {
                        var cron = _sm.Services.FirstOrDefault(x => x.Name == serviceName) ?? throw new Exception("no " + serviceName + "found");
                        Console.WriteLine("Stopped " + serviceName + "...");
                        var stoptask = cron.StopAsync();
                        stoptask.ContinueWith(tt =>
                        {
                            Console.WriteLine("Stopped " + serviceName + ".");
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("stop " + serviceName + ". " + ex.GetType().Name + ": " + ex.Message);
                    }
                }
                else if (cmd.StartsWith("status "))
                {
                    var serviceName = cmd.Substring("status ".Length).Trim();
                    try
                    {
                        var cron = _sm.Services.FirstOrDefault(x => x.Name == serviceName) ?? throw new Exception("no " + serviceName + "found");
                        Console.WriteLine(cron.Name + ": " + cron.ActiveState + " (" + cron.SubState + ")");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("stop " + serviceName + ". " + ex.GetType().Name + ": " + ex.Message);
                    }
                }
            }

            _tcs.SetResult();
        }
    }
}