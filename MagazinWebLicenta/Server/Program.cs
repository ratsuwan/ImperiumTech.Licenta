global using MagazinWebLicenta.Shared;
global using Microsoft.EntityFrameworkCore;
global using MagazinWebLicenta.Server.Data;
global using MagazinWebLicenta.Server.Services.ServiciulProduse;
global using MagazinWebLicenta.Server.Services.CategoryService;
global using MagazinWebLicenta.Server.Services.CartService;
global using MagazinWebLicenta.Server.Services.AuthService;
global using MagazinWebLicenta.Server.Services.OrderService;
global using MagazinWebLicenta.Server.Services.PaymentService;
global using MagazinWebLicenta.Server.Services.ServiciulAdresa;
global using MagazinWebLicenta.Server.Services.ServiciulTipuriDeProduse;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiciulProduse, ServiciulProduse>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IServiciulAdresa, ServiciulAdresa>();
builder.Services.AddScoped<IServiciulTipulDeProduse, ServiciulTipuriDeProduse>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = 
                   new SymmetricSecurityKey(System.Text.Encoding.UTF8
                   .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
builder.Services.AddHttpContextAccessor();



var app = builder.Build();

app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
