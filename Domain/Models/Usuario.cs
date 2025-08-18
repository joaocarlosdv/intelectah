using Domain.Models.Crud;

namespace Domain.Models
{
    public class Usuario : ModelCrud
    {
        public string? Nome {  get; set; }
        public string? Senha { get; set; }
        public string? Email { get; set; }
        public int NivelAcesso { get; set; }
    }
}
