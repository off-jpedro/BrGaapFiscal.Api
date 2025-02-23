using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Factory;
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

        public async Task<bool> Add(Fornecedor entity)
        {
            await _context.Fornecedores.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Fornecedor entity)
        {
            _context.Fornecedores.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Fornecedor entity)
        {
            var existingFornecedor = await _context.Fornecedores.FindAsync(entity.Id);
            if (existingFornecedor != null)
            {
                _context.Entry(existingFornecedor).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }
        }

        public async Task<IEnumerable<Fornecedor>> GetAll()
        {
            return await _context.Fornecedores.ToListAsync();
        }

        public async Task<Fornecedor> GetById(long id)
        {
            return await _context.Fornecedores.FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}