using Application.Dtos;
using Application.Helpers;

namespace Application.Validators
{
    public static class UsuarioValidator
    {
        public static RetornoValidacao Validar(UsuarioDto usuario)
        {
            if (!ApiHelper.ValidaEmail(usuario.Email!))
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "E-Mail inválido."
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
