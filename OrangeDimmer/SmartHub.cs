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
        var pin = _gpioService.RegisterOutPin("Light", 231);

        if (isOn) pin.On();
        else pin.Of();

        // اطلاع‌رسانی به همه کلاینت‌ها درباره وضعیت جدید
        await Clients.All.SendAsync("ReceiveLightStatus", isOn);
    }
}