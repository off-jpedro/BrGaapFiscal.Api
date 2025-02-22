using BrGaapFiscal.Api.Data;
using BrGaapFiscal.Api.Models;
using BrGaapFiscal.Api.Repositores.Interfaces;
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

        public async Task AddNotaFiscal(NotaFiscal notaFiscal)
        {
            await _context.NotaFiscais.AddAsync(notaFiscal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotaFiscalById(int id)
        {
            var notaFiscal = await _context.NotaFiscais.FindAsync(id);
            if (notaFiscal != null)
            {
                _context.NotaFiscais.Remove(notaFiscal);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Nota fiscal não encontrada.");
            }
        }

        public async Task<IEnumerable<NotaFiscal>> GetAllNotaFiscal()
        {
            return await _context.NotaFiscais
                .Include(n => n.Cliente)
                .Include(n => n.Fornecedor)
                .ToListAsync();
        }

        public async Task<NotaFiscal> GetNotaFiscalById(int id)
        {
            return await _context.NotaFiscais
                .Include(n => n.Cliente)
                .Include(n => n.Fornecedor)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task UpdateNotaFiscal(NotaFiscal notaFiscal)
        {
            var existingNotaFiscal = await _context.NotaFiscais.FindAsync(notaFiscal.Id);
            if (existingNotaFiscal != null)
            {
                _context.Entry(existingNotaFiscal).CurrentValues.SetValues(notaFiscal);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Nota fiscal não encontrada.");
            }
        }
    }
}
