using ApiExterna.Models;

namespace ApiExterna.Services
{
    public interface ICorreiosService
    {
        Task<Cep> ConsultarEndereco(string cep);
    }
}
