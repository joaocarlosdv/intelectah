using Application.Dtos.Crud;

namespace Application.Dtos
{
    public class ClienteDto : DtoCrud
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
    }
}
