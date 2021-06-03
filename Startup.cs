using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.Data;

namespace WebStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                //    var supportedCultures = new[]
                //{
                //    new CultureInfo("en-US"),
                //    new CultureInfo("en-GB"),
                //    new CultureInfo("en"),
                //    new CultureInfo("ru-RU"),
                //    new CultureInfo("ru"),
                //    new CultureInfo("de-DE"),
                //    new CultureInfo("de")
                //};
                //    app.UseRequestLocalization(new RequestLocalizationOptions
                //    {
                //        DefaultRequestCulture=new RequestCulture("en-GB"),
                //        SupportedCultures=supportedCultures,
                //        SupportedUICultures=supportedCultures
                //    });
                app.UseMigrationsEndPoint();
            }
            else
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

            // Say to application: "Use the session"
            app.UseSession();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapRazorPages();
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
              name: "areas",
              template: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
            );
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();
            services.AddControllersWithViews()
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization();
            services.AddRazorPages();
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("en-GB");
                options.AddSupportedUICultures("en-GB", "de-DE", "ja-JP", "fr-FR", "ar-sa", "uk-UA", "zh-CN", "ko-KR", "hi-IN", "he-IL");
            });

            // Add dpendency MVC Route
            services.AddMvc(options => options.EnableEndpointRouting = false);

            // Add midleware logic Sessions and cookie
            services.AddSession(options =>
            {
                // Wating session time
                options.IdleTimeout = TimeSpan.FromMinutes(30);

                // Activating cookie
                options.Cookie.HttpOnly = true;
            });

            services.AddDatabaseDeveloperPageExceptionFilter();
        }
    }
}