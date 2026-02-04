using System.Device.Gpio;
using System.Threading;

// نام فنی: PH7 | شماره لاین: 231 [cite: 2026-02-04]
int pin = 231;
using GpioController controller = new GpioController();
controller.OpenPin(pin, PinMode.Output);

Console.WriteLine("Dimmer Started on PH7 (Pin 19)... [cite: 2026-02-04]");

while (true)
{
    // فاز روشن شدن (Fade In)
    for (int i = 0; i <= 100; i += 5)
    {
        for (int repeat = 0; repeat < 5; repeat++) // تکرار برای ماندگاری در هر پله
            ExecutePwm(controller, pin, i);
    }
    // فاز خاموش شدن (Fade Out)
    for (int i = 100; i >= 0; i -= 5)
    {
        for (int repeat = 0; repeat < 5; repeat++)
            ExecutePwm(controller, pin, i);
    }
}

void ExecutePwm(GpioController controller, int pinNum, int dutyCycle)
{
    // کل زمان چرخه: 20 میلی‌ثانیه (فرکانس 50 هرتز)
    int totalTime = 20;
    int onTime = (dutyCycle * totalTime) / 100;
    int offTime = totalTime - onTime;

    if (onTime > 0)
    {
        controller.Write(pinNum, PinValue.High);
        Thread.Sleep(onTime);
    }
    if (offTime > 0)
    {
        controller.Write(pinNum, PinValue.Low);
        Thread.Sleep(offTime);
    }
}