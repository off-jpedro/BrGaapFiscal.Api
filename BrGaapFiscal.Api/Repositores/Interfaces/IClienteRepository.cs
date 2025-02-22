using BrGaapFiscal.Api.Models;

namespace BrGaapFiscal.Api.Repositores.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllCliente();
        Task<Cliente> GetClienteById(int id);
        Task AddCliente(Cliente cliente);

        Task UpdateCliente(Cliente cliente);

        Task DeleteClienteById(int id);
    }
}
