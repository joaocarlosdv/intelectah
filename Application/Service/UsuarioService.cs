using Application.Dtos;
using Application.Helpers;
using Application.Service.Crud;
using Application.Service.Interface;
using Application.Validators;
using AutoMapper;
using DataAccess.Repository.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Service
{
    public class UsuarioService : CrudService<Usuario, UsuarioDto>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IMapper mapper,
                              IUsuarioRepository repositorio,
                              IHttpContextAccessor accessor) : base(mapper, repositorio, accessor)
        {
            _usuarioRepository = repositorio;
        }

        public override async Task<OperationResponseDto<UsuarioDto>> Salvar(UsuarioDto dto)
        {
            var validacao = UsuarioValidator.Validar(dto);
            if (!validacao.isValido)
            {
                return new OperationResponseDto<UsuarioDto>
                {
                    Success = false,
                    Object = dto,
                    Message = validacao.Mensagem,
                    Exception = null
                };
            }

            if (dto.Id == 0 || dto.Id == null)
            {
                dto.Senha = ApiHelper.CriptoMD5(dto.Senha!);

                var jaCadastrado = (await Consultar(x => x.Nome == dto.Nome, null)).Object;
                if (jaCadastrado != null && jaCadastrado.Count > 0)
                {
                    if (jaCadastrado.FirstOrDefault()!.Deletado)
                    {
                        jaCadastrado.FirstOrDefault()!.Deletado = false;
                        jaCadastrado.FirstOrDefault()!.Nome = dto.Nome;
                        jaCadastrado.FirstOrDefault()!.Email = dto.Email;
                        jaCadastrado.FirstOrDefault()!.Senha = dto.Senha;
                        jaCadastrado.FirstOrDefault()!.NivelAcesso = dto.NivelAcesso;

                        return _mapper.Map<OperationResponseDto<UsuarioDto>>(await _repositorio.Alterar(_mapper.Map<Usuario>(jaCadastrado.FirstOrDefault()!)));
                    }
                    else
                    {
                        return new OperationResponseDto<UsuarioDto>
                        {
                            Success = false,
                            Object = dto,
                            Message = "Usuário já cadastrado.",
                            Exception = null
                        };
                    }
                }

                return _mapper.Map<OperationResponseDto<UsuarioDto>>(await _repositorio.Inserir(_mapper.Map<Usuario>(dto)));
            }
            else
            {
                var UsuarioAnt = (await Consultar(x => x.Id == dto.Id, null)).Object!.FirstOrDefault();
                dto.Senha = UsuarioAnt.Senha;

                return _mapper.Map<OperationResponseDto<UsuarioDto>>(await _repositorio.Alterar(_mapper.Map<Usuario>(dto)));
            }
        }

        public async Task<OperationResponseDto<List<UsuarioDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            return _mapper.Map<OperationResponseDto<List<UsuarioDto>>>(await _usuarioRepository.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
        }
    }
}
