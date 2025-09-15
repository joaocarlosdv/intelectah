using Application.Dtos;
using CronosPlus_App.Models;
using CronosPlus_App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CronosPlus_App.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServiceApp _usuarioService;
        public UsuarioController(IUsuarioServiceApp usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            var model = new UsuarioViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Usuario = null,
                Lista = null,
                Acao = "L"
            };

            return View(model);
        }

        public async Task<UsuarioViewModel> Consultar(int limit, int offset, string search, int colOrder, string dirOrder)
        {
            var lista = (await _usuarioService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));

            var model = new UsuarioViewModel
            {
                recordsTotal = lista.Object!.Count,
                recordsFiltered = lista.Object!.Count,
                Usuario = null,
                Lista = lista.Object,
                Acao = "L"
            };

            return model;
        }

        public IActionResult Incluir()
        {
            var model = new UsuarioViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Usuario = new UsuarioDto(),
                Lista = null,
                Acao = "I"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Alterar(int id)
        {
            var model = new UsuarioViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Usuario = (await _usuarioService.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "A"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Visualizar(int id)
        {
            var model = new UsuarioViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Usuario = (await _usuarioService.ConsultarPorId(id)).Object!.FirstOrDefault(),
                Lista = null,
                Acao = "V"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Salvar(UsuarioDto obj)
        {
            var retorno = await _usuarioService.Salvar(obj);
            if (retorno.Success)
            {
                TempData["sucesso"] = retorno.Message;

                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = retorno.Message;

                var model = new UsuarioViewModel
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    Usuario = obj,
                    Lista = null,
                    Acao = obj.Id == null ? "A" : "I"
                };

                return View("Cadastro", model);
            }
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var retorno = await _usuarioService.Deletar(id);
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
