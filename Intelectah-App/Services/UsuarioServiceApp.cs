using Application.Dtos;
using Intelectah_App.Services.Crud;
using Intelectah_App.Services.Interfaces;

namespace Intelectah_App.Services
{
    public class UsuarioServiceApp : CrudServiceApp<UsuarioDto>, IUsuarioServiceApp
    {
        public UsuarioServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base(configApp, httpContextAccessor)
        {
        }
    }
}
