using Application.Dtos;
using Application.Helpers;

namespace Application.Validators
{
    public static class FabricanteValidator
    {
        public static RetornoValidacao Validar(FabricanteDto fabricante)
        {
            if (fabricante.AnoFundacao >= DateTime.Now.Year)
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "Ano inválido."
                };
            }
            if (!ApiHelper.ValidaUrl(fabricante.WebSite!))
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "Site inválido."
                };
            }

            return new RetornoValidacao
            {
                isValido = true,
                Mensagem = string.Empty
            };
        }
    }
}
