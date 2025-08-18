using Application.Dtos;
using Application.Service.Interface;
using Domain.Models;
using Intelectah_Api.Controllers.Crud;
using Microsoft.AspNetCore.Mvc;
using NHTSAVehicleApi.Service;
using System.Net;

namespace Intelectah_Api.Controllers
{
    public class VeiculoController : ControllerCrud<Veiculo, VeiculoDto>
    {
        private readonly INHTSARecallService _nHTSARecallService;
        private readonly IVeiculoService _veiculoService;
        public VeiculoController(IVeiculoService service, INHTSARecallService nHTSARecallService) : base(service)
        {
            _nHTSARecallService = nHTSARecallService;
            _veiculoService = service;
        }

        [HttpGet("ConsultarRecall")]
        public async Task<IActionResult> ConsultarRecall(string fabricante, string modelo, int ano)
        {
            try
            {
                return Ok(await _nHTSARecallService.ConsultarRecalls(fabricante, modelo, ano));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("ConsultaPaginada")]
        public async Task<ActionResult> ConsultaPaginada(int limit = 10, int offset = 0, string? search = null, int colOrder = 0, string dirOrder = "asc")
        {
            try
            {
                return Ok(await _veiculoService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet("ConsultaPorFabricante")]
        public async Task<ActionResult> ConsultaPorFabricante(int fabricanteId)
        {
            try
            {
                return Ok(await _veiculoService.Consultar(x => x.FabricanteId == fabricanteId, null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }        
    }
}
