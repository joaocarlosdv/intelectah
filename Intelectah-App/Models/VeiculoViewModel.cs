using Application.Dtos;
using Intelectah_App.Models.Crud;

namespace Intelectah_App.Models
{
    public class VeiculoViewModel : ViewModelCrud
    {
        public List<FabricanteDto>? ListaFabricante { get; set; }
        public List<VeiculoDto>? Lista { get; set; }
        public VeiculoDto? Veiculo { get; set; }
    }
}
