using Application.Dtos;
using Application.Service.Crud;
using Application.Service.Interface;
using Application.Validators;
using AutoMapper;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Service
{
    public class ConcessionariaService : CrudService<Concessionaria, ConcessionariaDto>, IConcessionariaService
    {
        private readonly IConcessionariaRepository _concessionariaRepository;
        public ConcessionariaService(IMapper mapper,
                                     IConcessionariaRepository repositorio,
                                     IHttpContextAccessor accessor) : base(mapper, repositorio, accessor)
        {
            _concessionariaRepository = repositorio;
        }

        public override async Task<OperationResponseDto<ConcessionariaDto>> Salvar(ConcessionariaDto dto)
        {
            var validacao = ConcessionariaValidator.Validar(dto);
            if (!validacao.isValido)
            {
                return new OperationResponseDto<ConcessionariaDto>
                {
                    Success = false,
                    Object = dto,
                    Message = validacao.Mensagem,
                    Exception = null
                };
            }

            if (dto.Id == 0 || dto.Id == null)
            {
                var jaCadastrado = (await Consultar(x => x.Nome == dto.Nome && x.Bairro == dto.Bairro && x.Cidade == dto.Cidade && x.Uf == dto.Uf, null)).Object;
                if (jaCadastrado != null && jaCadastrado.Count > 0)
                {
                    if (jaCadastrado.FirstOrDefault()!.Deletado)
                    {
                        jaCadastrado.FirstOrDefault()!.Deletado = false;
                        jaCadastrado.FirstOrDefault()!.Nome = dto.Nome;
                        jaCadastrado.FirstOrDefault()!.Cep = dto.Cep;
                        jaCadastrado.FirstOrDefault()!.Logradouro = dto.Logradouro;
                        jaCadastrado.FirstOrDefault()!.Numero = dto.Numero;
                        jaCadastrado.FirstOrDefault()!.Bairro = dto.Bairro;
                        jaCadastrado.FirstOrDefault()!.Cidade = dto.Cidade;
                        jaCadastrado.FirstOrDefault()!.Uf = dto.Uf;
                        jaCadastrado.FirstOrDefault()!.Telefone = dto.Telefone;
                        jaCadastrado.FirstOrDefault()!.Email = dto.Email;
                        jaCadastrado.FirstOrDefault()!.CapacidadeMaxima = dto.CapacidadeMaxima;

                        return _mapper.Map<OperationResponseDto<ConcessionariaDto>>(await _repositorio.Alterar(_mapper.Map<Concessionaria>(jaCadastrado.FirstOrDefault()!)));
                    }
                    else
                    {
                        return new OperationResponseDto<ConcessionariaDto>
                        {
                            Success = false,
                            Object = dto,
                            Message = "Concessionária já cadastrada.",
                            Exception = null
                        };
                    }
                }

                return _mapper.Map<OperationResponseDto<ConcessionariaDto>>(await _repositorio.Inserir(_mapper.Map<Concessionaria>(dto)));
            }
            else
            {
                return _mapper.Map<OperationResponseDto<ConcessionariaDto>>(await _repositorio.Alterar(_mapper.Map<Concessionaria>(dto)));
            }
        }

        public async Task<OperationResponseDto<List<ConcessionariaDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            return _mapper.Map<OperationResponseDto<List<ConcessionariaDto>>>(await _concessionariaRepository.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
        }
    }
}
