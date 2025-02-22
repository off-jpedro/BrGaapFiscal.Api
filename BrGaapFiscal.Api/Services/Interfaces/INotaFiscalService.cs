using BrGaapFiscal.Api.Models;

namespace BrGaapFiscal.Api.Services.Interfaces
{
    public interface INotaFiscalService
    {
        Task AddNotaFiscal(NotaFiscal notaFiscal);
        Task<NotaFiscal> GetNotaFiscalById(int id);
        Task<IEnumerable<NotaFiscal>> GetAllNotaFiscal();
        Task UpdateNotaFiscal(NotaFiscal notaFiscal);
        Task DeleteNotaFiscalById(int id);
    }
}
