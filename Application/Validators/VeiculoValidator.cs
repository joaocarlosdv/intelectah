using Application.Dtos;

namespace Application.Validators
{
    public static class VeiculoValidator
    {
        public static RetornoValidacao Validar(VeiculoDto veiculo)
        {
            if (veiculo.AnoFabricacao > DateTime.Now.Year)
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "Ano inválido."
                };
            }
            if (veiculo.Preco <= 0)
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "Valor precisa ser positivo."
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
