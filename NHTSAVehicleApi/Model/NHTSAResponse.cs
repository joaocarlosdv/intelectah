namespace NHTSAVehicleApi.Model
{
    public class NHTSAResponse
    {
        public int Count { get; set; }
        public string? Message { get; set; }
        public List<NHTSARecall>? results { get; set; }
    }
}
