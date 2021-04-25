using System.Threading.Tasks;

namespace Domain.Core.CqsModule.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}