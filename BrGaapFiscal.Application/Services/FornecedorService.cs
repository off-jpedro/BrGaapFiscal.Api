using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Application.Exceptions;
using BrGaapFiscal.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace BrGaapFiscal.Api.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _repository;
        private readonly ILogger<FornecedorService> _logger;

        public FornecedorService(IFornecedorRepository fornecedorRepository, ILogger<FornecedorService> logger)
        {
            _repository = fornecedorRepository;
            _logger = logger;
        }

        public async Task<bool> Insert(Fornecedor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var fornecedorExistente = await _repository.GetById(entity.Id).ConfigureAwait(false);
                    if (fornecedorExistente != null)
                    {
                        _logger.LogWarning("Fornecedor já existe com o ID: {FornecedorId}", entity.Id);
                        throw new ArgumentException("Fornecedor já existe.");
                    }

                    var result = await _repository.Add(entity).ConfigureAwait(false);
                    if (result)
                    {
                        transaction.Complete();
                        return true;
                    }
                    else
                    {
                        throw new BusinessException("Falha ao inserir o fornecedor.");
                    }
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "Erro ao inserir o fornecedor: Fornecedor já existe.");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao inserir o fornecedor.");
                    throw new BusinessException($"Erro ao inserir o fornecedor. {ex.Message}");
                }
            }
        }

        public async Task<bool> Delete(Fornecedor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                var fornecedor = await _repository.GetById(entity.Id).ConfigureAwait(false);
                if (fornecedor == null)
                {
                    _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", entity.Id);
                    throw new KeyNotFoundException("Fornecedor não encontrado(a).");
                }

                return await _repository.Remove(entity).ConfigureAwait(false);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Erro ao deletar o fornecedor: Fornecedor não encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar o fornecedor.");
                throw new BusinessException($"Erro ao deletar o fornecedor. {ex.Message}");
            }
        }

        public async Task<IEnumerable<Fornecedor>> GetAll()
        {
            try
            {
                var fornecedores = await _repository.GetAll().ConfigureAwait(false);
                if (fornecedores == null || !fornecedores.Any())
                {
                    _logger.LogWarning("Nenhum fornecedor encontrado.");
                    throw new BusinessException("Nenhum fornecedor encontrado.");
                }
                return fornecedores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar fornecedores.");
                throw new BusinessException($"Erro ao buscar fornecedores. {ex.Message}");
            }
        }

        public async Task<Fornecedor> GetById(long id)
        {
            try
            {
                var fornecedor = await _repository.GetById(id).ConfigureAwait(false);
                if (fornecedor == null)
                {
                    _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", id);
                    throw new KeyNotFoundException("Fornecedor não encontrado(a).");
                }

                return fornecedor;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Erro ao buscar o fornecedor: Fornecedor não encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o fornecedor.");
                throw new BusinessException($"Erro ao buscar o fornecedor. {ex.Message}");
            }
        }

        public async Task<bool> Update(Fornecedor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existeFornecedor = await _repository.GetById(entity.Id).ConfigureAwait(false);
                    if (existeFornecedor == null)
                    {
                        _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", entity.Id);
                        throw new KeyNotFoundException("Fornecedor não encontrado(a).");
                    }

                    existeFornecedor.Nome = entity.Nome;

                    var result = await _repository.Update(existeFornecedor).ConfigureAwait(false);
                    if (result)
                    {
                        transaction.Complete();
                        return true;
                    }
                    else
                    {
                        throw new BusinessException("Falha ao atualizar o fornecedor.");
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar o fornecedor: Fornecedor não encontrado.");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar o fornecedor.");
                    throw new BusinessException($"Erro ao atualizar o fornecedor. {ex.Message}");
                }
            }
        }
    }
}