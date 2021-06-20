using Domain.Core.Commands;
using Domain.Core.CqsModule.Command;
using Domain.Core.CqsModule.Query;
using Domain.Core.Data.Repositories;
using Domain.Core.Queryes;
using Domain.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Web.Controllers
{
    public class ProfesionalesController : Controller
    {
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandProcessor _commandProcessor;
        private readonly IAuthenticatedUser _user;

        public ProfesionalesController(
            IProfesionalRepository profesionalRepository,
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            IAuthenticatedUser user)
        {
            _profesionalRepository = profesionalRepository ?? throw new ArgumentNullException(nameof(profesionalRepository));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var p = await _profesionalRepository.GetAllAsync();

            if (_user.UserInfo.Rol == Domain.Entities.Rol.Options.Profesional)
            {
                p = p
                    .Where(x => x.Id == _user.UserInfo.Id)
                    .ToList();
            }

            return View(p);
        }

        [Authorize]
        public async Task<IActionResult> Cola(Guid id)
        {
            var cola = await _queryProcessor.ProcessQueryAsync<ObtenerProfesionalColaQuery, ObtenerProfesionalColaQueryResult>(
                new ObtenerProfesionalColaQuery(id));

            return View(cola);
        }
    }
}