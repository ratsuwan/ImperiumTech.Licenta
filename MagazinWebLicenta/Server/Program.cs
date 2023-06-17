global using MagazinWebLicenta.Shared;
global using Microsoft.EntityFrameworkCore;
global using MagazinWebLicenta.Server.Data;
global using MagazinWebLicenta.Server.Services.ServiciulProduse;
global using MagazinWebLicenta.Server.Services.ServiciulCategorii;
global using MagazinWebLicenta.Server.Services.ServiciulCosCumparaturi;
global using MagazinWebLicenta.Server.Services.ServiciulAutentificari;
global using MagazinWebLicenta.Server.Services.ServiciulComenzi;
global using MagazinWebLicenta.Server.Services.ServiciulPlati;
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
builder.Services.AddScoped<IServiciulCategorii, ServiciulCategorii>();
builder.Services.AddScoped<IServiciulCosCumparaturi, ServiciulCosCumparaturi>();
builder.Services.AddScoped<IServiciulAutentificari, ServiciulAutentificari>();
builder.Services.AddScoped<IServiciulComenzi, ServiciulComenzi>();
builder.Services.AddScoped<IServiciulPlati, ServiciulPlati>();
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
