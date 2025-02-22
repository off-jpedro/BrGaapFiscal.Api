using BrGaapFiscal.Api.Data;
using BrGaapFiscal.Api.Models;
using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BrGaapFiscal.Api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository, ApplicationDbContext context)
        {
            _context = context;
            _clienteRepository = clienteRepository;
        }

        public async Task AddCliente(Cliente cliente)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _clienteRepository.AddCliente(cliente);

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteClienteById(int id)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cliente = await _clienteRepository.GetClienteById(id);
                    if (cliente == null)
                    {
                        throw new KeyNotFoundException("Cliente não encontrado(a).");
                    }

                    await _clienteRepository.DeleteClienteById(id);

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Cliente>> GetAllCliente()
        {
            return await _clienteRepository.GetAllCliente();
        }

        public async Task<Cliente> GetClienteById(int id)
        {
            var cliente = await _clienteRepository.GetClienteById(id);
            if (cliente == null)
            {
                throw new KeyNotFoundException("Cliente não encontrado(a).");
            }

            return cliente;
        }

        public async Task UpdateCliente(Cliente cliente)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existeCliente = await _clienteRepository.GetClienteById(cliente.Id);
                    if (existeCliente == null)
                    {
                        throw new KeyNotFoundException("Cliente não encontrado(a).");
                    }

                    existeCliente.Nome = cliente.Nome;

                    await _clienteRepository.UpdateCliente(existeCliente);
                 
                    await transaction.CommitAsync();
                }
                catch
                {
                  
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
