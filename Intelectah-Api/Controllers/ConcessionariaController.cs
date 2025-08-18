using Application.Dtos;
using Application.Service.Interface;
using Domain.Models;
using Intelectah_Api.Controllers.Crud;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Intelectah_Api.Controllers
{
    public class ConcessionariaController : ControllerCrud<Concessionaria, ConcessionariaDto>
    {
        private readonly IConcessionariaService _concessionariaService;
        public ConcessionariaController(IConcessionariaService service) : base(service)
        {
            _concessionariaService = service;
        }

        [HttpGet("ConsultaPaginada")]
        public async Task<ActionResult> ConsultaPaginada(int limit = 10, int offset = 0, string? search = null, int colOrder = 0, string dirOrder = "asc")
        {
            try
            {
                return Ok(await _concessionariaService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
