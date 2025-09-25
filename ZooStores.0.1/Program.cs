using System.Text.Json.Serialization;
using Zoobee.Application.ServiceCollectionExtensions;
using Zoobee.Infrastructure.ServiceCollectionExtensions;
using Zoobee.Web.ProgramConfigurators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
	.AddJsonOptions(options =>
	{ options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationLayer();

builder.Services.AddCors(o =>
{ o.AddPolicy("DevelopmentAllowAny", 
	builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });

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
