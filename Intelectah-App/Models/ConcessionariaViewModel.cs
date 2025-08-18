using Application.Dtos;
using Intelectah_App.Models.Crud;

namespace Intelectah_App.Models
{
    public class ConcessionariaViewModel : ViewModelCrud
    {
        public List<ConcessionariaDto>? Lista { get; set; }
        public ConcessionariaDto? Concessionaria { get; set; }
    }
}
