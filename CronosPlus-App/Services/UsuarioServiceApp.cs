using Application.Dtos;
using CronosPlus_App.Services.Crud;
using CronosPlus_App.Services.Interfaces;

namespace CronosPlus_App.Services
{
    public class UsuarioServiceApp : CrudServiceApp<UsuarioDto>, IUsuarioServiceApp
    {
        public UsuarioServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base(configApp, httpContextAccessor)
        {
        }
    }
}
