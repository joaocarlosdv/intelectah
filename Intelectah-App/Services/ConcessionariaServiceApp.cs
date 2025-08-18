using ApiExterna.Models;
using Application.Dtos;
using Intelectah_App.Services.Crud;
using Intelectah_App.Services.Interfaces;
using Newtonsoft.Json;

namespace Intelectah_App.Services
{
    public class ConcessionariaServiceApp : CrudServiceApp<ConcessionariaDto>, IConcessionariaServiceApp
    {
        public ConcessionariaServiceApp(IConfiguration configApp, IHttpContextAccessor httpContextAccessor) : base(configApp, httpContextAccessor)
        {
        }

        public async Task<Cep> ConsultarCep(string cep)
        {
            var result = await _httpClient.GetAsync(_configApp.GetSection("ConfigAppSettings").GetSection("UrlBaseApi").Value + $"ApiExterna/ConsultaCep?cep={cep}");

            if (!result.IsSuccessStatusCode)
                return new Cep();

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new Cep();

            return requisicao.ToObject<Cep>();
        }
    }
}
