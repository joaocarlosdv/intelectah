using Application.Dtos;
using CronosPlus_App.Services.Crud;
using CronosPlus_App.Services.Interfaces;

namespace CronosPlus_App.Services
{
    public class FabricanteServiceApp : CrudServiceApp<FabricanteDto>, IFabricanteServiceApp
    {
        public FabricanteServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base (configApp, httpContextAccessor)
        {
        }
    }
}
