using Application.Dtos.Crud;
using Application.Service.Crud;
using Domain.Models.Crud;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Intelectah_Api.Controllers.Crud
{
    [Route("api/[controller]")]
    [Authorize]
    public abstract class ControllerCrud<T, Dto> : Controller where T : ModelCrud where Dto : DtoCrud
    {
        private readonly ICrudService<T, Dto> _service;
        public ControllerCrud(ICrudService<T, Dto> service)
        {
            _service = service;
        }

        [HttpGet("Consultar")]
        public async virtual Task<IActionResult> Consultar()
        {
            try
            {
                return Ok(await _service.Consultar());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("ConsultarPorId")]
        public async virtual Task<IActionResult> ConsultarPorId(int id)
        {
            try
            {
                return Ok((await _service.Consultar(x => x.Id == id, null)));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpPost("Salvar")]
        public async virtual Task<IActionResult> Salvar([FromBody] Dto dto)
        {
            try
            {
                return Ok(await _service.Salvar(dto));

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet("Deletar")]
        public async virtual Task<IActionResult> Deletar(int id)
        {
            try
            {
                return Ok(await _service.Deletar(id));

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
