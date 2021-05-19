using Domain.Core.Commands;
using Domain.Core.Commands.Internals;
using Domain.Core.Commands.Validations;
using Domain.Core.CqsModule.Command;
using Domain.Core.CqsModule.Query;
using Domain.Core.Queryes;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Core.CqsModule.Registration
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCqsModule(this IServiceCollection services)
        {
            return services
                .AddScoped<ICommandProcessor, CommandProcessor>()
                .AddScoped<IQueryProcessor, QueryProcessor>()

                //Commands
                .AddCommand<AgregarTurnoCommand, AgregarTurnoCommandHandler>()

                .AddCommand<EjecutarAccionSobreTurnoResolverCommand, EjecutarAccionSobreTurnoResolverCommandHandler>()

                .AddCommand<PacienteCheckInCommand, PacienteCheckInCommandHandler>()
                .AddCommand<LlamarPacienteCommand, LlamarPacienteCommandHandler>()
                .AddCommand<FinDeTurnoCommand, FinDeTurnoCommandHandler>()
                .AddCommand<CancelarTurnoCommand, CancelarTurnoCommandHandler>()

                .AddCommand<CrearPacienteCommand, CrearPacienteCommandHandler, Paciente>()
   
                //Internals
                .AddCommand<CambiarEstadoTurnoCommand, CambiarEstadoTurnoCommandHandler>()

                //Validations
                .AddCommand<ValidarFechaTurnoEsMayorAHoyCommand, ValidarFechaTurnoEsMayorAHoyCommandHandler>()
                .AddCommand<ValidarProfesionalAtiendeEnFechaHoraCommand, ValidarProfesionalAtiendeEnFechaHoraCommandHandler>()
                .AddCommand<ValidarTurnoEstaLibreCommand, ValidarTurnoEstaLibreCommandHandler>()

                //Queries
                .AddQuery<ObtenerAgendaDelDiaQuery, ObtenerAgendaDelDiaResult, ObtenerAgendaDelDiaQueryHandler>()
                .AddQuery<ObtenerProfesionalColaQuery, ObtenerProfesionalColaQueryResult, ObtenerProfesionalColaQueryHandler>();
        }

        private static IServiceCollection AddCommand<TCommand, TCommandHandler>(this IServiceCollection services)
            where TCommand : ICommand
            where TCommandHandler : class, ICommandHandler<TCommand>
        {
            return services.AddScoped<ICommandHandler<TCommand>, TCommandHandler>();
        }

        private static IServiceCollection AddCommand<TCommand, TCommandHandler, TReturn>(this IServiceCollection services)
            where TCommand : ICommand
            where TCommandHandler : class, ICommandHandler<TCommand, TReturn>
        {
            return services.AddScoped<ICommandHandler<TCommand, TReturn>, TCommandHandler>();
        }

        private static IServiceCollection AddQuery<TQuery, TQueryResult, TQueryHandler>(this IServiceCollection services)
            where TQuery : IQuery<TQueryResult>
            where TQueryHandler : class, IQueryHandler<TQuery,TQueryResult>
        {
            return services.AddScoped<IQueryHandler<TQuery, TQueryResult>, TQueryHandler>();
        }
    }
}