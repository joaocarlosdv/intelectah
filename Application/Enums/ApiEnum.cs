using System.ComponentModel.DataAnnotations;

namespace Application.Enums
{
    public enum TipoVeiculoEnum
    {
        [Display(Name = "Carro")]
        Carro = 0,
        [Display(Name = "Moto")]
        Moto = 1,
        [Display(Name = "Caminhão")]
        Caminhão = 2,
    }

    public enum NivelAcessoEnum
    {
        [Display(Name = "Administrador")]
        Administrador = 0,
        [Display(Name = "Gerente")]
        Gerente = 1,
        [Display(Name = "Vendedor")]
        Vendedor = 2,
    }
}
