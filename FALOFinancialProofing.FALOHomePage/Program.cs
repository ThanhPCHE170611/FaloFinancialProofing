using FALOFinancialProofing.FALOHomePage.Services;

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
            builder.Services.AddSession();
            builder.Services.AddHttpClient();

            //Build Session Service
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            builder.Services.AddMemoryCache();

            builder.Services.AddHostedService<BankDataPollingService>();
            builder.Services.AddTransient<TransactionPollingDirect>();
            builder.Services.AddTransient<BankAccountService>();

            var app = builder.Build();

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
