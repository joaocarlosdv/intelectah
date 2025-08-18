using Application.Dtos;
using Application.Service.Interface;
using Domain.Models;
using Intelectah_Api.Controllers.Crud;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Intelectah_Api.Controllers
{
    public class UsuarioController : ControllerCrud<Usuario, UsuarioDto>
    {
        private readonly IUsuarioService _usuarioeService;
        public UsuarioController(IUsuarioService service) : base(service)
        {
            _usuarioeService = service;
        }

        [HttpGet("ConsultaPaginada")]
        public async Task<ActionResult> ConsultaPaginada(int limit = 10, int offset = 0, string? search = null, int colOrder = 0, string dirOrder = "asc")
        {
            try
            {
                return Ok(await _usuarioeService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
