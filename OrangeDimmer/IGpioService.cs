using System.Device.Gpio;

public interface IGpioService
{
    public IList<TPin> GetAllPin<TPin>() where TPin : class, IGpiop;
    public GpiopOut RegisterOutPin(string name, int pinNumber);

    public GpiopIn RegisterInPin(string name, int pinNumber);
}
