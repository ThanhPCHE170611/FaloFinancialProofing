using FALOFinancialProofing.Attributes;
using FALOFinancialProofing.DTOs;
using FALOFinancialProofing.Extensions;
using FALOFinancialProofing.Helpers;
using FALOFinancialProofing.Models;
using FALOFinancialProofing.Repository;
using FALOFinancialProofing.Services;
using FALOFinancialProofing.Services.AccountingBookServices;
using FALOFinancialProofing.Services.ApproveProcessServices;
using FALOFinancialProofing.Services.AttachmentFIleServices;
using FALOFinancialProofing.Services.BankServices;
using FALOFinancialProofing.Services.CampaignMemberService;
using FALOFinancialProofing.Services.CampaignRequestApproveHistoryServices;
using FALOFinancialProofing.Services.CampaignService;
using FALOFinancialProofing.Services.CreateCampaignFileServices;
using FALOFinancialProofing.Services.CreateCampaignRequestServices;
using FALOFinancialProofing.Services.CreateProjectFileServices;
using FALOFinancialProofing.Services.CreateProjectRequestApproveHistoryServices;
using FALOFinancialProofing.Services.CreateProjectRequestServices;
using FALOFinancialProofing.Services.CreateQrCodeServices;
using FALOFinancialProofing.Services.DebManagementServices;
using FALOFinancialProofing.Services.EmailService;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestHistoryService;
using FALOFinancialProofing.Services.MoveNextCampaignStatusRequestServices;
using FALOFinancialProofing.Services.OrganizationMemberServices;
using FALOFinancialProofing.Services.OrganizationServices;
using FALOFinancialProofing.Services.ProjectServices;
using FALOFinancialProofing.Services.RequestFormServices;
using FALOFinancialProofing.Services.SDGServices;
using FALOFinancialProofing.Services.SocialNetworkService;
using FALOFinancialProofing.Services.TransactionLogsServices;
using FALOFinancialProofing.Services.UserSDGServices;
using FALOFinancialProofing.Services.VoucherServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
            builder.Services.AddHttpClient();
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            builder.Services.AddScoped(typeof(AuthServices));
            builder.Services.AddScoped(typeof(RoleService));
            builder.Services.AddScoped<ITransactionLogService, TransactionLogService>();

            //builder.Services.AddHostedService<BankAccountPolling>(serviceProvider =>
            //{
            //    var iServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            //    var bankService = iServiceScopeFactory.CreateAsyncScope().ServiceProvider.GetRequiredService<IBankService>();
            //    return new BankAccountPolling(configuration, bankService);
            //});

            builder.Services.AddScoped<ICreateQrCodeService, CreateQrCodeService>();
            builder.Services.AddScoped<IBankService, BankService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISDGServices, SDGServices>();
            builder.Services.AddScoped<IOrganizationService, OrganizationService>();
            builder.Services.AddScoped<ICreateProjectRequestService, CreateProjectRequestService>();
            builder.Services.AddScoped<ICreateProjectFileService, CreateProjectFileService>();
            builder.Services.AddScoped<ISocialNetworkService, SocialNetworkService>();
            builder.Services.AddScoped<IRequestFormServices, RequestFormServices>();
            builder.Services.AddScoped<IAttachmentFileServices, AttachmentFileServices>();
            builder.Services.AddScoped<IApproveProcessServices, ApproveProcessServices>();
            builder.Services.AddScoped<IVoucherServices, VoucherServices>();
            builder.Services.AddScoped<IAcccountingBookServices, AccountingBookServices>();
            builder.Services.AddScoped<ICreateCampaignFileService, CreateCampaignFileService>();
            builder.Services.AddScoped<ICreateCampaignRequestService, CreateCampaignRequestService>();
            builder.Services.AddScoped<IMoveNextCampaignStatusRequestService, MoveNextCampaignStatusRequestService>();
            builder.Services.AddScoped<IMoveNextCampaignStatusRequestHistoryService, MoveNextCampaignStatusRequestHistoryService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IUserSDGService, UserSDGService>();
            builder.Services.AddScoped<ICampaignService, CampaignService>();
            builder.Services.AddScoped<ICampaignMemberService, CampaignMemberService>();
            builder.Services.AddScoped<IOrganizationMemberService, OrganizationMemberService>();
            builder.Services.AddScoped<ICreateProjectRequestApproveHistoryService, CreateProjectRequestApproveHistoryService>();
            builder.Services.AddScoped<ICampaignRequestApproveHistoryService, CampaignRequestApproveHistoryService>();
            builder.Services.AddScoped<IDebManagementServices, DebManagementServices>();
            builder.Services.AddHttpClient("MyHttpClient", client =>
            {
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddScoped(typeof(BankService1));
            builder.Services.AddScoped(typeof(WebHookService));
            builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ trong để lưu trữ session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thiết lập thời gian timeout cho session
                options.Cookie.HttpOnly = true; // Chỉ cho phép cookie session qua HTTP
                options.Cookie.IsEssential = true; // Đánh dấu cookie session là cần thiết
            });

            // Add Email Configs
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);


            //builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddControllers();
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 52428800; // Giới hạn 50Mb chẳng hạn
            });
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
            builder.Services.AddIdentity<User, Role>(options =>
            {
                // Cấu hình thời gian hết hạn token
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            }

                ).AddEntityFrameworkStores<FALOFinancialProofingDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                //options.TokenLifespan = TimeSpan.FromHours(1);
                options.TokenLifespan = TimeSpan.FromMinutes(10);
            });
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
            });
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
            //app.UseMiddleware<StaticFileMiddleware>();
            app.CustomStaticFiles(); // folder upload
            app.UseStaticFiles();
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}