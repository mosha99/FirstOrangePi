using System.Device.Gpio;
using System.Diagnostics;

// نام فنی: PH7 | پین فیزیکی: 19 [cite: 2026-02-04]
int pin = 231;
using GpioController controller = new GpioController();
controller.OpenPin(pin, PinMode.Output);

Console.WriteLine("Fast & Smooth Dimmer started on PH7... [cite: 2026-02-04]");

// استفاده از Stopwatch برای زمان‌بندی دقیق‌تر نانوثانیه‌ای
Stopwatch sw = new Stopwatch();

while (true)
{
    // Fade In & Out با گام‌های ۱ درصدی برای نرمی کامل
    for (int i = 0; i <= 200; i++)
    {
        int dutyCycle = i <= 100 ? i : 200 - i;

        // اجرای PWM سریع (فرکانس حدود 100 هرتز برای حذف لرزش)
        long totalTicks = Stopwatch.Frequency / 100; // 10ms cycle
        long onTicks = totalTicks * dutyCycle / 100;

        sw.Restart();
        controller.Write(pin, PinValue.High);
        while (sw.ElapsedTicks < onTicks) ; // Wait precision

        controller.Write(pin, PinValue.Low);
        while (sw.ElapsedTicks < totalTicks) ; // Wait precision
    }
}