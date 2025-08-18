using ApiExterna.Models;
using Application.Dtos;
using Intelectah_App.Models;
using Intelectah_App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Intelectah_App.Controllers
{
    public class ConcessionariaController : Controller
    {
        private readonly IConcessionariaServiceApp _concessionariaService;
        public ConcessionariaController(IConcessionariaServiceApp concessionariaService)
        {
            _concessionariaService = concessionariaService;
        }

        public IActionResult Index()
        {
            var model = new ConcessionariaViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Concessionaria = null,
                Lista = null,
                Acao = "L"
            };

            return View(model);
        }

        public async Task<ConcessionariaViewModel> Consultar(int limit, int offset, string search, int colOrder, string dirOrder)
        {
            var lista = (await _concessionariaService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));

            var model = new ConcessionariaViewModel
            {
                recordsTotal = lista.Object!.Count,
                recordsFiltered = lista.Object!.Count,
                Concessionaria = null,
                Lista = lista.Object,
                Acao = "L"
            };

            return model;
        }

        public async Task<Cep> ConsultarCep(string cep)
        {
            return await _concessionariaService.ConsultarCep(cep);            
        }

        public IActionResult Incluir()
        {
            var model = new ConcessionariaViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Concessionaria = new ConcessionariaDto(),
                Lista = null,
                Acao = "I"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Alterar(int id)
        {
            var model = new ConcessionariaViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Concessionaria = (await _concessionariaService.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "A"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Visualizar(int id)
        {
            var model = new ConcessionariaViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Concessionaria = (await _concessionariaService.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "V"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Salvar(ConcessionariaDto obj)
        {
            obj.Cep = obj.Cep.Replace("-","");
            var retorno = await _concessionariaService.Salvar(obj);
            if (retorno.Success)
            {
                TempData["sucesso"] = retorno.Message;

                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = retorno.Message;

                var model = new ConcessionariaViewModel
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    Concessionaria = obj,
                    Lista = null,
                    Acao = obj.Id == null ? "A" : "I"
                };

                return View("Cadastro", model);
            }
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var retorno = await _concessionariaService.Deletar(id);
            if (retorno.Success)
            {
                TempData["sucesso"] = retorno.Message;
            }
            else
            {
                TempData["erro"] = retorno.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
