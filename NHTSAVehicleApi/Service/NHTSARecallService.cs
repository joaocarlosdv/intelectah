using Newtonsoft.Json;
using NHTSAVehicleApi.Model;

namespace NHTSAVehicleApi.Service
{
    public class NHTSARecallService : INHTSARecallService
    {
        public async Task<NHTSAResponse> ConsultarRecalls(string make, string model, int modelYear)
        {
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetAsync($"https://api.nhtsa.gov/recalls/recallsByVehicle?make={make}&model={model}&modelYear={modelYear}");

            if (!result.IsSuccessStatusCode)
                return new NHTSAResponse();

            var responseBody = await result.Content.ReadAsStringAsync();
            var requisicao = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (requisicao == null)
                return new NHTSAResponse();

            var retorno = requisicao.ToObject<NHTSAResponse>();

            return retorno;
        }
    }
}
