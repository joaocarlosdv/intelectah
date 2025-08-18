using Acesso.Dtos;
using Acesso.Helpers;
using Acesso.Service.Interfaces;
using Application.Dtos;
using Application.Helpers;
using AutoMapper;
using DataAccess.Repository.Interfaces;

namespace Acesso.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public readonly IMapper _mapper;

        public LoginService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioAutenticadoDto> LoginAsync(LoginDto login)
        {
            var usuario = await _usuarioRepository.Consultar(x => x.Email == login.Login, null);
            if (!usuario.Success || usuario.Object == null || usuario.Object.Count == 0)
            {
                return new UsuarioAutenticadoDto
                {
                    Autenticado = false,
                    Mensagem = "Usuário não localizado."
                };
            }

            if (usuario.Object.FirstOrDefault()!.Senha != ApiHelper.CriptoMD5(login.Senha!))
            {
                return new UsuarioAutenticadoDto
                {
                    Autenticado = false,
                    Mensagem = "Senha inválida."
                };
            }

            var autentic = new UsuarioAutenticadoDto();
            autentic.Autenticado = true;
            autentic.DataCriacao = DateTime.Now;
            autentic.DataExpiracao = DateTime.Now.AddHours(4);
            autentic.Usuario = _mapper.Map<UsuarioDto>(usuario.Object.FirstOrDefault());
            autentic.Token = TokenHelper.GenerateToken(autentic);
            autentic.Mensagem = "Usuário autenticado.";
            
            return autentic;
        }
    }
}
