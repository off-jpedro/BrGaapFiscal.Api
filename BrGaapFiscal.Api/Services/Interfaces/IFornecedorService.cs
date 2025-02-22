using BrGaapFiscal.Api.Models;

namespace BrGaapFiscal.Api.Services.Interfaces
{
    public interface IFornecedorService
    {
        Task AddFornecedor(Fornecedor fornecedor);
        Task<Fornecedor> GetFornecedorById(int id);
        Task<IEnumerable<Fornecedor>> GetAllFornecedor();
        Task UpdateFornecedor(Fornecedor fornecedor);
        Task DeleteFornecedorById(int id);
    }
}
