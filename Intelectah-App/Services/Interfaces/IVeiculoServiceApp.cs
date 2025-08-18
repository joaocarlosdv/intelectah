using Application.Dtos;
using Intelectah_App.Services.Crud;

namespace Intelectah_App.Services.Interfaces
{
    public interface IVeiculoServiceApp : ICrudServiceApp<VeiculoDto>
    {
        Task<OperationResponseDto<List<VeiculoDto>>> ConsultarPorFabricante(int id);
    }
}
