using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using EfUnitOfWorkDemo.Database;
using EfUnitOfWorkDemo.Uow;
using EfUnitOfWorkDemo.Filters;
using EfUnitOfWorkDemo.Repositories;
using EfUnitOfWorkDemo.Domain.Entities;

namespace EfUnitOfWorkDemo
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
            services.AddControllers(o =>
            {
                o.Filters.Add(typeof(UowFilter));
            });

            services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]);
            });


            services.AddTransient<IUnitOfWorkManager, UnitOfWorkManager>();
            services.AddTransient<IDbContextTypeMatcher, DbContextTypeMatcher>();
            services.AddTransient<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>();
            services.AddSingleton(typeof(IDbContextFactory), new DbContextFactory());
            services.AddTransient<IDbContextResolver, DbContextResolver>();
            services.AddTransient<IEfCoreTransactionStrategy, EfCoreTransactionStrategy>();
            services.AddTransient<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddTransient<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>();

            var defaultConnectionStringResolver = new DefaultConnectionStringResolver();
            services.AddSingleton(typeof(IConnectionStringRegister), defaultConnectionStringResolver);
            services.AddSingleton(typeof(IConnectionStringResolver), defaultConnectionStringResolver);

            services.AddSingleton<IDbContextTypeMatcher, DbContextTypeMatcher>();
            services.AddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
            services.AddTransient<IActiveTransactionProvider, EfCoreActiveTransactionProvider>();

            //services.AddTransient(typeof(IRepository<,>, typeof(EfCoreRepositoryBase<AppDbContext, IEntity,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            });
        }
    }
}
