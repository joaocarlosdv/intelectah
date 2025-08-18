using Domain.Models.Crud;
using Newtonsoft.Json;

namespace Domain.Models
{
    public class Fabricante : ModelCrud
    {
        public string? Nome { get; set; }
	    public string? PaisOrigem { get; set; }
        public int AnoFundacao { get; set; }
        public string? WebSite { get; set; }

        [JsonIgnore]
        public List<Veiculo>? ListaVeiculo { get; private set; }
    }
}
