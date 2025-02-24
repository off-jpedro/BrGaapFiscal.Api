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
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Remove(NotaFiscal entity)
        {
            _context.NotaFiscais.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Update(NotaFiscal entity)
        {
            var notaFiscal = await _context.NotaFiscais.FindAsync(entity.Id);

            _context.Entry(notaFiscal).CurrentValues.SetValues(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
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