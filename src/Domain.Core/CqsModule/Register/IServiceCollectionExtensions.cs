using Domain.Core.Commands;
using Domain.Core.CqsModule.Command;
using Domain.Core.CqsModule.Query;
using Domain.Core.Queryes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Domain.Core.CqsModule.Register
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCqsModule(this IServiceCollection services)
        {
            services
                .AddScoped<ICommandProcessor, CommandProcessor>()
                .AddScoped<IQueryProcessor, QueryProcessor>()

                .AddScoped<ICommandHandler<AgregarTurnoCommand>, AgregarTurnoCommandHandler>()
                .AddScoped<ICommandHandler<ValidarAgregarTurnoCommand>, ValidarAgregarTurnoCommandHandler>()
                .AddScoped<ICommandHandler<PacienteCheckInCommand>, PacienteCheckInCommandHandler>()

                .AddScoped<IQueryHandler<ObtenerAgendaDelDiaQuery, ObtenerAgendaDelDiaResult>, ObtenerAgendaDelDiaQueryHandler>()
                .AddScoped<IQueryHandler<ObtenerProfesionalColaQuery, IEnumerable<ObtenerProfesionalColaQueryResult>>, ObtenerProfesionalColaQueryHandler>();

            return services;
        }
    }
}