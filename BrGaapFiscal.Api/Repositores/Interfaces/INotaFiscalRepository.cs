using BrGaapFiscal.Api.Models;

namespace BrGaapFiscal.Api.Repositores.Interfaces
{
    public interface INotaFiscalRepository
    {

        Task<IEnumerable<NotaFiscal>> GetAllNotaFiscal();
        Task<NotaFiscal> GetNotaFiscalById(int id);
        Task AddNotaFiscal(NotaFiscal notaFiscal);

        Task UpdateNotaFiscal(NotaFiscal notaFiscal);

        Task DeleteNotaFiscalById(int id);

    }
}
