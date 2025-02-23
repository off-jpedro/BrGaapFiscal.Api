using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using System.Transactions;

namespace BrGaapFiscal.Api.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _repository;

        public FornecedorService(IFornecedorRepository fornecedorRepository)
        {
            _repository = fornecedorRepository;
        }

        public async Task<bool> Insert(Fornecedor entity)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var fornecedorExistente = await _repository.GetById(entity.Id);
                    if (fornecedorExistente != null)
                    {
                        throw new ArgumentException("Fornecedor já existe.");
                    }

                    var result = await _repository.Add(entity);
                    transaction.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new KeyNotFoundException($"Erro ao inserir o fornecedor. {ex.Message}");
                }
            }
        }

        public async Task<bool> Delete(Fornecedor entity)
        {
            var fornecedor = await _repository.GetById(entity.Id);
            if (fornecedor == null)
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }

            return await _repository.Remove(entity);
        }

        public async Task<IEnumerable<Fornecedor>> GetAll()
        {
            try
            {
                var fornecedores = await _repository.GetAll();
                if (fornecedores == null || !fornecedores.Any())
                {
                    throw new KeyNotFoundException("Nenhum fornecedor encontrado.");
                }
                return fornecedores;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Erro ao buscar fornecedores. {ex.Message}");
            }
        }

        public async Task<Fornecedor> GetById(long id)
        {
            var fornecedor = await _repository.GetById(id);
            if (fornecedor == null)
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }

            return fornecedor;
        }

        public async Task<bool> Update(Fornecedor entity)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existeFornecedor = await _repository.GetById(entity.Id);
                    if (existeFornecedor == null)
                    {
                        throw new KeyNotFoundException("Fornecedor não encontrado(a).");
                    }

                    existeFornecedor.Nome = entity.Nome;

                    var result = await _repository.Update(existeFornecedor);
                    transaction.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new KeyNotFoundException($"Erro ao atualizar o fornecedor. {ex.Message}");
                }
            }
        }
    }
}