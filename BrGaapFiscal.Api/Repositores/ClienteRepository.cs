using BrGaapFiscal.Api.Data;
using BrGaapFiscal.Api.Models;
using BrGaapFiscal.Api.Repositores.Interfaces;
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

        public async Task AddCliente(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClienteById(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Cliente não encontrado(a).");
            }
        }

        public async Task<IEnumerable<Cliente>> GetAllCliente()
        {
            return await _context.Clientes
                .ToListAsync();
        }

        public async Task<Cliente> GetClienteById(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            var existingCliente = await _context.Clientes.FindAsync(cliente.Id);
            if (existingCliente != null)
            {
                _context.Entry(existingCliente).CurrentValues.SetValues(cliente);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Cliente não encontrado(a).");
            }
        }
    }
}
