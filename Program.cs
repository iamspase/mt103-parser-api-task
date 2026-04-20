using NLog.Web;
using SwiftMT103ApiTask.Models;
using SwiftMT103ApiTask.Repositories;
using SwiftMT103ApiTask.Services.MT103;
using SwiftMT103ApiTask.Utils;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
SQLitePCL.Batteries.Init();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Logging.ClearProviders();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Version = "v1",
        Title = "MT103 Swift message parser API",
        Description = "ASP.NET Core Web API for parsing and proccessing MT103 Swift messages."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IMT103Repository, MT103Repository>();
builder.Services.AddScoped<MT103Service>();

builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var connectionString = builder.Configuration.GetConnectionString("Default");

if(connectionString == null)
{
    throw new NullReferenceException("No connection string provided.");
}

DatabaseInitializer dbInitializer = new DatabaseInitializer(connectionString);
await dbInitializer.Initialize();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
