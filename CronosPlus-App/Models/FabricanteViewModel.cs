using Application.Dtos;
using CronosPlus_App.Models.Crud;

namespace CronosPlus_App.Models
{
    public class FabricanteViewModel : ViewModelCrud
    {        
        public List<FabricanteDto>? Lista { get; set; }
        public FabricanteDto? Fabricante { get; set; }
    }
}
