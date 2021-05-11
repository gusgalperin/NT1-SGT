using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Queryes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Web.Controllers
{
    public class ProfesionalesController : Controller
    {
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IQueryProcessor _queryProcessor;

        public ProfesionalesController(
            IProfesionalRepository profesionalRepository,
            IQueryProcessor queryProcessor)
        {
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        public async Task<IActionResult> Index()
        {
            var p = await _profesionalRepository.GetAllAsync();

            return View(p);
        }

        public async Task<IActionResult> Cola(Guid id)
        {
            var cola = await _queryProcessor.ProcessQueryAsync<ObtenerProfesionalColaQuery, IEnumerable<ObtenerProfesionalColaQueryResult>>(
                new ObtenerProfesionalColaQuery(id));

            return View(cola);
        }
    }
}