using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Domain.Core.CqsModule.Command
{
    public interface ICommandProcessor
    {
        Task ProcessCommandAsync<TCommand>(TCommand command) 
            where TCommand : ICommand;
    }

    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task ProcessCommandAsync<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            var handlers = _serviceProvider.GetServices<ICommandHandler<TCommand>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(command);
            }
        }
    }
}