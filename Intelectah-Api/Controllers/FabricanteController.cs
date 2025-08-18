using Application.Dtos;
using Application.Service.Interface;
using Domain.Models;
using Intelectah_Api.Controllers.Crud;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Intelectah_Api.Controllers
{
    public class FabricanteController : ControllerCrud<Fabricante, FabricanteDto>
    {
        private readonly IFabricanteService _fabricanteService;
        public FabricanteController(IFabricanteService service) : base(service)
        {
            _fabricanteService = service;
        }

        [HttpGet("ConsultaPaginada")]
        public async Task<ActionResult> ConsultaPaginada(int limit = 10, int offset = 0, string? search=null, int colOrder = 0, string dirOrder = "asc")
        {
            try
            {
                return Ok(await _fabricanteService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("ConsultaPaginadaCount")]
        public async Task<ActionResult> ConsultaPaginadaCount(string? search = null, int colOrder = 0, string dirOrder = "asc")
        {
            try
            {
                return Ok(await _fabricanteService.ConsultaPaginadaCount( search, colOrder, dirOrder));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
