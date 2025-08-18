using Application.Dtos;

namespace Acesso.Dtos
{
    public class UsuarioAutenticadoDto
    {
        public UsuarioDto Usuario { get; set; }
        public bool Autenticado { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string? Token { get; set; }
        public string? Mensagem { get; set; }
    }
}
