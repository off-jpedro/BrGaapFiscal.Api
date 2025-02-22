using BrGaapFiscal.Api.Models;

namespace BrGaapFiscal.Api.Repositores.Interfaces
{
    public interface IFornecedorRepository
    {
        Task<IEnumerable<Fornecedor>> GetAllFornecedor();
        Task<Fornecedor> GetFornecedorById(int id);
        Task AddFornecedor(Fornecedor fornecedor);

        Task UpdateFornecedor(Fornecedor fornecedor);

        Task DeleteFornecedorById(int id);
    }
}
