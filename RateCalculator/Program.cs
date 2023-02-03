using RateCalculator.Models;
using Microsoft.EntityFrameworkCore;
using RateCalculator.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaxScheduleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaxScheduleContext")));
builder.Services.AddScoped<ITaxService, TaxService>();

builder.Services.AddTransient<ExceptionMiddleware>();

builder.Services.AddControllers();

builder.Services.AddTransient<TaxScheduleValidation>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }