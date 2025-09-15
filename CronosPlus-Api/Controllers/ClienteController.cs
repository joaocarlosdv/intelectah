using Application.Dtos;
using Application.Service.Interface;
using Domain.Models;
using CronosPlus_Api.Controllers.Crud;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CronosPlus_Api.Controllers
{
    public class ClienteController : ControllerCrud<Cliente, ClienteDto>
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService service) : base(service)
        {
            _clienteService = service;
        }

        [HttpGet("ConsultaPaginada")]
        public async Task<ActionResult> ConsultaPaginada(int limit = 10, int offset = 0, string? search = null, int colOrder = 0, string dirOrder = "asc")
        {
            try
            {
                return Ok(await _clienteService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
