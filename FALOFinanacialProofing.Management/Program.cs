using FALOFinanacialProofing.Management;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using NuGet.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add other services

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie()
              .AddGoogle(options =>
              {

                  options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                  options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                  options.SaveTokens = true;
                  options.Scope.Add("openid");
                  options.Scope.Add("profile");
                  options.Scope.Add("email");
              });
builder.Services.AddAuthorization();
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(option => option.AllowAnyHeader().
               AllowAnyMethod().AllowAnyOrigin());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();
