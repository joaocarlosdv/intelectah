using Application.Dtos;
using Application.Helpers;

namespace Application.Validators
{
    public static class ConcessionariaValidator
    {
        public static RetornoValidacao Validar(ConcessionariaDto concessionaria)
        {
            if (!ApiHelper.ValidaCep(concessionaria.Cep!))
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "CEP inválido."
                };
            }
            if (!ApiHelper.ValidaEmail(concessionaria.Email!))
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "E-mail inválido."
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
