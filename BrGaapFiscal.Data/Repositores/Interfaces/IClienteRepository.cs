using BrGaapFiscal.Domain.Interfaces;
using BrGaapFiscal.Domain.Models;
using System.Threading.Tasks;

namespace BrGaapFiscal.Infra.Data.Repositores.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<int> GetMaxId();
    }
}
