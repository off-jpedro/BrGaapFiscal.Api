using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Application.Exceptions;
using BrGaapFiscal.Domain.Models;
using Microsoft.Extensions.Logging;

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

        public async Task<IEnumerable<Fornecedor>> GetAll()
        {
            try
            {
                return await _repository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pesquisar os fornecedores.");
                throw new BusinessException("Erro ao pesquisar os fornecedores.");
            }
        }

        public async Task<Fornecedor> GetById(long id)
        {
            try
            {
                var fornecedor = await _repository.GetById(id);
                if (fornecedor == null || fornecedor.Id <= 0)
                {
                    _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", id);
                    throw new KeyNotFoundException("Fornecedor não encontrado.");
                }

                return fornecedor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o fornecedor com ID: {FornecedorId}", id);
                throw new BusinessException($"Erro ao buscar o fornecedor com ID: {id}.");
            }
        }

        public async Task<bool> Insert(Fornecedor entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                var fornecedorExistente = await _repository.GetById(entity.Id);
                if (fornecedorExistente != null)
                {
                    _logger.LogWarning("Fornecedor já existe com o ID: {FornecedorId}", entity.Id);
                    throw new ArgumentException("Fornecedor já existe.");
                }

                var result = await _repository.Add(entity);
                if (!result)
                {
                    throw new BusinessException("Falha ao inserir o fornecedor.");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir o fornecedor.");
                throw;
            }
        }

        public async Task<bool> Update(Fornecedor entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                    throw new ArgumentNullException(nameof(entity));

                var fornecedorExistente = await _repository.GetById(entity.Id);
                if (fornecedorExistente == null)
                {
                    _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", entity.Id);
                    throw new KeyNotFoundException("Fornecedor não encontrado.");
                }

                fornecedorExistente.Nome = entity.Nome;

                var result = await _repository.Update(fornecedorExistente);
                if (!result)
                {
                    throw new BusinessException("Falha ao atualizar o fornecedor.");
                }

                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o fornecedor com ID: {FornecedorId}", entity.Id);
                throw;
            }
        }

        public async Task<bool> Delete(Fornecedor entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                    throw new ArgumentNullException(nameof(entity));

                var fornecedor = await _repository.GetById(entity.Id);
                if (fornecedor == null)
                {
                    _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", entity.Id);
                    throw new KeyNotFoundException("Fornecedor não encontrado.");
                }

                return await _repository.Remove(entity);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar o fornecedor com ID: {FornecedorId}", entity.Id);
                throw new BusinessException($"Erro ao deletar o fornecedor com ID: {entity.Id}.");
            }
        }
    }
}
