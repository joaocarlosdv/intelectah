using Application.Dtos;
using CronosPlus_App.Models;
using CronosPlus_App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CronosPlus_App.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteServiceApp _clienteService;
        public ClienteController(IClienteServiceApp clienteService)
        {
            _clienteService = clienteService;
        }

        public IActionResult Index()
        {
            var model = new ClienteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Cliente = null,
                Lista = null,
                Acao = "L"
            };

            return View(model);
        }

        public async Task<ClienteViewModel> Consultar(int limit, int offset, string search, int colOrder, string dirOrder)
        {
            var lista = (await _clienteService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));

            var model = new ClienteViewModel
            {
                recordsTotal = lista.Object!.Count,
                recordsFiltered = lista.Object!.Count,
                Cliente = null,
                Lista = lista.Object,
                Acao = "L"
            };

            return model;
        }

        public IActionResult Incluir()
        {
            var model = new ClienteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Cliente = new ClienteDto(),
                Lista = null,
                Acao = "I"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Alterar(int id)
        {
            var model = new ClienteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Cliente = (await _clienteService.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "A"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Visualizar(int id)
        {
            var model = new ClienteViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Cliente = (await _clienteService.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "V"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Salvar(ClienteDto obj)
        {
            var retorno = await _clienteService.Salvar(obj);
            if (retorno.Success)
            {
                TempData["sucesso"] = retorno.Message;

                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = retorno.Message;

                var model = new ClienteViewModel
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    Cliente = obj,
                    Lista = null,
                    Acao = obj.Id == null ? "A" : "I"
                };

                return View("Cadastro", model);
            }
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var retorno = await _clienteService.Deletar(id);
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
