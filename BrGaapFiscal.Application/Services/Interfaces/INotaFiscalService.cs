using BrGaapFiscal.Domain.Models;

namespace BrGaapFiscal.Api.Services.Interfaces
{
    public interface INotaFiscalService
    {
        Task<IEnumerable<NotaFiscal>> GetAll(int pageNumber, int pageSize);
        Task<NotaFiscal> GetById(long id);
        Task<bool> Insert(NotaFiscal entity);
        Task<bool> Delete(NotaFiscal entity);
        Task<bool> Update(NotaFiscal entity);
    }
}
