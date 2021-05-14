using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using Tests.Utils;
using System.Linq;
using Tests.Utils.Entities;

namespace Domain.Entities.Tests
{
    public class ProfesionalTests
    {
        [Fact]
        public void Ctor_EspecialidadesNull_ShouldThrowEx()
        {
            //arrange

            //act

            Action act = () => new Profesional("nombre", "email", "password", null, DiaHorario.DefaultTodaLaSemana());

            //assert

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_EspecialidadesEmpty_ShouldThrowEx()
        {
            //arrange

            //act

            Action act = () => new Profesional("nombre", "email", "password", new List<Especialidad>(), DiaHorario.DefaultTodaLaSemana());

            //assert

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_DiasNull_ShouldThrowEx()
        {
            //arrange

            //act

            Action act = () => new Profesional("nombre", "email", "password", new List<Especialidad> { new Especialidad("especialiad") }, null);

            //assert

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_DiasEmpty_ShouldThrowEx()
        {
            //arrange

            //act

            Action act = () => new Profesional("nombre", "email", "password", new List<Especialidad> { new Especialidad("especialiad") }, new List<DiaHorario>());

            //assert

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [DefaultData]
        public void Ctor_Valid_ShouldWork(
            IEnumerable<Especialidad> especialidades,
            IEnumerable<DiaHorario> dias)
        {
            //arrange

            //act

            var result = new Profesional("nombre", "email", "password", especialidades.ToList(), dias.ToList());

            //assert

            result.Nombre.Should().Be("nombre");
            result.Email.Should().Be("email");
            result.Password.Should().Be("password");
            result.DiasQueAtiende.Should().BeEquivalentTo(dias);
        }

        [Theory]
        [DefaultData]
        public void Atiende_ErrorDia_ShouldReturnFalse(
            IEnumerable<Especialidad> especialidades)
        {
            //arrange

            var p = new Profesional("nombre", "email", "password", especialidades.ToList(), DiaHorario.DefaultTodaLaSemana());

            //act

            var result = p.Atiende(
                new DateTimeOffset(2021, 4, 24, 1, 1, 1, 1, new TimeSpan()), //sabado
                new TimeSpan(9, 0, 0));

            //assert

            result.Should().BeFalse();
        }

        [Theory]
        [DefaultData]
        public void Atiende_ErrorFechaInicio_ShouldReturnFalse(
            IEnumerable<Especialidad> especialidades)
        {
            //arrange

            var p = new Profesional("nombre", "email", "password", especialidades.ToList(), DiaHorario.DefaultTodaLaSemana());

            //act

            var result = p.Atiende(
                new DateTimeOffset(2021, 4, 23, 1, 1, 1, 1, new TimeSpan()), //viernes
                new TimeSpan(19, 0, 0));

            //assert

            result.Should().BeFalse();
        }

        [Theory]
        [DefaultData]
        public void Atiende_Valid_ShouldReturnTrue(
            IEnumerable<Especialidad> especialidades)
        {
            //arrange

            var p = new Profesional("nombre", "email", "password", especialidades.ToList(), DiaHorario.DefaultTodaLaSemana());

            //act

            var result = p.Atiende(
                new DateTimeOffset(2021, 4, 23, 1, 1, 1, 1, new TimeSpan()), //viernes
                new TimeSpan(14, 0, 0));

            //assert

            result.Should().BeTrue();
        }

        [Theory]
        [DefaultData]
        public Profesional EncolarPaciente_ColaVacia(Turno turno)
        {
            //arrange

            var p = ProfesionalExtensions.ProfesionalDefault();

            //act

            p.EncolarPaciente(turno, new DateTime(), new TimeSpan());

            //assert

            p.Cola.Should().NotBeEmpty();
            p.Cola.Count().Should().Be(1);
            p.Cola.First().Turno.Should().Be(turno);
            p.Cola.First().Orden.Should().Be(1);

            return p;
        }

        [Fact]
        public void EncolarPaciente_PacienteLLegaTarde()
        {
            //arrange

            var primerTurno = new Turno(Guid.NewGuid(), Guid.NewGuid(), DateTime.Today, new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0));
            var turnoPaciente = new Turno(Guid.NewGuid(), Guid.NewGuid(), DateTime.Today, new TimeSpan(9, 0, 0), new TimeSpan(9, 30, 0));
            var horaActual = new TimeSpan(9, 47, 0);
            var fecha = DateTime.Today.Add(horaActual);

            var p = EncolarPaciente_ColaVacia(primerTurno);

            //act

            p.EncolarPaciente(turnoPaciente, fecha, TimeSpan.FromMinutes(15));

            //assert

            p.Cola.Should().NotBeEmpty();
            p.Cola.Count().Should().Be(2);
            p.Cola.Last().Turno.Should().Be(turnoPaciente);
            p.Cola.Last().Orden.Should().Be(2);
        }
    }
}
