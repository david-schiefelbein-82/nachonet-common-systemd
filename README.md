# Nachonet.Common.Systemd

## Descriptions
This package uses Tmds.DBus to talk to the systemd service to allow users to 
- start services
- stop services
- query services
- get events when a service is started or stopped

## Source Code

https://github.com/david-schiefelbein-82/nachonet-common-systemd

## Compatability

Developed and built on debian 12 - other linux distros are untested.

## Example

```
        private static async Task Main(string[] args)
        {
            var stopping = new CancellationTokenSource();

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

            await Task.Run(() =>
            {
               // blocking method, wrap in a task to avoid deadlock
               Console.ReadLine();
            });
            stopping.Cancel();
        }
```
## Other Notes

To change service status you must be running as root or under sudo