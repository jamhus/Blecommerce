global using Blecommerce.Shared;
global using Blecommerce.Shared.DTOS;

global using System.Net.Http.Json;
global using Blecommerce.Client.Services.ProductService;
global using Blecommerce.Client.Services.CategoryService;
global using Blecommerce.Server.Services.CartService;

using Blecommerce.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
