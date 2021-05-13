using Domain.Core.Commands;
using Domain.Core.CqsModule.Command;
using Domain.Core.CqsModule.Query;
using Domain.Core.Queryes;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Core.CqsModule.Registration
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCqsModule(this IServiceCollection services)
        {
            services
                .AddScoped<ICommandProcessor, CommandProcessor>()
                .AddScoped<IQueryProcessor, QueryProcessor>()

                //Commands

                .AddScoped<ICommandHandler<AgregarTurnoCommand>, AgregarTurnoCommandHandler>()
                .AddScoped<ICommandHandler<PacienteCheckInCommand>, PacienteCheckInCommandHandler>()
                .AddScoped<ICommandHandler<LlamarPacienteCommand>, LlamarPacienteCommandHandler>()

                //Queries

                .AddScoped<IQueryHandler<ObtenerAgendaDelDiaQuery, ObtenerAgendaDelDiaResult>, ObtenerAgendaDelDiaQueryHandler>()
                .AddScoped<IQueryHandler<ObtenerProfesionalColaQuery, ObtenerProfesionalColaQueryResult>, ObtenerProfesionalColaQueryHandler>();

            return services;
        }
    }
}