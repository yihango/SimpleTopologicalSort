using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TestConnection.Database;
using EasyUnitOfWork;
using TestConnection.Repositories;
using TestConnection.Filters;

namespace TestConnection
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
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<UowFilter>();
            });

            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient(typeof(UowFilter));
            services.AddEasyUnitOfWorkEfCore();
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
            }

            // 注册数据库连接
            app.ApplicationServices
                .GetService<IDbContextRegister>()
                .Register<AppDbContext>((options) =>
            {
                if (options.ExistingConnection != null)
                {
                    options.DbContextOptions.UseSqlServer(options.ExistingConnection);
                }
                else
                {
                    var connectionString = this.Configuration["ConnectionStrings:Default"];

                    options.DbContextOptions.UseSqlServer(connectionString);
                }
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
