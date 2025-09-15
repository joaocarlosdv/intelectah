using Application.Dtos;
using CronosPlus_App.Models.Crud;

namespace CronosPlus_App.Models
{
    public class VendasViewModel : ViewModelCrud
    {
        public List<VendasDto>? Lista { get; set; }
        public List<ClienteDto>? ListaCliente { get; set; }
        public List<ConcessionariaDto>? ListaConcessionaria { get; set; }
        public List<FabricanteDto>? ListaFabricante { get; set; }
        public List<VeiculoDto>? ListaVeiculo { get; set; }
        public VendasDto? Vendas { get; set; }
    }
}
