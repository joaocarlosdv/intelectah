using Domain.Models.Crud;
using Newtonsoft.Json;

namespace Domain.Models
{
    public class Cliente : ModelCrud
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        [JsonIgnore]
        public List<Vendas>? ListaVendas { get; private set; }
    }
}
