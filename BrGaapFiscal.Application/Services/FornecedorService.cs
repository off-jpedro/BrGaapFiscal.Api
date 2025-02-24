using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Application.Exceptions;
using BrGaapFiscal.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

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
                _logger.LogError(ex, "Erro ao pesquisar os fornecedores!");
                throw new BusinessException($"Erro ao pesquisar os fornecedores! {ex.Message}");
            }
        }

        public async Task<Fornecedor> GetById(long id)
        {
            try
            {
                var fornecedor = await _repository.GetById(id);
                if (fornecedor == null || fornecedor.Id <= 0)
                {
                    throw new KeyNotFoundException("Fornecedor não encontrado.");
                }
                return fornecedor;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Fornecedor com ID: {id} não encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o Fornecedor com ID: {id}");
                throw new BusinessException($"Erro ao buscar o Fornecedor com ID: {id}. {ex.Message}");
            }
        }

        public async Task<bool> Insert(Fornecedor entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                var result = await _repository.Add(entity);
                if (!result)
                {
                    throw new BusinessException("Falha ao inserir o Fornecedor.");
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

                var existeFornecedor = await _repository.GetById(entity.Id);
                if (existeFornecedor == null || existeFornecedor.Id <= 0)
                {
                    _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", entity.Id);
                    throw new KeyNotFoundException("Fornecedor não encontrado.");
                }

                existeFornecedor.Nome = entity.Nome;

                var result = await _repository.Update(existeFornecedor);
                if (!result)
                {
                    throw new BusinessException("Falha ao atualizar o Fornecedor.");
                }

                return true;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Fornecedor com ID: {entity.Id} não encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o Fornecedor com ID: {entity.Id}");
                throw;
            }
        }

        public async Task<bool> Delete(Fornecedor entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                    throw new ArgumentNullException("id", "ID do Fornecedor não informado.");

                var fornecedor = await _repository.GetById(entity.Id);
                if (fornecedor == null || fornecedor.Id <= 0)
                {
                    _logger.LogWarning("Fornecedor não encontrado com o ID: {FornecedorId}", entity.Id);
                    throw new KeyNotFoundException("Fornecedor não encontrado.");
                }

                return await _repository.Remove(entity);
            }
            catch (ArgumentNullException aex)
            {
                _logger.LogWarning($"Fornecedor com ID: {entity.Id} não informado.");
                throw;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Fornecedor com ID: {entity.Id} não encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar o Fornecedor com ID: {entity.Id}");
                throw new BusinessException($"Erro ao deletar o Fornecedor com ID: {entity.Id}. {ex.Message}");
            }
        }
    }
}