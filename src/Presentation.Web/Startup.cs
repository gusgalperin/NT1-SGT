using Domain.Core.Commands;
using Domain.Core.CqsModule.Command;
using Domain.Core.CqsModule.Query;
using Domain.Core.Data;
using Domain.Core.Data.Repositories;
using Domain.Core.Herlpers;
using Domain.Core.Queryes;
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

namespace Presentation.Web
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

                .AddScoped<ICommandProcessor, CommandProcessor>()
                .AddScoped<IQueryProcessor, QueryProcessor>()

                .AddScoped<IPacienteRepository, PacienteRepository>()
                .AddScoped<IProfesionalRepository, ProfesionalRepository>()
                .AddScoped<ITurnoRepository, TurnoRepository>()

                .AddSingleton<IUnitOfWork, UnitOfWork>()

                .AddScoped<ICommandHandler<AgregarTurnoCommand>, AgregarTurnoCommandHandler>()
                .AddScoped<ICommandHandler<ValidarAgregarTurnoCommand>, ValidarAgregarTurnoCommandHandler>()
                .AddScoped<IQueryHandler<ObtenerAgendaDelDiaQuery, ObtenerAgendaDelDiaResult>, ObtenerAgendaDelDiaQueryHandler>();
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