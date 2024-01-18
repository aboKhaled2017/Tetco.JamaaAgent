using API;
using API.CrossCuttings.MiddleWares;
using API.CrossCuttings.OpenAPI;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder ( args );

// Add services to the container.
builder.Services.AddApplicationServices ( );

builder.Services.AddInfrastructureServices ( builder.Configuration );

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
