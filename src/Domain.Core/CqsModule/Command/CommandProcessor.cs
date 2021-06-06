using Domain.Core.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Domain.Core.CqsModule.Command
{
    public interface ICommandProcessor
    {
        Task ProcessCommandAsync<TCommand>(TCommand command) 
            where TCommand : ICommand;

        Task<TReturn> ProcessCommandAsync<TCommand, TReturn>(TCommand command)
             where TCommand : ICommand;
    }

    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticatedUser _authenticatedUser;

        public CommandProcessor(
            IServiceProvider serviceProvider,
            IAuthenticatedUser authenticatedUser)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _authenticatedUser = authenticatedUser ?? throw new ArgumentNullException(nameof(authenticatedUser));
        }

        public async Task ProcessCommandAsync<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            var handlers = _serviceProvider.GetServices<ICommandHandler<TCommand>>();

            foreach (var handler in handlers)
            {
                if (handler is ISecuredCommand h1)
                    _authenticatedUser.ValidarPermiso(h1.PermisoRequerido);

                if (handler is IValidatable<TCommand> h2)
                    await h2.ValidateAsync(command);

                await handler.HandleAsync(command);
            }
        }

        public async Task<TReturn> ProcessCommandAsync<TCommand, TReturn>(TCommand command)
            where TCommand : ICommand
        {
            var handlers = _serviceProvider.GetServices<ICommandHandler<TCommand, TReturn>>();

            foreach (var handler in handlers)
            {
                if (handler is ISecuredCommand h1)
                    _authenticatedUser.ValidarPermiso(h1.PermisoRequerido);

                if (handler is IValidatable<TCommand> h2)
                    await h2.ValidateAsync(command);

                return await handler.HandleAsync(command);
            }

            throw new InvalidOperationException();
        }
    }
}