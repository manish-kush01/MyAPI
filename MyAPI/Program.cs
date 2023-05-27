using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyAPI.Data;
using MyAPI.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//to log the instances in a log file (Serilog nuget package)
//Log.Logger=new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//    .File("log/myApilogs.txt",rollingInterval:RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog();

builder.Services.AddControllers(/*Options=>*/
//{ Options.ReturnHttpNotAcceptable = true; }
).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

//Entityframework using to connect to DB (registering dependency injection)
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//to add our custom logging in the container [different types for different periods(singleton, scoped, transient)]
builder.Services.AddSingleton<ILogging, Loggingv2>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
