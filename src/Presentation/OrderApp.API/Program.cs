using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Application;
using OrderApp.Application.Dtos.Requests.Validator;
using OrderApp.Application.Middlewares;
using OrderApp.Infrastructure;
using OrderApp.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddFluentValidation(v =>
{
    v.RegisterValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationRegistration();
builder.Services.AddPersistanceServices(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddInfrastructureServices();

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
