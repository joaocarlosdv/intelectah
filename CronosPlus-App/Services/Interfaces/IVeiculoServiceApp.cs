using Application.Dtos;
using CronosPlus_App.Services.Crud;

namespace CronosPlus_App.Services.Interfaces
{
    public interface IVeiculoServiceApp : ICrudServiceApp<VeiculoDto>
    {
        Task<OperationResponseDto<List<VeiculoDto>>> ConsultarPorFabricante(int id);
    }
}
