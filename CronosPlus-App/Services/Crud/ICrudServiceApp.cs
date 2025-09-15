using Application.Dtos;

namespace CronosPlus_App.Services.Crud
{
    public interface ICrudServiceApp<Dto>
    {
        Task<OperationResponseDto<List<Dto>>> Consultar();
        Task<OperationResponseDto<List<Dto>>> ConsultarPorId(int id);
        Task<OperationResponseDto<List<Dto>>> ConsultaPaginada(int limit = 10, int offset = 0, string? search = null, int colOrder = 0, string dirOrder = "asc");
        Task<OperationResponseDto<int>> ConsultaPaginadaCount(string? search = null, int colOrder = 0, string dirOrder = "asc");
        Task<OperationResponseDto<Dto>> Salvar(Dto dto);
        Task<OperationResponseDto<Dto>> Deletar(int id);
    }
}
