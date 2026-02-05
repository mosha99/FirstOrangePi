using Iot.Device.KeyMatrix;
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
        int[] rowPins = { 79, 78, 76, 75 }; // پین‌های 26, 24, 21, 19
        int[] colPins = { 74, 73, 71 };     // پین‌های 18, 16, 12
        char[] keys = {
                '1', '2', '3',
                '4', '5', '6',
                '7', '8', '9',
                '*', '0', '#'
            };

        using KeyMatrix keypad = new KeyMatrix(rowPins, colPins, TimeSpan.FromMilliseconds(20));

        keypad.KeyEvent += (sender, keyEvent) =>
        {
            if (keyEvent.EventType == PinEventTypes.Falling)
            {
                // پیدا کردن کاراکتر کلید بر اساس ایندکس
                char pressedKey = keys[keyEvent.Input];
                Console.WriteLine($"[Event] KeyPresed : {pressedKey}");
            }
        };

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
