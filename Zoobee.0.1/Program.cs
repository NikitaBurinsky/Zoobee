#define DEV

using Zoobee.Application.ServiceCollectionExtensions;
using Zoobee.Infrastructure.ServiceCollectionExtensions;
using Serilog;
using Serilog.Events;
using Zoobee.Web.Views;
using Zoobee.Infrastructure.Parsers.Program_Configuration.Building;
using Zoobee.Web.ProgramConfigurators.AppPreRun;
using Zoobee.Infrastructure.Parsers.ProgramConfigurators.AppPreRun;
using Zoobee.Infrastructure.Program_Configuration.Assembly_Validation;
using Zoobee.Web.ProgramConfigurators.App_PreRun;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration, false);
builder.Services.AddInfrastructureParsers(builder.Configuration, false);
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
app.InfrastructureAssemplyCheck();

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
#if DEV
if(!app.SeedCountries("C:\\Users\\Formatis\\Documents\\GitHub\\Zoobee\\Zoobee.0.1\\Seeding\\countries.json"))
	throw new Exception("FHFHFHF Countries Seeding Failed");

#endif

app.ScrapingUrlsSeeding();
app.Run();

public partial class Program { }

