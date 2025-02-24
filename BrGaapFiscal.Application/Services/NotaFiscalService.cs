﻿using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using System.Transactions;
using Microsoft.Extensions.Logging;
using BrGaapFiscal.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BrGaapFiscal.Api.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _notaFiscalRepository;
        private readonly IClienteService _clienteService;
        private readonly IFornecedorService _fornecedorService;
        private readonly ILogger<NotaFiscalService> _logger;

        public NotaFiscalService(
            INotaFiscalRepository notaFiscalRepository,
            IClienteService clienteService,
            IFornecedorService fornecedorService,
            ILogger<NotaFiscalService> logger)
        {
            _notaFiscalRepository = notaFiscalRepository;
            _clienteService = clienteService;
            _fornecedorService = fornecedorService;
            _logger = logger;
        }

        public async Task<IEnumerable<NotaFiscal>> GetAll()
        {
            try
            {
                return await _notaFiscalRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pesquisar as notas fiscais!");
                throw new BusinessException($"Erro ao pesquisar as notas fiscais! {ex.Message}");
            }
        }

        public async Task<NotaFiscal> GetById(long id)
        {
            try
            {
                var notaFiscal = await _notaFiscalRepository.GetById(id);
                if (notaFiscal == null || notaFiscal.Id <= 0)
                {
                    throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                }
                return notaFiscal;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Nota fiscal com ID: {id} não encontrada.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar a nota fiscal com ID: {id}");
                throw new BusinessException($"Erro ao buscar a nota fiscal com ID: {id}. {ex.Message}");
            }
        }

        public async Task<bool> Insert(NotaFiscal entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                if (entity.Cliente == null || entity.Cliente.Id <= 0 || string.IsNullOrEmpty(entity.Cliente.Nome))
                    throw new ArgumentNullException("Cliente inválido. Veja se está preenchendo os campos obrigatórios");

                if (entity.Fornecedor == null || entity.Fornecedor.Id <= 0 || string.IsNullOrEmpty(entity.Fornecedor.Nome))
                    throw new ArgumentNullException("Fornecedor inválido. Veja se está preenchendo os campos obrigatórios");

                var cliente = await _clienteService.GetById(entity.Cliente.Id);
                if (cliente == null || cliente.Id <= 0)
                {
                    await _clienteService.Insert(entity.Cliente);
                }
                else
                {
                    entity.Cliente = cliente;
                }

                var fornecedor = await _fornecedorService.GetById(entity.Fornecedor.Id);
                if (fornecedor == null || fornecedor.Id <= 0)
                {
                    await _fornecedorService.Insert(entity.Fornecedor);
                }
                else
                {
                    entity.Fornecedor = fornecedor;
                }

                var result = await _notaFiscalRepository.Add(entity);
                if (!result)
                {
                    throw new BusinessException("Falha ao inserir a Nota Fiscal");
                }

                return true;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Erro ao inserir a nota fiscal: argumento nulo.");
                throw;
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Erro ao inserir a nota fiscal: falha na lógica de negócios.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir a nota fiscal.");
                throw new BusinessException($"Erro ao inserir a nota fiscal. {ex.Message}");
            }
        }

        public async Task<bool> Update(NotaFiscal entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                    throw new ArgumentNullException(nameof(entity));

                var existeNotaFiscal = await _notaFiscalRepository.GetById(entity.Id);
                if (existeNotaFiscal == null || existeNotaFiscal.Id <= 0)
                {
                    throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                }

                var result = await _notaFiscalRepository.Update(existeNotaFiscal);
                if (!result)
                {
                    throw new BusinessException("Falha ao atualizar a Nota Fiscal");
                }

                return true;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Nota fiscal com ID: {entity.Id} não encontrada.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar a nota fiscal com ID: {entity.Id}");
                throw;
            }
        }

        public async Task<bool> Delete(NotaFiscal entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                    throw new ArgumentNullException("id", "ID da Nota fiscal não informado.");

                var notaFiscal = await _notaFiscalRepository.GetById(entity.Id);
                if (notaFiscal == null || notaFiscal.Id <= 0)
                {
                    throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                }

                return await _notaFiscalRepository.Remove(entity);
            }
            catch (ArgumentNullException aex)
            {
                _logger.LogWarning($"Nota fiscal com ID: {entity.Id} não informado.");
                throw;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Nota fiscal com ID: {entity.Id} não encontrada.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar a nota fiscal com ID: {entity.Id}");
                throw new BusinessException($"Erro ao deletar a nota fiscal com ID: {entity.Id}. {ex.Message}");
            }
        }
    }
}
