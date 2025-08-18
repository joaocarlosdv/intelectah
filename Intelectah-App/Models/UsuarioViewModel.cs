using Application.Dtos;
using Intelectah_App.Models.Crud;

namespace Intelectah_App.Models
{
    public class UsuarioViewModel : ViewModelCrud
    {
        public List<UsuarioDto>? Lista { get; set; }
        public UsuarioDto? Usuario { get; set; }
    }
}
