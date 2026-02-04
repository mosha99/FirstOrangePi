using System.Device.Gpio;

public class GpioService : IGpioService, IDisposable
{
    private readonly GpioController _controller;
    private const int LightPin = 231;

    public GpioService()
    {
        _controller = new GpioController();
        _controller.OpenPin(LightPin, PinMode.Output);
    }

    public void ToggleLight(bool state)
    {
        _controller.Write(LightPin, state ? PinValue.High : PinValue.Low);
        Console.WriteLine($"[Hardware] PH7 Light is now {(state ? "ON" : "OFF")} [cite: 2026-02-04]");
    }

    public void Dispose()
    {
        _controller.Dispose();
    }
}
