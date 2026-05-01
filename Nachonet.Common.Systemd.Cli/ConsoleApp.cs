namespace Nachonet.Common.Systemd.Cli
{
    public class ConsoleApp(SystemdServiceManager sem)
    {
        private readonly StringComparison _ignoreCase = StringComparison.InvariantCultureIgnoreCase;
        private readonly SystemdServiceManager _sm = sem;

        private SystemdService GetServiceByName(string serviceName)
        {
            var svc = _sm.Services.FirstOrDefault(x => string.Equals(x.Name, serviceName, _ignoreCase));
            svc ??= _sm.Services.FirstOrDefault(x => string.Equals(x.Name, serviceName + ".service", _ignoreCase));

            return svc ?? throw new Exception(serviceName + " not found");
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                PrintUsage();
                while (true)
                {
                    string? cmd = Console.ReadLine();
                    if (cmd == null)
                        break;

                    else if (cmd == "quit" || cmd == "q" || cmd == "exit")
                    {
                        break;
                    }
                    else if (cmd.StartsWith("start ", _ignoreCase))
                    {
                        var serviceName = cmd["start ".Length..].Trim();
                        try
                        {
                            var svc = GetServiceByName(serviceName);
                            Console.WriteLine("Starting " + svc.Name + "...");
                            var startTask = svc.StartAsync();
                            startTask.ContinueWith(tt =>
                            {
                                Console.WriteLine("Started \"" + svc.Name + "\"");
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("start " + serviceName + ". " + ex.GetType().Name + ": " + ex.Message);
                        }
                    }
                    else if (cmd.StartsWith("stop ", _ignoreCase))
                    {
                        var serviceName = cmd["stop ".Length..].Trim();
                        try
                        {
                            var svc = GetServiceByName(serviceName);
                            Console.WriteLine("Stopping " + svc.Name + "...");
                            var stoptask = svc.StopAsync();
                            stoptask.ContinueWith(tt =>
                            {
                                Console.WriteLine("Stopped \"" + svc.Name + "\"");
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("stop \"" + serviceName + "\" failed - " + ex.GetType().Name + ": " + ex.Message);
                        }
                    }
                    else if (cmd.StartsWith("status ", _ignoreCase))
                    {
                        var serviceName = cmd["status ".Length..].Trim();
                        try
                        {
                            var svc = GetServiceByName(serviceName);
                            Console.WriteLine(svc.Name + " : " + svc.ActiveState + " (" + svc.SubState + ")");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("status \"" + serviceName + "\" failed - " + ex.GetType().Name + ": " + ex.Message);
                        }
                    }
                    else if (cmd.StartsWith("list ", _ignoreCase))
                    {
                        var serviceName = cmd["list ".Length..].Trim();
                        try
                        {
                            SystemdService[] svcList = [ .. from x in _sm.Services
                                          where x.Name.Contains(serviceName, _ignoreCase)
                                          select x];
                            Console.WriteLine("list \"" + serviceName + "\" - found " + svcList.Length + " services");
                            foreach (var svc in svcList)
                                Console.WriteLine(svc.Name + " : " + svc.ActiveState + " (" + svc.SubState + ")");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("list \"" + serviceName + "\" failed - " + ex.GetType().Name + ": " + ex.Message);
                        }
                    }
                    else if (cmd.StartsWith("help"))
                    {
                        PrintUsage();
                    }
                    else
                    {
                        Console.WriteLine("Unknown Command \"" + cmd + "\"");
                    }
                }
            }, cancellationToken);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  start <service-name>  - Start a service");
            Console.WriteLine("  stop <service-name>   - Stop a service");
            Console.WriteLine("  status <service-name> - Get the status of a service");
            Console.WriteLine("  list <search-term>    - List services matching the search term");
            Console.WriteLine("  help                  - this screen");
            Console.WriteLine("  quit|q|exit           - Exit the application");
        }
    }   
}