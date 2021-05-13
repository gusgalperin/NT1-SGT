using System.Threading.Tasks;

namespace Domain.Core.CqsModule.Command
{
    public interface IValidatable<TCommand>
        where TCommand : ICommand
    {
        Task ValidateAsync(TCommand command);
    }
}