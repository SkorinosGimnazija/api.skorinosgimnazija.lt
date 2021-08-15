using Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
using Application.Menus.Dtos;
using Application.Menus;
using Application.Posts;
using Application.Posts2;
using Domain.CMS;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
using Application.Domains;
using Application.Domains.Dtos;

[Route("admin/domains")]
    [Authorize(Roles = Roles.Admin)]
    public class DomainController : BaseApiController
    {
        [HttpGet("list")]
        public async Task<ActionResult<List<Domain>>> GetDomains(CancellationToken ct)
        {
            return await Mediator.Send(new DomainsList.Query(), ct);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Domain>> GetDomain(int id, CancellationToken ct)
        {
            return await Mediator.Send(new DomainDetails.Query(id), ct);
        }

        [HttpPost]
        public async Task<ActionResult<Domain>> CreateDomain(DomainCreateDto domain, CancellationToken ct)
        {
            return await Mediator.Send(new DomainCreate.Command(domain), ct);
        }

        [HttpPut]
        public async Task<IActionResult> EditDomain(Domain domain, CancellationToken ct)
        {
            return await Mediator.Send(new DomainEdit.Command(domain), ct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDomain(int id, CancellationToken ct)
        {
            return await Mediator.Send(new DomainDelete.Command(id), ct);
        }
    }
}
