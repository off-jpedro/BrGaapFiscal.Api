using BrGaapFiscal.Domain.Models;

namespace BrGaapFiscal.Api.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAll(int pageNumber, int pageSize);
        Task<Cliente> GetById(long id);
        Task<bool> Insert(Cliente entity);
        Task<bool> Delete(Cliente entity);
        Task<bool> Update(Cliente entity);
    }
}
