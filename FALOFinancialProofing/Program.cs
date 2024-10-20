using FALOFinancialProofing.Attributes;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.TransactionLogsServices;
using FALOFinancialProofing.Services.SDGServices;
using FALOFinancialProofing.Services.SocialNetworkService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Security.Claims;
using System.Text;
using FALOFinancialProofing.Services.OrganizationServices;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using FALOFinancialProofing.Services.CreateProjectFileServices;
using FALOFinancialProofing.Services.CreateCampaignFileServices;
using FALOFinancialProofing.Services.CreateCampaignRequestServices;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices;
using FALOFinancialProofing.Services.ProjectServices;
using FALOFinancialProofing.Services.CampaignService;
using FALOFinancialProofing.Services.CampaignMemberService;

namespace FALOFinancialProofing
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Services.AddCors();
            // Add services to the container.
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            builder.Services.AddScoped(typeof(AuthServices));
            builder.Services.AddScoped<ITransactionLogService, TransactionLogService>();
            
            //builder.Services.AddScoped(typeof(AuthServices));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISDGServices, SDGServices>();
            builder.Services.AddScoped<IOrganizationService, OrganizationService>();
            builder.Services.AddScoped<ICreateProjectRequestService, CreateProjectRequestService>();
            builder.Services.AddScoped<ICreateProjectFileService, CreateProjectFileService>();
            builder.Services.AddScoped<ISocialNetworkService, SocialNetworkService>();
            builder.Services.AddScoped<ICreateCampaignFileService, CreateCampaignFileService>();
            builder.Services.AddScoped<ICreateCampaignRequestService, CreateCampaignRequestService>();
            builder.Services.AddScoped<IMoveNextCampaignStatusRequestService, MoveNextCampaignStatusRequestService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<ICampaignService, CampaignService>();
            builder.Services.AddScoped<ICampaignMemberService, CampaignMemberService>();


            // Add Email Configs
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);





            //builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                    }
                });
            });
            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<FALOFinancialProofingDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDbContext<FALOFinancialProofingDbContext>(options =>
            {
                // Đọc chuỗi kết nối
                string connectstring = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectstring);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("JwtAppsettings"));
            var secretKey = builder.Configuration["JwtAppsettings:SecretKey"];
            var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //options.SecurityTokenValidators.Clear();
                //options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler());
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = builder.Configuration["JwtAppsettings:Audience"],
                    ValidIssuer = builder.Configuration["JwtAppsettings:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),
                    ClockSkew = TimeSpan.Zero
                };
            }).AddGoogle(googleOptions =>
            {
                //googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                //googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];

                googleOptions.ClientId = "1079045769870-tn77k2e2psvlv5tdi7mak8i0nlqahr76.apps.googleusercontent.com";
                googleOptions.ClientSecret = "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";
            });
            ;
            builder.Services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdminOnly", policy
                //    => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                //options.AddPolicy("UserOnly", policy
                //                       => policy.RequireClaim("Role", "Staff"));


                //for (int age = 18; age < 23; age++)
                //{
                //    options.AddPolicy($"MinimumAge{age}", policy => policy.Requirements.Add(new MinimumAgeRequirement(age)));
                //}

            });
            builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
            var app = builder.Build();
            app.UseCors(option => option.AllowAnyHeader().
                AllowAnyMethod().AllowAnyOrigin());
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}