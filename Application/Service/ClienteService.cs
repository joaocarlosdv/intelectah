using Application.Dtos;
using Application.Service.Crud;
using Application.Service.Interface;
using Application.Validators;
using AutoMapper;
using DataAccess.Repository.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Service
{
    public class ClienteService : CrudService<Cliente, ClienteDto>, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IMapper mapper,
                              IClienteRepository repositorio,
                              IHttpContextAccessor accessor) : base(mapper, repositorio, accessor)
        {
            _clienteRepository = repositorio;
        }

        public override async Task<OperationResponseDto<ClienteDto>> Salvar(ClienteDto dto)
        {
            var validacao = ClienteValidator.Validar(dto);
            if (!validacao.isValido)
            {
                return new OperationResponseDto<ClienteDto>
                {
                    Success = false,
                    Object = dto,
                    Message = validacao.Mensagem,
                    Exception = null
                };
            }            

            if (dto.Id == 0 || dto.Id == null)
            {
                var jaCadastrado = (await Consultar(x => x.Cpf == dto.Cpf, null)).Object;
                if (jaCadastrado != null && jaCadastrado.Count > 0)
                {
                    if (jaCadastrado.FirstOrDefault()!.Deletado)
                    {
                        jaCadastrado.FirstOrDefault()!.Deletado = false;
                        jaCadastrado.FirstOrDefault()!.Nome = dto.Nome;
                        jaCadastrado.FirstOrDefault()!.Telefone = dto.Telefone;

                        return _mapper.Map<OperationResponseDto<ClienteDto>>(await _repositorio.Alterar(_mapper.Map<Cliente>(jaCadastrado.FirstOrDefault()!)));
                    }
                    else
                    {
                        return new OperationResponseDto<ClienteDto>
                        {
                            Success = false,
                            Object = dto,
                            Message = "Cliente já cadastrado com este CPF.",
                            Exception = null
                        };
                    }
                }                

                return _mapper.Map<OperationResponseDto<ClienteDto>>(await _repositorio.Inserir(_mapper.Map<Cliente>(dto)));
            }
            else
            {
                return _mapper.Map<OperationResponseDto<ClienteDto>>(await _repositorio.Alterar(_mapper.Map<Cliente>(dto)));
            }
        }

        public async Task<OperationResponseDto<List<ClienteDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            return _mapper.Map<OperationResponseDto<List<ClienteDto>>>(await _clienteRepository.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
        }
    }
}
