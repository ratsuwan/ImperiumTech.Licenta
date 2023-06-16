global using MagazinWebLicenta.Shared;
global using System.Net.Http.Json;
global using MagazinWebLicenta.Client.Services.ProductService;
global using MagazinWebLicenta.Client.Services.CategoryService;
global using MagazinWebLicenta.Client.Services.AuthService;
global using Microsoft.AspNetCore.Components.Authorization;
global using MagazinWebLicenta.Client.Services.CartService;
global using MagazinWebLicenta.Client.Services.OrderService;
global using MagazinWebLicenta.Client.Services.AddressService;
global using MagazinWebLicenta.Client.Services.ProductTypeService;
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
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();



await builder.Build().RunAsync();
