using BrGaapFiscal.Domain.Models;

namespace BrGaapFiscal.Api.Services.Interfaces
{
    public interface IFornecedorService
    {
        Task<IEnumerable<Fornecedor>> GetAll(int pageNumber, int pageSize);
        Task<Fornecedor> GetById(long id);
        Task<bool> Insert(Fornecedor entity);
        Task<bool> Delete(Fornecedor entity);
        Task<bool> Update(Fornecedor entity);
    }
}
