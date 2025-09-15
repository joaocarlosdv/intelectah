using Application.Dtos;
using CronosPlus_App.Models.Crud;

namespace CronosPlus_App.Models
{
    public class ConcessionariaViewModel : ViewModelCrud
    {
        public List<ConcessionariaDto>? Lista { get; set; }
        public ConcessionariaDto? Concessionaria { get; set; }
    }
}
