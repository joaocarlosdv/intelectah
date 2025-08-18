using Application.Dtos;
using AutoMapper;
using Domain.ModelResponse;
using Domain.Models;

namespace CrossCutting.Mapper
{
    public class ApiMapper : Profile
    {
        public ApiMapper()
        {   
            CreateMap(typeof(OperationResponse<>), typeof(OperationResponseDto<>))
            .ForMember("Object", opt => opt.MapFrom("Object"))
            .ReverseMap();

            CreateMap<Fabricante, FabricanteDto>().ReverseMap();
            CreateMap<Veiculo, VeiculoDto>().ReverseMap();
            CreateMap<Concessionaria, ConcessionariaDto>().ReverseMap();
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Vendas, VendasDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
        }
    }
}
