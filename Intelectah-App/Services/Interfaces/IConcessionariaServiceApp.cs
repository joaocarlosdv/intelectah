using ApiExterna.Models;
using Application.Dtos;
using Intelectah_App.Services.Crud;

namespace Intelectah_App.Services.Interfaces
{
    public interface IConcessionariaServiceApp : ICrudServiceApp<ConcessionariaDto>
    {
        Task<Cep> ConsultarCep(string cep);
    }
}
