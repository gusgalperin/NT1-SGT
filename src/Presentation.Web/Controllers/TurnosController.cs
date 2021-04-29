using Domain.Core.CqsModule.Command;
using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Exceptions;
using Domain.Core.Queryes;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Models.Shared;
using Presentation.Web.Models.Turnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Web.Controllers
{
    public class TurnosController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandProcessor _commandProcessor;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IPacienteRepository _pacienteRepository;

        public TurnosController(
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            IProfesionalRepository profesionalRepository,
            IPacienteRepository pacienteRepository)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
            _pacienteRepository = pacienteRepository ?? throw new ArgumentNullException(nameof(pacienteRepository));
        }

        public async Task<IActionResult> Index(DateTimeOffset? fecha = null)
        {
            var turnos = await _queryProcessor.ProcessQueryAsync<ObtenerAgendaDelDiaQuery, ObtenerAgendaDelDiaResult>(
                new ObtenerAgendaDelDiaQuery(fecha));

            return View(TurnosViewModel.FromQuery(turnos));
        }

        public async Task<IActionResult> Nuevo()
        {
            var viewModel = new CrearTurnoViewModel
            {
                Profesionales = await _profesionalRepository.GetAllAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(CrearTurnoViewModel model)
        {
            try
            {
                await _commandProcessor.ProcessCommandAsync(model.ToCommand());

                return RedirectToAction("Index", "Turnos");
            }
            catch (UserException ex)
            {
                model.ExceptionMessage = $"No se pudo crear el turno: {ex.Message}";
                model.Profesionales = await _profesionalRepository.GetAllAsync();

                return View(model);
            }
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
                listaHorarios.Add(new Select2Model { Id = horaTurno.ToString(), Text = horaTurno.ToString("hh\\:mm") });
                horaTurno = horaTurno.Add(profesional.DuracionTurno);
            }

            return listaHorarios;
        }
    }
}