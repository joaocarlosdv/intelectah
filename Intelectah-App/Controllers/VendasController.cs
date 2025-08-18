using Application.Dtos;
using Domain.Models;
using Intelectah_App.Models;
using Intelectah_App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Intelectah_App.Controllers
{
    public class VendasController : Controller
    {
        private readonly IVendasServiceApp _vendasService;
        private readonly IClienteServiceApp _clienteService;
        private readonly IConcessionariaServiceApp _concessionariaService;
        private readonly IFabricanteServiceApp _fabricanteService;
        private readonly IVeiculoServiceApp _veiculoService;
        public VendasController(IVendasServiceApp vendasService,
                                IClienteServiceApp clienteService,
                                IConcessionariaServiceApp concessionariaService,
                                IFabricanteServiceApp fabricanteService,
                                IVeiculoServiceApp veiculoService)
        {
            _vendasService = vendasService;
            _clienteService = clienteService;
            _concessionariaService = concessionariaService;
            _fabricanteService = fabricanteService;
            _veiculoService = veiculoService;
        }

        public IActionResult Index()
        {
            var model = new VendasViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Vendas = null,
                Lista = null,
                Acao = "L"
            };

            return View(model);
        }

        public async Task<VendasViewModel> Consultar(int limit, int offset, string search, int colOrder, string dirOrder)
        {
            var lista = (await _vendasService.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));

            var model = new VendasViewModel
            {
                recordsTotal = lista.Object!.Count,
                recordsFiltered = lista.Object!.Count,
                Vendas = null,
                Lista = lista.Object,
                Acao = "L"
            };

            return model;
        }

        public async Task<IActionResult> Incluir()
        {
            var model = new VendasViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Vendas = new VendasDto
                {
                    Cliente = new ClienteDto(),
                    Veiculo = new VeiculoDto(),
                    Concessionaria = new ConcessionariaDto(),
                },
                Lista = null,
                ListaCliente = (await _clienteService.Consultar()).Object,
                ListaConcessionaria = (await _concessionariaService.Consultar()).Object,
                ListaFabricante = (await _fabricanteService.Consultar()).Object,
                Acao = "I"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Alterar(int id)
        {
            var venda = (await _vendasService.ConsultarPorId(id)).Object!.FirstOrDefault();
            var model = new VendasViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Vendas = venda,
                Lista = null,
                ListaCliente = (await _clienteService.Consultar()).Object,
                ListaConcessionaria = (await _concessionariaService.Consultar()).Object,
                ListaFabricante = (await _fabricanteService.Consultar()).Object,
                ListaVeiculo = (await _veiculoService.ConsultarPorFabricante(venda!.Veiculo!.FabricanteId)).Object,
                Acao = "A"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Visualizar(int id)
        {
            var venda = (await _vendasService.ConsultarPorId(id)).Object!.FirstOrDefault();
            var model = new VendasViewModel
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                Vendas = venda,
                Lista = null,
                ListaCliente = (await _clienteService.Consultar()).Object,
                ListaConcessionaria = (await _concessionariaService.Consultar()).Object,
                ListaFabricante = (await _fabricanteService.Consultar()).Object,
                ListaVeiculo = (await _veiculoService.ConsultarPorFabricante(venda!.Veiculo!.FabricanteId)).Object,
                Acao = "V"
            };

            return View("Cadastro", model);
        }

        public async Task<IActionResult> Salvar(VendasDto obj)
        {
            var retorno = await _vendasService.Salvar(obj);
            if (retorno.Success)
            {
                TempData["sucesso"] = retorno.Message;

                return RedirectToAction("Index");
            }
            else
            {
                TempData["erro"] = retorno.Message;

                obj.Veiculo = new VeiculoDto();

                var model = new VendasViewModel
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    Vendas = obj,
                    Lista = null,
                    ListaCliente = (await _clienteService.Consultar()).Object,
                    ListaConcessionaria = (await _concessionariaService.Consultar()).Object,
                    ListaFabricante = (await _fabricanteService.Consultar()).Object,
                    ListaVeiculo = (await _veiculoService.ConsultarPorFabricante(obj!.Veiculo!.FabricanteId)).Object,
                    Acao = obj.Id == null ? "A" : "I"
                };

                return View("Cadastro", model);
            }
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var retorno = await _vendasService.Deletar(id);
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
