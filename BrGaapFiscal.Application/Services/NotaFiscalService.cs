﻿using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using System.Transactions;
using Microsoft.Extensions.Logging;
using BrGaapFiscal.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<NotaFiscal>> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber <= 0) pageNumber = 1;
                if (pageSize <= 0) pageSize = 10;

                return await _notaFiscalRepository.GetAll(pageNumber, pageSize);
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
                if (id <= 0)
                {
                    throw new ArgumentNullException("ID não informado.");
                }

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
            catch (ArgumentNullException kex)
            {
                _logger.LogWarning($"Dados invalidos.");
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
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.ValorNota <= 0)
                throw new BusinessException("O valor da nota é obrigatório e deve ser válido.");

            // Gerar o ID automaticamente caso seja 0
            if (entity.Id == 0)
            {
                var maxId = await _notaFiscalRepository.GetMaxId();
                entity.Id = maxId + 1;
            }

            if (entity.NumeroNota == 0)
            {
                var maxNumeroNota = await _notaFiscalRepository.GetMaxNumeroNota();
                entity.NumeroNota = maxNumeroNota + 1;
            }

            var novoCliente = await _clienteService.Insert(entity.Cliente);
            var novoFornecedor = await _fornecedorService.Insert(entity.Fornecedor);

            entity.Cliente = novoCliente ? entity.Cliente : throw new BusinessException("Falha ao inserir o Cliente.");
            entity.Fornecedor = novoFornecedor ? entity.Fornecedor : throw new BusinessException("Falha ao inserir o Fornecedor.");

            var result = await _notaFiscalRepository.Add(entity);
            if (!result)
                throw new BusinessException("Falha ao inserir a Nota Fiscal.");

            return true;
        }



        public async Task<bool> Update(NotaFiscal entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0 || entity.NumeroNota <= 0 || entity.ValorNota <= 0)
                    throw new ArgumentNullException(nameof(entity));

                if (entity.Cliente == null || entity.Cliente.Id <= 0 || string.IsNullOrEmpty(entity.Cliente.Nome))
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (entity.Fornecedor == null || entity.Fornecedor.Id <= 0 || string.IsNullOrEmpty(entity.Fornecedor.Nome))
                {
                    throw new ArgumentNullException(nameof(entity));
                }

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
            catch (ArgumentNullException aex)
            {
                _logger.LogWarning(aex, "Erro nos dados da nota fiscal.");
                throw;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Nota fiscal com ID: {entity.Id} não encontrada.");
                throw;
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Erro ao atualizar a nota fiscal.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar a nota fiscal com ID: {entity.Id}");
                throw new BusinessException($"Erro ao atualizar a nota fiscal com ID: {entity.Id}. {ex.Message}");
            }
        }

        public async Task<bool> Delete(NotaFiscal entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                {
                    throw new ArgumentNullException("id", "ID da Nota fiscal não informado.");
                }

                var notaFiscal = await _notaFiscalRepository.GetById(entity.Id);
                if (notaFiscal == null || notaFiscal.Id <= 0)
                {
                    throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                }

                var result = await _notaFiscalRepository.Remove(notaFiscal);
                if (!result)
                {
                    throw new BusinessException("Falha ao excluir a Nota Fiscal.");
                }

                return true;
            }
            catch (ArgumentNullException aex)
            {
                _logger.LogWarning(aex, "Erro nos dados da nota fiscal.");
                throw;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Nota fiscal com ID: {entity.Id} não encontrada.");
                throw;
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Erro ao excluir a nota fiscal.");
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