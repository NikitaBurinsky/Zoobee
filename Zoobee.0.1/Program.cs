using Zoobee.Application.ServiceCollectionExtensions;
using Zoobee.Infrastructure.Parsers.Program_Configuration.Pipelines;
using Zoobee.Infrastructure.ServiceCollectionExtensions;
using Zoobee.Web.ProgramConfigurators.Building;
using Zoobee.Web.ProgramConfigurators.Startup;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console(LogEventLevel.Information)
	.WriteTo.File($"{builder.Configuration["Logging:LogsStorage:LogsFolderPath"]}/Logs/log-{DateTime.Now}.txt")
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration, true);
builder.Services.AddParsers(builder.Configuration, false);
builder.Services.AddRepositories();
builder.Services.AddPresentationLayer();

builder.Host.UseSerilog();

builder.Services.AddCors(o =>
{
	o.AddPolicy("DevelopmentAllowAny",
	builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Build
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseCors("DevelopmentAllowAny");
app.MapControllers();
app.RolesSeeding();
app.Run();

public partial class Program { }

