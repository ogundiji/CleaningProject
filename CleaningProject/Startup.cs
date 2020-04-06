using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CleaningProject.Models;
using CleaningProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;
using ScottBrady91.AspNetCore.Identity;


namespace CleaningProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .Build());

            services.AddMvc();

            var connectionString = Configuration["connection"];
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<CleaningUserDbContext>(options => options.UseSqlServer(connectionString
                , sql => sql.MigrationsAssembly(migrationAssembly)));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(20);
            });
            services.AddIdentity<CleaningUser, IdentityRole>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = "emailConfig";

                //password configurations
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 6;
                options.User.RequireUniqueEmail = true;

                //lockout configurations
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 4;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
            .AddEntityFrameworkStores<CleaningUserDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailConfirmationTokenProvider<CleaningUser>>("emailConfig")
            .AddPasswordValidator<DoesNotContainPasswordValidator<CleaningUser>>();

            services.AddScoped<IPasswordHasher<CleaningUser>, BCryptPasswordHasher<CleaningUser>>();



            services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromHours(3));

            services.Configure<EmailConfirmationTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromDays(2));

          
            services.AddScoped<IUserClaimsPrincipalFactory<CleaningUser>, CleaningUserClaimsPrincipalFactory>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");

            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddScoped<IUserImage, UserImageImp>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEquipment, EquipmentRepository>();
            services.AddScoped<IService, ServiceImp>();
            services.AddScoped<IServiceType, ServiceTypeImp>();
            services.AddScoped<IServiceConfig, ConfigureService>();
            services.AddScoped<IRequestService, ServiceRequestImp>();
            services.AddScoped<ITeam, TeamRepository>();
            services.AddScoped<IConfigureTeam, ConfigureTeamRepository>();
            services.AddScoped<ICleaning, CleaningItemImp>();
            services.AddScoped<IEquipmentTracking, EquipmentTrackingImp>();
            services.AddScoped<IAppointment, AppointmentImpl>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IEmailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();

            // StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
           
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            RotativaConfiguration.Setup(env);
            CreateUserRole(serviceProvider).Wait();
        }
       private async Task CreateUserRole(IServiceProvider serviceProvider)
       {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "SuperAdmin", "Staff", "Customer" };
            IdentityResult roleResult;
            foreach (var k in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(k);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(k));
                }
            }
       }
    }
}
