using Application.Dtos;
using Intelectah_App.Services.Crud;
using Intelectah_App.Services.Interfaces;

namespace Intelectah_App.Services
{
    public class VendasServiceApp : CrudServiceApp<VendasDto>, IVendasServiceApp
    {
        public VendasServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base(configApp, httpContextAccessor)
        {
        }
    }
}
