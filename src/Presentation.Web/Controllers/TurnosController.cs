using Domain.Core.Commands;
using Domain.Core.CqsModule.Command;
using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using Domain.Core.Helpers;
using Domain.Core.Queryes;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Models.Shared;
using Presentation.Web.Models.Turnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Web.Controllers
{
    [Authorize]
    public class TurnosController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandProcessor _commandProcessor;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ITurnoRepository _turnoRepository;

        public TurnosController(
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            IProfesionalRepository profesionalRepository,
            IPacienteRepository pacienteRepository,
            ITurnoRepository turnoRepository)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
            _pacienteRepository = pacienteRepository ?? throw new ArgumentNullException(nameof(pacienteRepository));
            _turnoRepository = turnoRepository ?? throw new ArgumentNullException(nameof(turnoRepository));
        }

        //public async Task<IActionResult> Index(DateTimeOffset? fecha = null)
        //{
        //    var turnos = await _queryProcessor.ProcessQueryAsync<ObtenerAgendaDelDiaQuery, ObtenerAgendaDelDiaResult>(
        //        new ObtenerAgendaDelDiaQuery(fecha));

        //    return View(TurnosViewModel.FromQuery(turnos));
        //}

        public async Task<IActionResult> Index(DateTime? fecha = null)
        {
            var agenda = await _queryProcessor.ProcessQueryAsync<ObtenerCalendarioDelDiaQuery, ObtenerCalendarioDelDiaQueryResult>(
                new ObtenerCalendarioDelDiaQuery(fecha ?? DateTime.Today));

            return View("Index2", agenda);
        }

        public async Task<IActionResult> Nuevo(CrearTurnoViewModel model)
        {
            var viewModel = model ?? new CrearTurnoViewModel();

            viewModel.Profesionales = await _profesionalRepository.GetAllAsync();
            viewModel.Fecha = new DateTime(viewModel.Fecha.Year, viewModel.Fecha.Month, viewModel.Fecha.Day);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearTurnoViewModel model)
        {
            try
            {
                await _commandProcessor.ProcessCommandAsync(model.ToCommand());

                return RedirectToAction("Index", "Turnos", new { fecha = model.Fecha });
            }
            catch (UserException ex)
            {
                model.ExceptionMessage = $"No se pudo crear el turno: {ex.Message}";
                model.Profesionales = await _profesionalRepository.GetAllAsync();

                return View("Nuevo", model);
            }
        }

        public async Task<IActionResult> CheckIn(Guid turnoId, DateTime fecha)
        {
            await _commandProcessor.ProcessCommandAsync(new PacienteCheckInCommand(turnoId));

            return RedirectToAction("Index", fecha);
        }

        public async Task<IActionResult> EjecutarAccion(Guid turnoId, TurnoAccion accion)
        {
            await _commandProcessor.ProcessCommandAsync(new EjecutarAccionSobreTurnoResolverCommand(turnoId, accion));

            return RedirectToAction("Detalle", new { id = turnoId });
        }

        public async Task<IActionResult> Detalle(Guid id)
        {
            var turno = await _queryProcessor.ProcessQueryAsync<ObtenerTurnoQuery, ObtenerTurnoQueryResult>(new ObtenerTurnoQuery(id));

            return View(turno);
        }

        [HttpGet]
        [Route("turnos/pacientes")]
        public async Task<IEnumerable<Select2Model>> GetPacientesAsync([FromQuery] string query)
        {
            var pacientes = await _pacienteRepository.FindFromQueryAsync(query);

            return pacientes
                .Select(x => new Select2Model { Id = x.Id.ToString(), Text = x.Nombre })
                .ToList();
        }

        [HttpGet]
        [Route("turnos/profesional/horarios")]
        public async Task<IEnumerable<Select2Model>> GetHorariosProfesionalAsync([FromQuery] Guid profesionalId, [FromQuery] DateTime fecha)
        {
            var profesional = await _profesionalRepository.GetOneAsync(profesionalId);

            var diaHorario = profesional.DiasQueAtiende
                .FirstOrDefault(x => x.Dia == fecha.DayOfWeek);

            var listaHorarios = new List<Select2Model>();

            if (diaHorario == null)
                return listaHorarios;

            var horaTurno = diaHorario.HoraDesde;

            while (horaTurno < diaHorario.HoraHasta)
            {
                listaHorarios.Add(new Select2Model { Id = horaTurno.ToString(), Text = horaTurno.ToLegibleString() });
                horaTurno = horaTurno.Add(profesional.DuracionTurno);
            }

            return listaHorarios;
        }

        public IActionResult CrearPaciente()
        {
            return PartialView("_CrearPaciente", new Models.Pacientes.Nuevo { FechaNacmiento = DateTime.Now});
        }

        [HttpPost]
        public async Task<IActionResult> CrearPaciente(Models.Pacientes.Nuevo model)
        {
            if (ModelState.IsValid)
            {
                var paciente = await _commandProcessor.ProcessCommandAsync<CrearPacienteCommand, Paciente>(
                    new CrearPacienteCommand(model.Dni, model.Nombre, model.FechaNacmiento));

                model.SetId(paciente.Id);
            }

            return PartialView("_CrearPaciente", model);
        }
    }
}