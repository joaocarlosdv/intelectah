using Application.Dtos;
using CronosPlus_App.Services.Crud;
using CronosPlus_App.Services.Interfaces;

namespace CronosPlus_App.Services
{
    public class ClienteServiceApp : CrudServiceApp<ClienteDto>, IClienteServiceApp
    {
        public ClienteServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base(configApp, httpContextAccessor)
        {
        }
    }
}
