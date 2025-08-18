namespace NHTSAVehicleApi.Model
{
    public class NHTSARecall
    {
        public string? Manufacturer { get; set; }
        public string? NHTSACampaignNumber { get; set; }
        public bool parkIt { get; set; }
        public bool parkOutSide { get; set; }
        public bool overTheAirUpdate { get; set; }
        public string? ReportReceivedDate { get; set; }
        public string? Component {  get; set; }
        public string? Summary { get; set; }
        public string? Consequence { get; set; }
        public string? Remedy { get; set; }
        public string? Notes { get; set; }
        public string? ModelYear { get; set; }
        public string? Make {  get; set; }
        public string? Model {  get; set; }
    }
}
