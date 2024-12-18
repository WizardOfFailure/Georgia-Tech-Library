using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddCacheManager(x => x.WithDictionaryHandle()); ;


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add monitoring


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Prometheus monitoring
app.UseHttpMetrics(options =>

{

    options.RequestDuration.Enabled = true; // Measure request duration

    options.RequestCount.Enabled = true;
}    // Measure request count

);

app.UseMetricServer();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseMetricServer();

await app.UseOcelot();

app.Run();
