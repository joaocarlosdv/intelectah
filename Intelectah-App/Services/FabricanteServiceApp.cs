using Application.Dtos;
using Intelectah_App.Services.Crud;
using Intelectah_App.Services.Interfaces;

namespace Intelectah_App.Services
{
    public class FabricanteServiceApp : CrudServiceApp<FabricanteDto>, IFabricanteServiceApp
    {
        public FabricanteServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base (configApp, httpContextAccessor)
        {
        }
    }
}
