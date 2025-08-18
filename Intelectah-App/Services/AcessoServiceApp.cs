using Acesso.Dtos;
using Application.Dtos;
using Intelectah_App.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Intelectah_App.Services
{
    public class AcessoServiceApp : IAcessoServiceApp
    {
        public readonly HttpClient _httpClient;
        public readonly IConfiguration _configApp;

        public AcessoServiceApp(IConfiguration configApp)
        {
            _httpClient = new HttpClient();
            _configApp = configApp;
        }
        public async Task<UsuarioAutenticadoDto> Login(LoginDto loginDto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"Login/Login", stringContent);

            if (!result.IsSuccessStatusCode)
                return new UsuarioAutenticadoDto
                    {
                        Autenticado = false,
                        Mensagem = result.RequestMessage!.ToString(),
                    };

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new UsuarioAutenticadoDto
                {
                    Autenticado = false,
                    Mensagem = "Falha na requisição.",
                };

            return requisicao.ToObject<UsuarioAutenticadoDto>();
        }
    }
}
