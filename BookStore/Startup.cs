using BookStore.Data;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.Repository;
using BookStore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddRazorPages().AddRazorRuntimeCompilation().AddViewOptions(option =>
            {
                option.HtmlHelperOptions.ClientValidationEnabled = true;
            });

            services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(
               _configuration.GetConnectionString("DefaultConnection")));


            //Use Identity in BookStore Application 
            //We Have Install microsoft.aspnetcore.identity.entityframeworkcore from nuget packages
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<BookStoreContext>();

            //Add Some Column In IdentityUser Tbl
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<BookStoreContext>();

            //Create A Token for Email Confirmation 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BookStoreContext>().AddDefaultTokenProviders();

            //Change Password formate for IdentityBy Default Given
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                //If SignIn.RequireConfirmedEmail = true; then in signIn Action is result.IsNotAllowed Email must approved or active 
                options.SignIn.RequireConfirmedEmail = true;

                //lockout the user if password put incorrect like banking application
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });
            //for Login Redirect if not logged in then redirection 
            services.ConfigureApplicationCookie(config =>
            {
                //Path Comes From appsetting.json file
                config.LoginPath = _configuration["Application:SinginPath"];
            });

            services.AddControllersWithViews();

            //Add All IRepository & Repository as Services
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            //Get User For User Id From Below Services 
            services.AddScoped<IUserServices, UserServices>();

            services.AddSingleton<IMessageRepository, MessageRepository>();

            //Add Claims in Helpers ... 
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrinicialFactory>();

            #region Get Value from appsetting.json File of Key (NewBookAlert) Using IOptions 
            services.Configure<NewBookAlertConfig>("InternalBook", _configuration.GetSection("NewBookAlert"));
            services.Configure<NewBookAlertConfig>("ThirdPartyBook", _configuration.GetSection("ThirdPartyBook"));
            #endregion

            //For Mail Service
            services.Configure<SMTPConfigModel>(_configuration.GetSection("SMTPConfig"));
            services.AddScoped<IEmailServices, EmailServices>();

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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //For Identity Authentication 
            app.UseAuthentication();
            app.UseDeveloperExceptionPage();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                #region Conventional Routing Ex.

                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                //for Admin Area 
                //endpoints.MapControllerRoute(
                //  name: "MyArea",
                //  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                #region Multi Optional Parameter
                //endpoints.MapControllerRoute(
                //    name: "Default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}/{name?}");
                #endregion

                #region You Can Also Change Route Method
                //endpoints.MapControllerRoute(
                //   name: "Default",
                //   pattern: "{action}/{controller}/{id?}");
                #endregion

                #region Conventional Routing Ex.
                //endpoints.MapControllerRoute(
                //    name: "AboutUs",
                //    pattern: "about-us{id?}",
                //    defaults:new {controller="Home" , action= "AboutUs" });
                #endregion

                #endregion End Conventional Routing Ex.

                #region Attribute Routing Ex.
                //endpoints.MapControllers();
                #endregion
            });
        }
    }
}
