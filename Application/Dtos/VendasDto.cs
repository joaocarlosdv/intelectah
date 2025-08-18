using Application.Dtos.Crud;

namespace Application.Dtos
{
    public class VendasDto : DtoCrud
    {
        public int VeiculoId { get; set; }
        public int ConcessionariaId { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal PrecoVenda { get; set; }
        public string? ProtocoloVenda { get; set; }

        public VeiculoDto? Veiculo { get; set; }
        public ConcessionariaDto? Concessionaria { get; set; }
        public ClienteDto? Cliente { get; set; }
    }
}
