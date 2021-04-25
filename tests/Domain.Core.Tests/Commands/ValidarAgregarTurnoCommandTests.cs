using AutoFixture.Xunit2;
using Domain.Core.Commands;
using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
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
            ValidarAgregarTurnoCommand command,
            ValidarAgregarTurnoCommandHandler sut
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

            //act

            Func<Task> func = async () => await sut.HandleAsync(command);

            //assert

            func.Should().ThrowAsync<ProfesionalNoAtiendeException>();
        }
    }
}