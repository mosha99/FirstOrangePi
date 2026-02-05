using Microsoft.AspNetCore.SignalR;

public class SmartHub : Hub
{
    private readonly IGpioService _gpioService;

    public SmartHub(IGpioService gpioService)
    {
        _gpioService = gpioService;
    }

    public async Task SetLightState(bool isOn)
    {
        /*var pin = _gpioService.RegisterOutPin("Light", 231);

        if (isOn) pin.On();
        else pin.Of();*/

        // اطلاع‌رسانی به همه کلاینت‌ها درباره وضعیت جدید
        await Clients.All.SendAsync("ReceiveLightStatus", isOn);
    }

    public async Task SetDimmerLevel(int level)
    {
        // تبدیل درصد به مقدار قابل فهم برای درایور (مثلاً 0.0 تا 1.0)
        double dutyCycle = level / 100.0;

        var pin = _gpioService.RegisterOutPin("Light", 231);

        pin.SetPwm(dutyCycle); // فرض بر این است که متد SetPwm را در سرویس داری

        // اطلاع‌رسانی به بقیه کلاینت‌ها برای هماهنگی اسلایدرها
        await Clients.All.SendAsync("ReceiveDimmerLevel", level);

        Console.WriteLine($"[SignalR] PH7 Dimmer set to: {level}% [cite: 2026-02-04]");
    }
}