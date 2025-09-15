using Application.Dtos;
using CronosPlus_App.Models.Crud;

namespace CronosPlus_App.Models
{
    public class UsuarioViewModel : ViewModelCrud
    {
        public List<UsuarioDto>? Lista { get; set; }
        public UsuarioDto? Usuario { get; set; }
    }
}
