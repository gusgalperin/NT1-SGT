using AutoFixture.Xunit2;
using Domain.Core.Commands;
using Domain.Core.CqsModule.Command;
using Domain.Core.Data;
using Domain.Core.Data.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Tests.Utils;
using Xunit;

namespace Domain.Core.Tests.Commands
{
    public class AgregarTurnoCommandTests
    {
        [Theory]
        [DefaultData]
        public async Task Handle_Valid_ShouldWork(
            [Frozen] Mock<ICommandProcessor> commandProcessorMock,
            [Frozen] Mock<ITurnoRepository> turnoRepoMock,
            [Frozen] Mock<IUnitOfWork> uowMock,
            AgregarTurnoCommand command,
            AgregarTurnoCommandHandler sut
            )
        {
            //arrange

            commandProcessorMock
                .Setup(x => x.ProcessCommandAsync(It.IsAny<ValidarAgregarTurnoCommand>()));

            Turno created = null;

            turnoRepoMock
                .Setup(x => x.AddAsync(It.IsAny<Turno>()))
                .Callback<Turno>(x => created = x);

            uowMock
                .SetupGet(x => x.Turnos)
                .Returns(turnoRepoMock.Object);

            uowMock
                .Setup(x => x.SaveChangesAsync());

            //act

            await sut.HandleAsync(command);

            //assert

            created.Should().NotBeNull();
            created.Estado.Should().Be(TurnoEstado.Pendiente);
            created.ProfesionalId.Should().Be(command.IdProfesional);
            created.PacienteId.Should().Be(command.IdPaciente);
            created.Fecha.Should().Be(command.Fecha);
            created.HoraInicio.Should().Be(command.HoraInicio);
            created.HoraFin.Should().Be(command.HoraFin);

            turnoRepoMock.Verify(x => x.AddAsync(It.IsAny<Turno>()), Times.Once);
            uowMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}