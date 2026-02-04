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
        _gpioService.ToggleLight(isOn);

        // اطلاع‌رسانی به همه کلاینت‌ها درباره وضعیت جدید
        await Clients.All.SendAsync("ReceiveLightStatus", isOn);
    }
}