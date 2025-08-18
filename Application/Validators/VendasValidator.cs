using Application.Dtos;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class VendasValidator
    {
        public static RetornoValidacao Validar(VendasDto venda)
        {
            if (venda.DataVenda > DateTime.Now)
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "Data não pode ser futura."
                };
            }

            if (venda.PrecoVenda > venda.Veiculo!.Preco)
            {
                return new RetornoValidacao
                {
                    isValido = false,
                    Mensagem = "Preço da venda não pode ser maior que i preço do veículo."
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
