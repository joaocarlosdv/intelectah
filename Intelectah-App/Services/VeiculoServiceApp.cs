using Application.Dtos;
using Intelectah_App.Services.Crud;
using Intelectah_App.Services.Interfaces;
using Newtonsoft.Json;

namespace Intelectah_App.Services
{
    public class VeiculoServiceApp : CrudServiceApp<VeiculoDto>, IVeiculoServiceApp
    {
        public VeiculoServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base(configApp, httpContextAccessor)
        {
        }

        public virtual async Task<OperationResponseDto<List<VeiculoDto>>> ConsultarPorFabricante(int id)
        {
            var result = await _httpClient.GetAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"{typeof(VeiculoDto).Name.Replace("Dto", "")}/ConsultaPorFabricante?fabricanteId={id}");

            if (!result.IsSuccessStatusCode)
                return new OperationResponseDto<List<VeiculoDto>>
                {
                    Success = false,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new OperationResponseDto<List<VeiculoDto>>
                {
                    Success = false,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            return requisicao.ToObject<OperationResponseDto<List<VeiculoDto>>>();
        }
    }
}
