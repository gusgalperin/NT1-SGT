﻿using System.Threading.Tasks;

namespace Domain.Core.CqsModule.Command
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task ValidateAsync(TCommand command);
        Task HandleAsync(TCommand command);
    }
}