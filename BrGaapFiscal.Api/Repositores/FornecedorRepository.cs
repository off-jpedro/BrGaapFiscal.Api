using BrGaapFiscal.Api.Data;
using BrGaapFiscal.Api.Models;
using BrGaapFiscal.Api.Repositores.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrGaapFiscal.Api.Repositores
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly ApplicationDbContext _context;

        public FornecedorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFornecedor(Fornecedor fornecedor)
        {
            await _context.Fornecedores.AddAsync(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFornecedorById(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                _context.Fornecedores.Remove(fornecedor);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }
        }

        public async Task<IEnumerable<Fornecedor>> GetAllFornecedor()
        {
            return await _context.Fornecedores
                .ToListAsync();
        }

        public async Task<Fornecedor> GetFornecedorById(int id)
        {
            return await _context.Fornecedores
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task UpdateFornecedor(Fornecedor fornecedor)
        {
            var existingFornecedor = await _context.Fornecedores.FindAsync(fornecedor.Id);
            if (existingFornecedor != null)
            {
                _context.Entry(existingFornecedor).CurrentValues.SetValues(fornecedor);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }
        }
    }
}
