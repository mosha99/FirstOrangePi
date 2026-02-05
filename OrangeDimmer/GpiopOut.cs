using Microsoft.AspNetCore.Mvc;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Device.Pwm.Drivers;

public record GpiopOut : IGpiop
{
    public GpiopOut(GpioController Controller, string name, int pinNumber)
    {
        this.Controller = Controller;
        Name = name;
        PinNumber = pinNumber;
    }
    private SoftwarePwmChannel Channel { set; get; } = null!;
    private GpioController Controller { get; }
    public string Name { get; }
    public int PinNumber { get; }

    public void On()
        => Controller.Write(PinNumber, PinValue.High);

    public void Of()
        => Controller.Write(PinNumber, PinValue.Low);

    public void SetPwm(double dutyCycle)
    {
        // بسیار مهم: اگر پین قبلاً باز شده، آن را می‌بندیم تا Busy نباشد
        if (Controller.IsPinOpen(PinNumber))
        {
            Controller.ClosePin(PinNumber);
        }
        Channel?.Dispose();
        Channel = new SoftwarePwmChannel(PinNumber, 400, dutyCycle, usePrecisionTimer: true);
        Channel.Start();
    }
}
