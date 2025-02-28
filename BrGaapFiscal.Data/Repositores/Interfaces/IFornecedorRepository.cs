using BrGaapFiscal.Domain.Interfaces;
using BrGaapFiscal.Domain.Models;

namespace BrGaapFiscal.Api.Repositores.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        Task<int> GetMaxId();

    }
}
