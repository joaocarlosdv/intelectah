using ApiExterna.Models;
using Application.Dtos;
using CronosPlus_App.Services.Crud;

namespace CronosPlus_App.Services.Interfaces
{
    public interface IConcessionariaServiceApp : ICrudServiceApp<ConcessionariaDto>
    {
        Task<Cep> ConsultarCep(string cep);
    }
}
