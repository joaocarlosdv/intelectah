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
    public class VendasService : CrudService<Vendas, VendasDto>, IVendasService
    {
        private readonly IVeiculoService _veiculoService;
        private readonly IConcessionariaService _concessionariaService;
        private readonly IClienteService _clienteService;
        private readonly IVendasRepository _vendasRepository;
        public VendasService(IMapper mapper,
                             IVendasRepository repositorio,
                             IVeiculoService veiculoService,
                             IConcessionariaService concessionariaService,
                             IClienteService clienteService,
                             IHttpContextAccessor accessor) : base(mapper, repositorio, accessor)
        {
            _veiculoService = veiculoService;
            _concessionariaService = concessionariaService;
            _clienteService = clienteService;
            _vendasRepository = repositorio;
        }
        public override async Task<OperationResponseDto<VendasDto>> Salvar(VendasDto dto)
        {
            dto.Veiculo = (await _veiculoService.Consultar(x => x.Id == dto.VeiculoId, null)).Object!.FirstOrDefault();
            dto.Concessionaria = (await _concessionariaService.Consultar(x => x.Id == dto.ConcessionariaId, null)).Object!.FirstOrDefault();
            dto.Cliente = (await _clienteService.Consultar(x => x.Id == dto.ClienteId, null)).Object!.FirstOrDefault();

            var validacao = VendasValidator.Validar(dto);
            if (!validacao.isValido)
            {
                return new OperationResponseDto<VendasDto>
                {
                    Success = false,
                    Object = dto,
                    Message = validacao.Mensagem,
                    Exception = null
                };
            }

            dto.Veiculo = null;
            dto.Concessionaria = null;
            dto.Cliente = null;

            if (dto.Id == 0 || dto.Id == null)
            {
                dto.ProtocoloVenda = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                    .Replace("=", "") 
                    .Replace("+", "")
                    .Replace("/", "")
                    .Substring(0, 20);

                var jaCadastrado = (await Consultar(x => x.ProtocoloVenda == dto.ProtocoloVenda, null)).Object;
                if (jaCadastrado != null && jaCadastrado.Count > 0)
                {
                    if (jaCadastrado.FirstOrDefault()!.Deletado)
                    {
                        jaCadastrado.FirstOrDefault()!.Deletado = false;
                        jaCadastrado.FirstOrDefault()!.VeiculoId = dto.VeiculoId;
                        jaCadastrado.FirstOrDefault()!.ConcessionariaId = dto.ConcessionariaId;
                        jaCadastrado.FirstOrDefault()!.ClienteId = dto.ClienteId;
                        jaCadastrado.FirstOrDefault()!.DataVenda = dto.DataVenda;
                        jaCadastrado.FirstOrDefault()!.PrecoVenda = dto.PrecoVenda;

                        return _mapper.Map<OperationResponseDto<VendasDto>>(await _repositorio.Alterar(_mapper.Map<Vendas>(jaCadastrado.FirstOrDefault()!)));
                    }
                    else
                    {
                        return new OperationResponseDto<VendasDto>
                        {
                            Success = false,
                            Object = dto,
                            Message = "Venda já cadastrada.",
                            Exception = null
                        };
                    }
                }                

                return _mapper.Map<OperationResponseDto<VendasDto>>(await _repositorio.Inserir(_mapper.Map<Vendas>(dto)));
            }
            else
            {
                return _mapper.Map<OperationResponseDto<VendasDto>>(await _repositorio.Alterar(_mapper.Map<Vendas>(dto)));
            }
        }
        public async Task<OperationResponseDto<List<VendasDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            return _mapper.Map<OperationResponseDto<List<VendasDto>>>(await _vendasRepository.ConsultaPaginada(limit, offset, search, colOrder, dirOrder));
        }
    }
}
