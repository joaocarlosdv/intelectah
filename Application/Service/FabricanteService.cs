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
    public class FabricanteService : CrudService<Fabricante, FabricanteDto>, IFabricanteService
    {
        private readonly IFabricanteRepository fabricanteRepository;
        public FabricanteService(IMapper mapper,
                                 IFabricanteRepository repositorio,
                                 IHttpContextAccessor accessor) : base(mapper, repositorio, accessor)
        {
            fabricanteRepository = repositorio;
        }

        public override async Task<OperationResponseDto<FabricanteDto>> Salvar(FabricanteDto dto)
        {
            var validacao = FabricanteValidator.Validar(dto);
            if (!validacao.isValido)
            {
                return new OperationResponseDto<FabricanteDto>
                {
                    Success = false,
                    Object = dto,
                    Message = validacao.Mensagem,
                    Exception = null
                };
            }

            if (dto.Id == 0 || dto.Id == null)
            {
                var jaCadastrado = (await Consultar(x => x.Nome == dto.Nome, null)).Object;
                if (jaCadastrado != null && jaCadastrado.Count > 0)
                {
                    if (jaCadastrado.FirstOrDefault()!.Deletado)
                    {
                        jaCadastrado.FirstOrDefault()!.Deletado = false;
                        jaCadastrado.FirstOrDefault()!.Nome = dto.Nome;
                        jaCadastrado.FirstOrDefault()!.PaisOrigem = dto.PaisOrigem;
                        jaCadastrado.FirstOrDefault()!.WebSite = dto.WebSite;
                        jaCadastrado.FirstOrDefault()!.AnoFundacao = dto.AnoFundacao;

                        return _mapper.Map<OperationResponseDto<FabricanteDto>>(await _repositorio.Alterar(_mapper.Map<Fabricante>(jaCadastrado.FirstOrDefault()!)));
                    }
                    else
                    {
                        return new OperationResponseDto<FabricanteDto>
                        {
                            Success = false,
                            Object = dto,
                            Message = "Fabricante já cadastrado com este Nome.",
                            Exception = null
                        };
                    }
                }

                return _mapper.Map<OperationResponseDto<FabricanteDto>>(await _repositorio.Inserir(_mapper.Map<Fabricante>(dto)));
            }
            else
            {
                return _mapper.Map<OperationResponseDto<FabricanteDto>>(await _repositorio.Alterar(_mapper.Map<Fabricante>(dto)));
            }
        }
        public async Task<OperationResponseDto<List<FabricanteDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            return _mapper.Map<OperationResponseDto<List<FabricanteDto>>>(await fabricanteRepository.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
        }
        public async Task<OperationResponseDto<int>> ConsultaPaginadaCount(string? search, int colOrder, string dirOrder)
        {
            return _mapper.Map<OperationResponseDto<int>>(await fabricanteRepository.ConsultaPaginadaCount( search, colOrder, dirOrder));
        }
    }
}
