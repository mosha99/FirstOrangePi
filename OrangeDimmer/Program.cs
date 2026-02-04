var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGpioService, GpioService>();
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