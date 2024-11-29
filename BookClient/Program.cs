using BookClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Models;
using System.Reflection.Metadata;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ??
    throw new NullReferenceException("ApiBaseUrl not found");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddSingleton(sp =>
    new HubConnectionBuilder()
        .WithUrl($"{apiBaseUrl}{HubConstants.TableHubUrl}")
        .WithAutomaticReconnect()
        .Build());

await builder.Build().RunAsync();
