using Application.Dtos.Crud;
using Application.Enums;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class VeiculoDto : DtoCrud
    {
        public string? Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        public TipoVeiculoEnum TipoVeiculo { get; set; }
        public string? Descricao { get; set; }

        public FabricanteDto? Fabricante { get; set; }

        public string TipoVeiculoName 
        {
            get
            {
                var displayAttribute = TipoVeiculo.GetType()
                                        .GetField(TipoVeiculo.ToString())
                                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                as DisplayAttribute[];

                return displayAttribute.Length > 0 ? displayAttribute[0].Name : TipoVeiculo.ToString();
            }
        }
    }
}
