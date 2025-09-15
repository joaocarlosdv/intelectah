using Application.Dtos;
using CronosPlus_App.Models.Crud;

namespace CronosPlus_App.Models
{
    public class ClienteViewModel : ViewModelCrud
    {
        public List<ClienteDto>? Lista { get; set; }
        public ClienteDto? Cliente { get; set; }
    }
}
