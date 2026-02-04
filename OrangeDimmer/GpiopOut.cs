using System.Device.Gpio;

public record GpiopOut : IGpiop
{
    public GpiopOut(GpioController Controller, string name, int pinNumber)
    {
        this.Controller = Controller;
        Name = name;
        PinNumber = pinNumber;
    }

    private GpioController Controller { get; }
    public string Name { get; }
    public int PinNumber { get; }

    public void On()
        => Controller.Write(PinNumber, PinValue.High);

    public void Of()
        => Controller.Write(PinNumber, PinValue.Low);
}
