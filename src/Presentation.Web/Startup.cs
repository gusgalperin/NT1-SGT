using Domain.Core.CqsModule.Registration;
using Domain.Core.Data;
using Domain.Core.Data.Repositories;
using Domain.Core.Helpers;
using Domain.Core.Options;
using Domain.Core.Security;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Presentation.Web.MIddlewares;
using Presentation.Web.Views.Login;

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
            services
                .AddOptions()
                .Configure<TurnoOptions>(Configuration.GetSection(nameof(TurnoOptions)));

            services.AddLogging((ILoggingBuilder builder) =>
            {
                builder.AddConsole();
            });

            services
                .AddAuthentication(AccountController.CookieScheme)
                .AddCookie(AccountController.CookieScheme, options =>
                {
                    options.AccessDeniedPath = "/account/denied";
                    options.LoginPath = "/account/login";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.None;
                options.Secure = CookieSecurePolicy.Always;
            });

            services.AddAuthorization();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

            services.AddMvc();
            services.AddControllersWithViews();

            services
                .AddScoped<IDateTimeProvider, DateTimeProvider>()

                .AddScoped<IPacienteRepository, PacienteRepository>()
                .AddScoped<IProfesionalRepository, ProfesionalRepository>()
                .AddScoped<ITurnoRepository, TurnoRepository>()
                .AddScoped<IUserRepository, UserRepository>()

                .AddScoped<ILoginService, LoginService>()

                .AddScoped<IUnitOfWork, UnitOfWork>()

                .AddScoped<IAuthenticatedUser, AuthenticatedUser>()

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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<SetAuthenticatedUserMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}