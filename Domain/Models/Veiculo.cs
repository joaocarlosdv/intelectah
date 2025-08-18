using Domain.Models.Crud;
using Newtonsoft.Json;

namespace Domain.Models
{
    public class Veiculo : ModelCrud
    {
        public string? Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        public int TipoVeiculo { get; set; }
        public string? Descricao { get; set; }

        public Fabricante? Fabricante { get; set; }
        [JsonIgnore]
        public List<Vendas>? ListaVendas { get; private set; }
    }
}
