using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Factory;
using Microsoft.EntityFrameworkCore;

namespace BrGaapFiscal.Api.Repositores
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private readonly ApplicationDbContext _context;

        public NotaFiscalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(NotaFiscal entity)
        {
            await _context.NotaFiscais.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(NotaFiscal entity)
        {
            _context.NotaFiscais.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(NotaFiscal entity)
        {
            var existingNotaFiscal = await _context.NotaFiscais.FindAsync(entity.Id);
            if (existingNotaFiscal != null)
            {
                _context.Entry(existingNotaFiscal).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new KeyNotFoundException("Nota fiscal não encontrada.");
            }
        }

        public async Task<IEnumerable<NotaFiscal>> GetAll()
        {
            return await _context.NotaFiscais
                .Include(n => n.Cliente)
                .Include(n => n.Fornecedor)
                .ToListAsync();
        }

        public async Task<NotaFiscal> GetById(long id)
        {
            return await _context.NotaFiscais
                .Include(n => n.Cliente)
                .Include(n => n.Fornecedor)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}