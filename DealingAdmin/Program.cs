using DealingAdmin;
using DealingAdmin.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrading.SettingsReader;
using System;

var builder = WebApplication.CreateBuilder(args);

var liveDemoManager = new LiveDemoServiceMapper();
var settingsModel = SettingsReader.ReadSettings<SettingsModel>();

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();

builder.Services.BindLogger(settingsModel);
var serviceBusTcpClient = builder.Services.BindServiceBus(settingsModel);
builder.Services.BindGrpcServices(settingsModel);

builder.Services.BindMyNoSql(settingsModel, liveDemoManager);

settingsModel.BindPostgresRepositories(liveDemoManager);
builder.Services.BindAzureStorage(settingsModel);
builder.Services.InitLiveDemoManager(liveDemoManager);
builder.Services.BindServices(settingsModel);
var app = builder.Build();

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

AppJobService.Init();
AppJobService.Start();

app.Run();
