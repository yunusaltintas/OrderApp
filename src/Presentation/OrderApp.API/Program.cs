using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Application;
using OrderApp.Application.Dtos.Requests.Validator;
using OrderApp.Application.Extension;
using OrderApp.Application.Filters;
using OrderApp.Application.Middlewares;
using OrderApp.Application.SystemsModels;
using OrderApp.Infrastructure;
using OrderApp.Persistence;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

SeriLogExtension.ConfigureLoggin();
builder.Host.UseSerilog();

builder.Services.AddControllers(o =>
{
    o.Filters.Add(new ValidationFilter());
}).AddFluentValidation(v =>
{
    v.RegisterValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMqSystemModel>(builder.Configuration.GetSection("RabbitMqSystemModel"));
builder.Services.Configure<SmtpSystemModel>(builder.Configuration.GetSection("SmtpSystemModel"));

builder.Services.AddApplicationRegistration();
builder.Services.AddPersistanceServices(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddInfrastructureServices();

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<RequestResponseMiddleware>();
app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoint => { endpoint.MapControllers(); });

app.DatabaseInitialize();

app.Run();
