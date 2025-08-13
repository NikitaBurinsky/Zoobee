using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using ZooStorages.Application.ServiceCollectionExtensions;
using ZooStorages.Infrastructure.ServiceCollectionExtensions;
using ZooStores.Infrastructure.Repositoties;
using ZooStores.Web.ProgramConfigurators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
	.AddJsonOptions(options =>
	{ options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

builder.Services.AddSwaggerEndpointsDocumentation();
builder.Services.AddLocalizationResources();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(o =>
{ o.AddPolicy("MyPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });

// Build
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	app.RolesInitialization();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseCors("MyPolicy");
app.MapControllers();
app.Run();

public partial class Program { }
