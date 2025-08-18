using Application.Dtos;
using Intelectah_App.Models.Crud;

namespace Intelectah_App.Models
{
    public class FabricanteViewModel : ViewModelCrud
    {        
        public List<FabricanteDto>? Lista { get; set; }
        public FabricanteDto? Fabricante { get; set; }
    }
}
