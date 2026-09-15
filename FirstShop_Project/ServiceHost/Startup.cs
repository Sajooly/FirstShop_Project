using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_ShopQuery.Contracts;
using _01_ShopQuery.Query;
using AccountManagement.Configuration;
using BlogManagement.Configuration;
using CommentManagement.Configuration;
using DiscountManagement.Configuration;
using InventoryManagement.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopManagement.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace ServiceHost
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
            //HttpContextAccessor
            services.AddHttpContextAccessor();
            //services.AddScoped<IHttpContextAccessor, HttpContextAccessor>(); // این خط با خط بالایی فرقی نداره


            var connectionString = Configuration.GetConnectionString("FirstShop");

            ShopManagementBootstrapper.Configure(services, connectionString);
            DiscountManagementBootstrapper.Configure(services, connectionString);
            InventoryManagementBootstrapper.Configure(services, connectionString);
            BlogManagementBootstrapper.Configure(services, connectionString);
            CommentManagementBootstrapper.Configure(services, connectionString);
            AccountManagementBootstrapper.Configure(services, connectionString);



            //For Encode for HTMl
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
            //End

            services.AddTransient<IFileUploader, FileUploader>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IAuthHelper, AuthHelper>();


            //Authentication
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Account");
                    o.LogoutPath = new PathString("/Account");
                    o.AccessDeniedPath = new PathString("/AccessDenied");
                });
            //End


            //Add Policy For Authorization By Role-Based 

            services.AddAuthorization(option =>
            {
                option.AddPolicy("AdminArea",
                builder => builder.RequireRole(Roles.Adminstration, Roles.ContentUploader));

                option.AddPolicy("Shop",
              builder => builder.RequireRole(Roles.Adminstration));

                option.AddPolicy("Discount",
                    builder => builder.RequireRole(Roles.Adminstration));

                option.AddPolicy("Account",
                   builder => builder.RequireRole(Roles.Adminstration));
            });

            //End


            services.AddRazorPages()
                .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
                .AddRazorPagesOptions(option =>
                {
                    option.Conventions.AuthorizeAreaFolder("Administration", "/", "AdminArea");
                    option.Conventions.AuthorizeAreaFolder("Administration", "/Shop", "Shop");
                    option.Conventions.AuthorizeAreaFolder("Administration", "/Discounts", "Discount");
                    option.Conventions.AuthorizeAreaFolder("Administration", "/Accounts", "Account");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
