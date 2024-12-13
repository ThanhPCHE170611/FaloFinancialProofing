using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace FALOFinancialProofing.FALOHomePage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

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

            //Build Session Service
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.IsEssential = true; // Make session cookie essential for GDPR compliance
            });
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
                pattern: "{controller=Homepage}/{action=Index}/{id?}");

            app.UseResponseCaching();
            app.UseSession();
            app.MapRazorPages();
            app.Run();
        }
    }
}
