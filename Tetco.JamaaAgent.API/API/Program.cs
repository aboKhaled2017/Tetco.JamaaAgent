using API;
using API.CrossCuttings.MiddleWares;
using API.CrossCuttings.OpenAPI;
using Application;
using Domain.Common.Settings;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder ( args );

var generalSetting = builder.Configuration.GetSection("GeneralSetting").Get<GeneralSetting>();
builder.Services.AddSingleton(generalSetting);


// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error() // Log only errors
    .WriteTo.Console() // Log to the console
    .WriteTo.File(GetLogFilePath(), rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog(); // Use Serilog for logging



// Add services to the container.
builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices ( builder.Configuration , generalSetting);

//builder.Services.AddAutomatedAutorest ( );
builder.Services.AddControllers ( );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAPIServices ( );

builder.Services.AddEndpointsApiExplorer ( );
builder.Services.AddAgentSwaggerAPIs();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    
    //app.UseMiddleware<AutoRestMiddleware>();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection ( );

app.UseAuthentication ( );
app.UseAuthorization ( );

app.UseMiddleware<JamaaAgentExceptionMiddleWare> ( );

app.MapControllers ( );

app.Run ( );

string GetLogFilePath()
{
    string logsDirectory = "logs";
    string logFileName = $"log-{DateTime.Now:yyyy-MM-dd}.txt";
    string logFilePath = Path.Combine(logsDirectory, logFileName);

    // Create the logs directory if it doesn't exist
    Directory.CreateDirectory(logsDirectory);

    return logFilePath;
}