using BookStore.Data;
using BookStore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddRazorPages().AddRazorRuntimeCompilation().AddViewOptions(option =>
            //{
            //    option.HtmlHelperOptions.ClientValidationEnabled = true;
            //});

            services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(
               Configuration.GetConnectionString("DefaultConnection")
                ));

            services.AddControllersWithViews();
            #region only work in debug mode not in realse mode
#if DEBUG
            services.AddRazorPages().AddRazorRuntimeCompilation();
#endif
            #endregion

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
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
            app.UseDeveloperExceptionPage();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                #region Conventional Routing Ex.

                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

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
