using AutoFixture.Xunit2;
using Domain.Core.Commands.Validations;
using Domain.Core.Exceptions;
using Domain.Core.Helpers;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Tests.Utils;
using Xunit;

namespace Domain.Core.Tests.Commands
{
    public class ValidarFechaTurnoEsMayorAHoyCommandTests
    {
        [Theory]
        [DefaultData]
        public void HandleAsync_Valid_Shouldwork(
            [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock,
            ValidarFechaTurnoEsMayorAHoyCommandHandler sut
            )
        {
            //arrange

            var now = DateTime.Now;
            var fechaTurno = now.AddDays(1);

            dateTimeProviderMock
                .Setup(x => x.Ahora())
                .Returns(now);

            var command = new ValidarFechaTurnoEsMayorAHoyCommand(Guid.NewGuid(), fechaTurno, fechaTurno.TimeOfDay);

            //act

            Func<Task> func = async () => await sut.HandleAsync(command);

            //assert

            func.Should().NotThrowAsync();
        }

        [Theory]
        [DefaultData]
        public void HandleAsync_FechaMenor_ShouldThrowEx(
            [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock,
            ValidarFechaTurnoEsMayorAHoyCommandHandler sut
            )
        {
            //arrange

            var now = DateTime.Now;
            var fechaTurno = now.AddDays(-1);

            dateTimeProviderMock
                .Setup(x => x.Ahora())
                .Returns(now);

            var command = new ValidarFechaTurnoEsMayorAHoyCommand(Guid.NewGuid(), fechaTurno, fechaTurno.TimeOfDay);

            //act

            Func<Task> func = async () => await sut.HandleAsync(command);

            //assert

            func.Should().ThrowAsync<UserException>().WithMessage("La fecha debe ser mayor a 'ahora'");
        }

        [Theory]
        [DefaultData]
        public void HandleAsync_FechaIgual_ShouldThrowEx(
            [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock,
            ValidarFechaTurnoEsMayorAHoyCommandHandler sut
            )
        {
            //arrange

            var now = DateTime.Now;

            dateTimeProviderMock
                .Setup(x => x.Ahora())
                .Returns(now);

            var command = new ValidarFechaTurnoEsMayorAHoyCommand(Guid.NewGuid(), now, now.TimeOfDay);

            //act

            Func<Task> func = async () => await sut.HandleAsync(command);

            //assert

            func.Should().ThrowAsync<UserException>().WithMessage("La fecha debe ser mayor a 'ahora'");
        }
    }
}