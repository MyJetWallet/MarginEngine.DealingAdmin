using DealingAdmin;
using DealingAdmin.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrading.SettingsReader;
using MudBlazor.Services;
using SimpleTrading.ServiceStatusReporterConnector;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var liveDemoManager = new LiveDemoServiceMapper();
var settingsModel = SettingsReader.ReadSettings<SettingsModel>();

// Add services to the container
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.BindLogger(settingsModel);
var serviceBusTcpClient = builder.Services.BindServiceBus(settingsModel);
builder.Services.BindGrpcServices(liveDemoManager, settingsModel);

var myNoSqlTcpClient = builder.Services.BindMyNoSql(liveDemoManager, settingsModel);
builder.Services.BindPostgresRepositories(liveDemoManager, settingsModel);
builder.Services.BindAzureStorage(settingsModel);
builder.Services.InitLiveDemoManager(liveDemoManager);
builder.Services.BindServices(settingsModel);
builder.Services.AddAntDesign();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
});

var app = builder.Build();

app.BindIsAlive();
app.BindServicesTree(Assembly.GetExecutingAssembly());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

serviceBusTcpClient.Start();
myNoSqlTcpClient.Start();

AppJobService.Init();
AppJobService.Start();

app.Run();
