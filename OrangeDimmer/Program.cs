using System.Device.Gpio;

// نام فنی: PH7 | پین فیزیکی: 19
int pin = 231;
using GpioController controller = new GpioController();
controller.OpenPin(pin, PinMode.Output);

Console.WriteLine("Dimmer started with .NET 8.0 on Pin 19 (PH7)...");

while (true)
{
    // از صفر تا صد درصد روشنایی
    for (int i = 0; i <= 100; i += 2)
    {
        ExecutePwm(controller, pin, i);
    }
    // از صد تا صفر درصد روشنایی
    for (int i = 100; i >= 0; i -= 2)
    {
        ExecutePwm(controller, pin, i);
    }
}

void ExecutePwm(GpioController controller, int pinNum, int dutyCycle)
{
    // ایجاد یک چرخه 10 میلی‌ثانیه‌ای
    int onTime = dutyCycle / 10;
    int offTime = 10 - onTime;

    if (onTime > 0)
    {
        controller.Write(pinNum, PinValue.High);
        Thread.SpinWait(onTime);
    }
    if (offTime > 0)
    {
        controller.Write(pinNum, PinValue.Low);
        Thread.SpinWait(offTime);
    }
}