using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(assembly);
    conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
    conf.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddCarter();

builder.Services.AddMarten(conf =>
{
    conf.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment()) 
    builder.Services.InitializeMartenWith<CatalogInitialData>();


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(opt => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();