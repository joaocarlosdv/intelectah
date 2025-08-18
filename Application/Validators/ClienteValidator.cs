using Application.Dtos;
using Application.Helpers;

namespace Application.Validators
{
    public static class ClienteValidator
    {
        public static RetornoValidacao Validar(ClienteDto cliente)
        {            
            if (!ApiHelper.ValidaCpf(cliente.Cpf!))
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "CPF inválido."
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
