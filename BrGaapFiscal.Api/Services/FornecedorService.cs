using BrGaapFiscal.Api.Data;
using BrGaapFiscal.Api.Models;
using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BrGaapFiscal.Api.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFornecedorRepository _repository;

        public FornecedorService(ApplicationDbContext dbContext, IFornecedorRepository fornecedorRepository)
        {
            _context = dbContext;
            _repository = fornecedorRepository;
        }

        public async Task AddFornecedor(Fornecedor fornecedor)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _repository.AddFornecedor(fornecedor);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteFornecedorById(int id)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var fornecedor = await _repository.GetFornecedorById(id);
                    if (fornecedor == null)
                    {
                        throw new KeyNotFoundException("Fornecedor não encontrado(a).");
                    }

                    await _repository.DeleteFornecedorById(id);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Fornecedor>> GetAllFornecedor()
        {
            return await _repository.GetAllFornecedor();
        }

        public async Task<Fornecedor> GetFornecedorById(int id)
        {
            var fornecedor = await _repository.GetFornecedorById(id);
            if (fornecedor == null)
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }

            return fornecedor;
        }

        public async Task UpdateFornecedor(Fornecedor fornecedor)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existeFornecedor = await _repository.GetFornecedorById(fornecedor.Id);
                    if (existeFornecedor == null)
                    {
                        throw new KeyNotFoundException("Fornecedor não encontrado(a).");
                    }

                    existeFornecedor.Nome = fornecedor.Nome;

                    await _repository.UpdateFornecedor(existeFornecedor);
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
