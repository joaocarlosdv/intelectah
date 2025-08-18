using Application.Dtos.Crud;

namespace Application.Dtos
{
    public class FabricanteDto : DtoCrud
    {
        public string? Nome { get; set; }
        public string? PaisOrigem { get; set; }
        public int AnoFundacao { get; set; }
        public string? WebSite { get; set; }
    }
}
