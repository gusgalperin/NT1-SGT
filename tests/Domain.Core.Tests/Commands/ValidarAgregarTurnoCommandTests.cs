using AutoFixture.Xunit2;
using Domain.Core.Commands;
using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using Domain.Core.Helpers;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Utils;
using Xunit;

namespace Domain.Core.Tests.Commands
{
    public class ValidarAgregarTurnoCommandTests
    {
        [Theory]
        [DefaultData]
        public void Handle_ProfesionalNoAtiendeEnFecha_ShouldThrowEx(
            [Frozen] Mock<IProfesionalRepository> profesionalRepoMock,
            AgregarTurnoCommandHandler sut
            )
        {
            //arrange

            var hoy = DateTime.Now;

            var command = new AgregarTurnoCommand(Guid.NewGuid(), Guid.NewGuid(), hoy.AddDays(1), new TimeSpan(9, 0, 0));

            var profesional = new Profesional("sarasa", "sarasa", "sarasa",
                new List<Especialidad> { new Especialidad("sarasa") },
                new List<DiaHorario> { new DiaHorario(hoy.DayOfWeek, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)) });

            profesionalRepoMock
                .Setup(x => x.GetOneAsync(command.IdProfesional))
                .ReturnsAsync(profesional);

            //act

            Func<Task> func = async () => await sut.ValidateAsync(command);

            //assert

            func.Should().ThrowAsync<ProfesionalNoAtiendeException>();
        }

        [Theory]
        [DefaultData]
        public void Handle_HoraInicioSuperiorAHoraFin_ShouldThrowEx(
            AgregarTurnoCommandHandler sut
            )
        {
            //arrange

            var command = new AgregarTurnoCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.Today, new TimeSpan(9, 0, 0));

            //act

            Func<Task> func = async () => await sut.ValidateAsync(command);

            //assert

            func.Should().ThrowAsync<UserException>().WithMessage("La hora de fin debe ser posterior a la de incicio");
        }

        [Theory]
        [DefaultData]
        public void Handle_FechaMenorAAhora_ShouldThrowEx(
            [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock,
            AgregarTurnoCommandHandler sut
            )
        {
            //arrange

            var ahora = DateTime.Now;

            dateTimeProviderMock.Setup(x => x.Ahora()).Returns(ahora);

            var command = new AgregarTurnoCommand(Guid.NewGuid(), Guid.NewGuid(), ahora.AddMinutes(-1), new TimeSpan(9, 0, 0));

            //act

            Func<Task> func = async () => await sut.HandleAsync(command);

            //assert

            func.Should().ThrowAsync<UserException>().WithMessage("La fecha debe ser mayor a 'ahora'");
        }

        [Theory]
        [DefaultData]
        public void Handle_TurnoOcupado_ShouldThrowEx(
            [Frozen] Mock<IProfesionalRepository> profesionalRepoMock,
            [Frozen] Mock<ITurnoRepository> turnoRepoMock,
            AgregarTurnoCommand command,
            AgregarTurnoCommandHandler sut
            )
        {
            //arrange

            var hoy = DateTimeOffset.UtcNow;

            var profesional = new Profesional("sarasa", "sarasa", "sarasa",
                new List<Especialidad> { new Especialidad("sarasa") },
                new List<DiaHorario> { new DiaHorario(hoy.DayOfWeek, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0)) });

            profesionalRepoMock
                .Setup(x => x.GetOneAsync(command.IdProfesional))
                .ReturnsAsync(profesional);

            var turno = new Turno(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, new TimeSpan(), new TimeSpan());

            turnoRepoMock
                .Setup(x => x.BuscarTurnoAsync(command.IdProfesional, command.Fecha, command.HoraInicio))
                .ReturnsAsync(turno);

            //act

            Func<Task> func = async () => await sut.HandleAsync(command);

            //assert

            func.Should().ThrowAsync<TurnoOcupadoException>();
        }
    }
}