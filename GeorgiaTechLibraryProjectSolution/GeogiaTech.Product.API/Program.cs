using System.Reflection;
using GeorgiaTech.Data.Persistence;
using Microsoft.AspNetCore.Components;
//using FluentAssertions.Common;
using MediatR;
using Prometheus;
//using Serilog;
using GeorgiaTech.Application;
using GeorgiaTech.Application.Contracts;
using GeogiaTech.Product.API.Utilities;
using GeorgiaTech.Product.Application;
using GeorgiaTech.Product.Persistence;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//custom services
builder.Services.AddScoped<DataContext, DataContext>();
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//builder.Services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

//add the instance singleton
builder.Services.AddSingleton<InstanceHelper>();


//add healthchecks
builder.Services.AddHealthChecks();

//use serilog
var configuration = builder.Configuration;
string seqUrl = configuration.GetValue<string>("Settings:SeqLogAddress");
Console.WriteLine("Starting up");
Console.WriteLine($"SeqUrl: {seqUrl}");
// Serilog configuration
/*Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "Product.Api") // Enrich with the tag "Service" and the name of this service
    .WriteTo.Seq(seqUrl)
    .CreateLogger();*/
//add serilog
//add the logger
builder.Logging.ClearProviders();
/*builder.Logging.AddSerilog();
builder.Services.AddReviewApplicationServices();
builder.Services.AddReviewInfrastructureServices();
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
//always show swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
