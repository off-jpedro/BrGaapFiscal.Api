using BrGaapFiscal.Domain.Interfaces;
using BrGaapFiscal.Domain.Models;

namespace BrGaapFiscal.Api.Repositores.Interfaces
{
    public interface INotaFiscalRepository : IRepository<NotaFiscal>
    {
        Task<int> GetMaxId();
        Task<int> GetMaxNumeroNota();

    }
}
