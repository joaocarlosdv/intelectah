using Acesso.Helpers;
using Application.Dtos;
using Application.Dtos.Crud;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Intelectah_App.Services.Crud
{
    public abstract class CrudServiceApp<Dto> : ICrudServiceApp<Dto> where Dto : DtoCrud
    {
        public readonly HttpClient _httpClient;
        public readonly IConfiguration _configApp;

        public CrudServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor)
        {
            _configApp = configApp;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", UsuarioConectadoHelper.GetToken(httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity));           
        }        
        public virtual async Task<OperationResponseDto<List<Dto>>> Consultar()
        {
            var result = await _httpClient.GetAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"{typeof(Dto).Name.Replace("Dto","")}/Consultar");

            if (!result.IsSuccessStatusCode)
                return new OperationResponseDto<List<Dto>>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new OperationResponseDto<List<Dto>>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            return requisicao.ToObject<OperationResponseDto<List<Dto>>>();
        }

        public virtual async Task<OperationResponseDto<List<Dto>>> ConsultarPorId(int id)
        {
            var result = await _httpClient.GetAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"{typeof(Dto).Name.Replace("Dto", "")}/ConsultarPorId?id={id}");

            if (!result.IsSuccessStatusCode)
                return new OperationResponseDto<List<Dto>>
                {
                    Success = false,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new OperationResponseDto<List<Dto>>
                {
                    Success = false,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            return requisicao.ToObject<OperationResponseDto<List<Dto>>>();
        }

        public virtual async Task<OperationResponseDto<List<Dto>>> ConsultaPaginada(int limit = 10, int offset = 1, string? search = null, int colOrder = 0, string dirOrder = "asc")
        {
            var result = await _httpClient.GetAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"{typeof(Dto).Name.Replace("Dto", "")}/ConsultaPaginada?limit={limit}&offset={offset}&search={search}&colOrder{colOrder}&dirOrder{dirOrder}");

            if (!result.IsSuccessStatusCode)
                return new OperationResponseDto<List<Dto>>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new OperationResponseDto<List<Dto>>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            return requisicao.ToObject<OperationResponseDto<List<Dto>>>();
        }

        public virtual async Task<OperationResponseDto<int>> ConsultaPaginadaCount(string? search = null, int colOrder = 0, string dirOrder = "asc")
        {
            var result = await _httpClient.GetAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"{typeof(Dto).Name.Replace("Dto", "")}/ConsultaPaginadaCount?search={search}&colOrder{colOrder}&dirOrder{dirOrder}");

            if (!result.IsSuccessStatusCode)
                return new OperationResponseDto<int>
                {
                    Success = true,
                    Object = 0,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new OperationResponseDto<int>
                {
                    Success = true,
                    Object = 0,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            return requisicao.ToObject<OperationResponseDto<int>>();
        }

        public virtual async Task<OperationResponseDto<Dto>> Salvar(Dto dto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"{typeof(Dto).Name.Replace("Dto", "")}/Salvar", stringContent);

            if (!result.IsSuccessStatusCode)
                return new OperationResponseDto<Dto>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new OperationResponseDto<Dto>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            return requisicao.ToObject<OperationResponseDto<Dto>>();
        }

        public virtual async Task<OperationResponseDto<Dto>> Deletar(int id)
        {
            var result = await _httpClient.GetAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"{typeof(Dto).Name.Replace("Dto", "")}/Deletar?id={id}");

            if (!result.IsSuccessStatusCode)
                return new OperationResponseDto<Dto>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new OperationResponseDto<Dto>
                {
                    Success = true,
                    Object = null,
                    Message = result.RequestMessage!.ToString(),
                    Exception = null
                };

            return requisicao.ToObject<OperationResponseDto<Dto>>();
        }
    }
}
