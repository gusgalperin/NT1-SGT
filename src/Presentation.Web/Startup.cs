using Domain.Core.CqsModule.Registration;
using Domain.Core.Data;
using Domain.Core.Data.Repositories;
using Domain.Core.Helpers;
using Domain.Core.Options;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Presentation.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions()
                .Configure<TurnoOptions>(Configuration.GetSection(nameof(TurnoOptions)));
            services.AddControllersWithViews();

            services.AddLogging((ILoggingBuilder builder) =>
            {
                builder.AddConsole();
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(x => x.LoginPath = "/Login/Index");
            services.AddAuthorization();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

            services.AddMvc();

            services
                .AddScoped<IDateTimeProvider, DateTimeProvider>()

                .AddScoped<IPacienteRepository, PacienteRepository>()
                .AddScoped<IProfesionalRepository, ProfesionalRepository>()
                .AddScoped<ITurnoRepository, TurnoRepository>()

                .AddScoped<IUnitOfWork, UnitOfWork>()

                .AddCqsModule();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}