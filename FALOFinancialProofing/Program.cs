
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FALOFinancialProofing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            //builder.Services.AddControllers();
            builder.Services.AddDbContext<FALOFinancialProofingDbContext>(options =>
            {
                // Đọc chuỗi kết nối
                string connectstring = builder.Configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine("Kết nối thành công");
                // Sử dụng MS SQL Server
                options.UseSqlServer(connectstring);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
