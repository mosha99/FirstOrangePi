using Microsoft.AspNetCore.SignalR;
using System.Device.Gpio;

public class PortMonitoringWorker : BackgroundService
{
    private readonly IGpioService _gpioService;
    private readonly IHubContext<SmartHub> _hubContext;

    public PortMonitoringWorker(IGpioService gpioService, IHubContext<SmartHub> hubContext)
    {
        _gpioService = gpioService;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var pin = _gpioService.RegisterInPin("Key", 232);
        pin.SubscribeToPinChange(async (pinEventType) =>
         {
             if(pinEventType == PinEventTypes.Rising)
             {
                 Console.WriteLine("Key Pressed");
             }
             else
             {
                 Console.WriteLine("Key Released");
             }
             await _hubContext.Clients.All.SendAsync("PinValueChanged", pin.Name, pinEventType.ToString());
         });
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
