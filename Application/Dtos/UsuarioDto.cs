using Application.Dtos.Crud;
using Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class UsuarioDto : DtoCrud
    {
        public string? Nome { get; set; }
        public string? Senha { get; set; }
        public string? Email { get; set; }
        public NivelAcessoEnum NivelAcesso { get; set; }
        public string NivelAcessoName
        {
            get
            {
                var displayAttribute = NivelAcesso.GetType()
                                        .GetField(NivelAcesso.ToString())!
                                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                as DisplayAttribute[];

                return displayAttribute.Length > 0 ? displayAttribute[0].Name : NivelAcesso.ToString();
            }
        }
    }
}
