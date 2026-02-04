using Microsoft.AspNetCore.Mvc;
using System.Device.Gpio;

public class GpioService : IGpioService, IDisposable
{
    private GpioController GpioController { get; }

    public event Action<bool>? OnDoorStatusChanged;
    public GpioService(GpioController gpioController)
    {
        GpioController = gpioController;
    }

    public Dictionary<string, IGpiop> RegisteredPins = [];

    public TPin? GetPin<TPin>(string name)
        where TPin : class, IGpiop
    {
        if (RegisteredPins.TryGetValue(name, out var value) && value is TPin pin)
            return pin;

        return null;
    }

    public GpiopOut RegisterOutPin(string name, int pinNumber)
    {
        GpiopOut pin;

        if (RegisteredPins.ContainsKey(name))
        {
            pin = (RegisteredPins[name] as GpiopOut)!;

            if (pin.PinNumber != pinNumber)
                throw new InvalidOperationException($"Pin with name {name} is already registered with a different pin number.");

            return pin;
        }

        GpioController.OpenPin(pinNumber, PinMode.Output);

        pin = new GpiopOut(GpioController, name, pinNumber);

        RegisteredPins.Add(name, pin);

        return pin;
    }

    public GpiopIn RegisterInPin(string name, int pinNumber)
    {
        GpiopIn pin;

        if (RegisteredPins.ContainsKey(name))
        {
            pin = (RegisteredPins[name] as GpiopIn)!;

            if (pin.PinNumber != pinNumber)
                throw new InvalidOperationException($"Pin with name {name} is already registered with a different pin number.");

            return pin;
        }

        GpioController.OpenPin(pinNumber, PinMode.InputPullUp);

        pin = new GpiopIn(GpioController, name, pinNumber);

        RegisteredPins.Add(name, pin);

        return pin;
    }

    public void Dispose()
    {
        GpioController.Dispose();
    }

    public IList<TPin> GetAllPin<TPin>() where TPin : class, IGpiop
    {
        return RegisteredPins?.Values?
             .Select(x => (x as TPin)!)?
             .Where(x => x is not null)?.ToList() ?? [];
    }
}
