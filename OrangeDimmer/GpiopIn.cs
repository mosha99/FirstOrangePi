using System.Device.Gpio;

public class GpiopIn : IGpiop
{
    private Action<PinEventTypes> OnPinChange { set; get; } = null!;
    public GpiopIn(GpioController Controller, string name, int pinNumber)
    {
        this.Controller = Controller;
        Name = name;
        PinNumber = pinNumber;

        Controller.RegisterCallbackForPinValueChangedEvent(
            pinNumber,
            PinEventTypes.Falling | PinEventTypes.Rising,
            (s, e) => OnPinChange?.Invoke(e.ChangeType));
    }

    private GpioController Controller { get; }
    public string Name { get; }
    public int PinNumber { get; }

    public void SubscribeToPinChange(Action<PinEventTypes> action)
        => OnPinChange += action;

    public void unsubscribeToPinChange(Action<PinEventTypes> action)
        => OnPinChange -= action;
}