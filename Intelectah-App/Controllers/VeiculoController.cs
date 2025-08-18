using Application.Dtos;
using Intelectah_App.Models;
using Intelectah_App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Intelectah_App.Controllers
{
    public class VeiculoController : Controller
    {
        private readonly IVeiculoServiceApp _veiculoServiceApp;
        private readonly IFabricanteServiceApp _fabricanteServiceApp;
        public VeiculoController(IVeiculoServiceApp veiculoServiceApp,
                                 IFabricanteServiceApp fabricanteServiceApp)
        {
            _veiculoServiceApp = veiculoServiceApp;
            _fabricanteServiceApp = fabricanteServiceApp;
        }

        public IActionResult Index()
        {
            var model = new VeiculoViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Veiculo = null,
                Lista = null,
                ListaFabricante = null,
                Acao = "L"
            };

            return View(model);
        }

        public async Task<VeiculoViewModel> Consultar(int limit, int offset, string search, int colOrder, string dirOrder)
        {
            var lista = (await _veiculoServiceApp.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));

            var model = new VeiculoViewModel
            {
                recordsTotal = lista.Object!.Count,
                recordsFiltered = lista.Object!.Count,
                Veiculo = null,
                Lista = lista.Object,
                Acao = "L"
            };

            return model;
        }

        public async Task<IActionResult> Incluir()
        {
            var model = new VeiculoViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Veiculo = new VeiculoDto(),
                ListaFabricante = (await _fabricanteServiceApp.Consultar()).Object!.OrderBy(x => x.Nome).ToList(),
                Lista = null,
                Acao = "I"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Alterar(int id)
        {
            var model = new VeiculoViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Veiculo = (await _veiculoServiceApp.ConsultarPorId(id)).Object!.FirstOrDefault(),
                ListaFabricante = (await _fabricanteServiceApp.Consultar()).Object!.OrderBy(x => x.Nome).ToList(),
                Lista = null,
                Acao = "A"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Visualizar(int id)
        {
            var model = new VeiculoViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Veiculo = (await _veiculoServiceApp.ConsultarPorId(id)).Object!.FirstOrDefault(),
                ListaFabricante = (await _fabricanteServiceApp.Consultar()).Object!.OrderBy(x => x.Nome).ToList(),
                Lista = null,
                Acao = "V"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Salvar(VeiculoDto obj)
        {
            var retorno = await _veiculoServiceApp.Salvar(obj);
            if (retorno.Success)
            {
                TempData["sucesso"] = retorno.Message;

                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = retorno.Message;

                var model = new VeiculoViewModel
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    Veiculo = obj,
                    ListaFabricante = (await _fabricanteServiceApp.Consultar()).Object!.OrderBy(x => x.Nome).ToList(),
                    Lista = null,
                    Acao = obj.Id == null ? "A" : "I"
                };

                return View("Cadastro", model);
            }
        }
        public async Task<IActionResult> Excluir(int id)
        {
            var retorno = await _veiculoServiceApp.Deletar(id);
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

        public async Task<List<VeiculoDto>> ConsultarPorFabricante(int id)
        {    
            return (await _veiculoServiceApp.ConsultarPorFabricante(id)).Object!;
        }
    }
}
