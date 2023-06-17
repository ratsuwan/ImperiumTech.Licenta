global using MagazinWebLicenta.Shared;
global using System.Net.Http.Json;
global using MagazinWebLicenta.Client.Services.ServiciulProduse;
global using MagazinWebLicenta.Client.Services.ServiciulCategorii;
global using MagazinWebLicenta.Client.Services.ServiciulAutentificari;
global using Microsoft.AspNetCore.Components.Authorization;
global using MagazinWebLicenta.Client.Services.ServiciulCosCumparaturi;
global using MagazinWebLicenta.Client.Services.ServiciulComenzi;
global using MagazinWebLicenta.Client.Services.ServiciulAdresa;
global using MagazinWebLicenta.Client.Services.ServiciulTipuriDeProduse;
using MagazinWebLicenta.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using MudBlazor.Services;

;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IServiciulProduse, ServiciulProduse>();
builder.Services.AddScoped<IServiciulCategorii, ServiciulCategorii>();
builder.Services.AddScoped<IServiciulCosCumparaturi, ServiciulCosCumparaturi>();
builder.Services.AddScoped<IServiciulAutentificari, ServiciulAutentificari>();
builder.Services.AddScoped<IServiciulComenzi, ServiciulComenzi>();
builder.Services.AddScoped<IServiciulAdresa, ServiciulAdresa>();
builder.Services.AddScoped<IServiciulTipuriDeProduse, ServiciulTipuriDeProduse>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();



await builder.Build().RunAsync();
