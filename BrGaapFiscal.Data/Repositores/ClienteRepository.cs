using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Factory;
using BrGaapFiscal.Infra.Data.Repositores.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrGaapFiscal.Api.Repositores
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Cliente entity)
        {
            await _context.Clientes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Cliente entity)
        {
            _context.Clientes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Cliente entity)
        {
            var existingCliente = await _context.Clientes.FindAsync(entity.Id);
            if (existingCliente != null)
            {
                _context.Entry(existingCliente).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new KeyNotFoundException("Cliente não encontrado(a).");
            }
        }

        public async Task<IEnumerable<Cliente>> GetAll()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetById(long id)
        {
            return await _context.Clientes.FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}