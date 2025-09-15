using Application.Dtos;
using CronosPlus_App.Services.Crud;
using CronosPlus_App.Services.Interfaces;

namespace CronosPlus_App.Services
{
    public class VendasServiceApp : CrudServiceApp<VendasDto>, IVendasServiceApp
    {
        public VendasServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base(configApp, httpContextAccessor)
        {
        }
    }
}
