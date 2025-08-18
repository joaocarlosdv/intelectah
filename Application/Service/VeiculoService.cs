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
    public class VeiculoService : CrudService<Veiculo, VeiculoDto>, IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepositorio;
        public VeiculoService(IMapper mapper,
                              IVeiculoRepository repositorio,
                              IHttpContextAccessor accessor) : base(mapper, repositorio, accessor)
        {
            _veiculoRepositorio = repositorio;
        }

        public override async Task<OperationResponseDto<VeiculoDto>> Salvar(VeiculoDto dto)
        {
            var validacao = VeiculoValidator.Validar(dto);
            if (!validacao.isValido)
            {
                return new OperationResponseDto<VeiculoDto>
                {
                    Success = false,
                    Object = dto,
                    Message = validacao.Mensagem,
                    Exception = null
                };
            }

            if (dto.Id == 0 || dto.Id == null)
            {
                return _mapper.Map<OperationResponseDto<VeiculoDto>>(await _repositorio.Inserir(_mapper.Map<Veiculo>(dto)));
            }
            else
            {
                return _mapper.Map<OperationResponseDto<VeiculoDto>>(await _repositorio.Alterar(_mapper.Map<Veiculo>(dto)));
            }
        }

        public async Task<OperationResponseDto<List<VeiculoDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            return _mapper.Map<OperationResponseDto<List<VeiculoDto>>>(await _veiculoRepositorio.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
        }
    }
}
