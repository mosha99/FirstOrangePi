using System.Device.Gpio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGpioService, GpioService>();

// ۱. ثبت کنترلر اصلی سخت‌افزار
builder.Services.AddSingleton<System.Device.Gpio.GpioController>();

// ۲. ثبت سرویس منطقی شما که از کنترلر استفاده می‌کند
builder.Services.AddSingleton<IGpioService, GpioService>();

// ۳. ثبت ورکر پس‌زمینه برای مانیتورینگ سنسور PH8 (لاین 232) [cite: 2026-02-04]
builder.Services.AddHostedService<PortMonitoringWorker>();

builder.Services.AddSignalR();

// اجازه دسترسی به همه کلاینت‌ها برای تست
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .SetIsOriginAllowed(hostName => true)));

var app = builder.Build();

app.UseCors("CorsPolicy");
app.MapHub<SmartHub>("/smarthub");

app.Run("http://0.0.0.0:5000");

