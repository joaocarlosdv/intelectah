using Application.Dtos;
using Intelectah_App.Models.Crud;

namespace Intelectah_App.Models
{
    public class ClienteViewModel : ViewModelCrud
    {
        public List<ClienteDto>? Lista { get; set; }
        public ClienteDto? Cliente { get; set; }
    }
}
