using BrGaapFiscal.Api.Data;
using BrGaapFiscal.Api.Models;
using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BrGaapFiscal.Api.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _notaFiscalRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly ApplicationDbContext _context;

        public NotaFiscalService(
            INotaFiscalRepository notaFiscalRepository,
            IClienteRepository clienteRepository,
            IFornecedorRepository fornecedorRepository,
            ApplicationDbContext context)
        {
            _notaFiscalRepository = notaFiscalRepository;
            _clienteRepository = clienteRepository;
            _fornecedorRepository = fornecedorRepository;
            _context = context;
        }

        public async Task<IEnumerable<NotaFiscal>> GetAllNotaFiscal()
        {
            return await _notaFiscalRepository.GetAllNotaFiscal();
        }

        public async Task<NotaFiscal> GetNotaFiscalById(int id)
        {
            return await _notaFiscalRepository.GetNotaFiscalById(id);
        }

        public async Task AddNotaFiscal(NotaFiscal notaFiscal)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _clienteRepository.AddCliente(notaFiscal.Cliente);
                    await _fornecedorRepository.AddFornecedor(notaFiscal.Fornecedor);
                    await _notaFiscalRepository.AddNotaFiscal(notaFiscal);

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteNotaFiscalById(int id)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var notaFiscal = await _notaFiscalRepository.GetNotaFiscalById(id);
                    if (notaFiscal == null)
                    {
                        throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                    }

                    await _notaFiscalRepository.DeleteNotaFiscalById(id);

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdateNotaFiscal(NotaFiscal notaFiscal)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existeNotaFiscal = await _notaFiscalRepository.GetNotaFiscalById(notaFiscal.Id);
                    if (existeNotaFiscal == null)
                    {
                        throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                    }

                    existeNotaFiscal.Cliente = notaFiscal.Cliente;
                    existeNotaFiscal.Fornecedor = notaFiscal.Fornecedor;
                    existeNotaFiscal.NumeroNota = notaFiscal.NumeroNota;
                    existeNotaFiscal.ValorNota = notaFiscal.ValorNota;


                    await _notaFiscalRepository.UpdateNotaFiscal(existeNotaFiscal);

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
