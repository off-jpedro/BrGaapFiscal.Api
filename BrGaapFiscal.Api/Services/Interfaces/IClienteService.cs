using BrGaapFiscal.Api.Models;

namespace BrGaapFiscal.Api.Services.Interfaces
{
    public interface IClienteService
    {
        Task AddCliente(Cliente cliente);
        Task<Cliente> GetClienteById(int id);
        Task<IEnumerable<Cliente>> GetAllCliente();
        Task UpdateCliente(Cliente cliente);
        Task DeleteClienteById(int id);
    }
}
