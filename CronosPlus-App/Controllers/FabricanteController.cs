using Application.Dtos;
using CronosPlus_App.Models;
using CronosPlus_App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CronosPlus_App.Controllers
{
    public class FabricanteController : Controller
    {
        private readonly IFabricanteServiceApp _fabricanteServiceApp;
        public FabricanteController(IFabricanteServiceApp fabricanteServiceApp) 
        {
            _fabricanteServiceApp = fabricanteServiceApp;
        }
        public IActionResult Index()
        {
            var model = new FabricanteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Fabricante = null,
                Lista = null,
                Acao = "L"
            };

            return View(model);
        }
        public async Task<FabricanteViewModel> Consultar(int limit, int offset, string search, int colOrder, string dirOrder)
        {
            var lista = (await _fabricanteServiceApp.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));

            var model = new FabricanteViewModel
            {
                recordsTotal = lista.Object!.Count,
                recordsFiltered = lista.Object!.Count, 
                Fabricante = null,
                Lista = lista.Object,
                Acao = "L"
            };

            return model;
        }

        public async Task<List<FabricanteDto>>? ConsultarTodos()
        {
            var lista = (await _fabricanteServiceApp.Consultar());
            if (lista.Success && lista.Object != null && lista.Object!.Count > 0)
            {
                return lista.Object;
            }

            return new List<FabricanteDto>();
        }
        public async Task<FabricanteDto>? ConsultarPorId(int id)
        {
            var lista = (await _fabricanteServiceApp.ConsultarPorId(id));
            if (lista.Success && lista.Object != null && lista.Object!.Count > 0)
            { 
                return lista.Object.FirstOrDefault()!;
            }

            return new FabricanteDto();
        }
        public IActionResult Incluir()
        {
            var model = new FabricanteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Fabricante = new FabricanteDto(),
                Lista = null,
                Acao = "I"
            };
            
            return View("Cadastro", model);
        }

        public async Task<IActionResult> Alterar(int id)
        {
            var model = new FabricanteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Fabricante = (await _fabricanteServiceApp.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "A"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Visualizar(int id)
        {
            var model = new FabricanteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Fabricante = (await _fabricanteServiceApp.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "V"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Salvar(FabricanteDto obj)
        {
            var retorno = await _fabricanteServiceApp.Salvar(obj);
            if (retorno.Success)
            {
                TempData["sucesso"] = retorno.Message;

                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = retorno.Message;

                var model = new FabricanteViewModel
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    Fabricante = obj,
                    Lista = null,
                    Acao = obj.Id == null ? "A" : "I"
                };

                return View("Cadastro", model);
            }
        }
        public async Task<IActionResult> Excluir(int id)
        {
            var retorno = await _fabricanteServiceApp.Deletar(id);
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
