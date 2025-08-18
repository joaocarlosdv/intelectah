using NHTSAVehicleApi.Model;

namespace NHTSAVehicleApi.Service
{
    public interface INHTSARecallService
    {
        Task<NHTSAResponse> ConsultarRecalls(string make, string model, int modelYear);
    }
}
