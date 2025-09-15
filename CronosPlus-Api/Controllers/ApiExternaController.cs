using ApiExterna.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CronosPlus_Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ApiExternaController : Controller
    {
        private readonly ICorreiosService _correiosService;
        public ApiExternaController(ICorreiosService correiosService)
        {
            _correiosService = correiosService;
        }

        [HttpGet("ConsultaCep")]
        public async Task<ActionResult> ConsultaCep(string cep)
        {
            try
            {
                return Ok(await _correiosService.ConsultarEndereco(cep));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
