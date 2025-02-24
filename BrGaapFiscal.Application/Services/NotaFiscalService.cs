using BrGaapFiscal.Api.Repositores.Interfaces;
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
                var notasFiscais = await _notaFiscalRepository.GetAll();
                if (notasFiscais == null || !notasFiscais.Any())
                {
                    throw new BusinessException("Nenhuma nota fiscal encontrada.");
                }
                return notasFiscais;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro ao pesquisar as notas fiscais! {ex.Message}");
            }
        }

        public async Task<NotaFiscal> GetById(long id)
        {
            try
            {
                var notaFiscal = await _notaFiscalRepository.GetById(id);
                if (notaFiscal == null)
                {
                    throw new KeyNotFoundException("Nota fiscal não encontrada.");
                }
                return notaFiscal;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Erro ao buscar a nota fiscal. {ex.Message}");
            }
        }

        public async Task<bool> Insert(NotaFiscal entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Cliente == null) throw new ArgumentNullException(nameof(entity.Cliente));
            if (entity.Fornecedor == null) throw new ArgumentNullException(nameof(entity.Fornecedor));

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var cliente = await _clienteService.GetById(entity.Cliente.Id).ConfigureAwait(false);
                    if (cliente == null)
                    {
                        await _clienteService.Insert(entity.Cliente).ConfigureAwait(false);
                    }
                    else
                    {
                        entity.Cliente = cliente;
                    }

                    var fornecedor = await _fornecedorService.GetById(entity.Fornecedor.Id).ConfigureAwait(false);
                    if (fornecedor == null)
                    {
                        await _fornecedorService.Insert(entity.Fornecedor).ConfigureAwait(false);
                    }
                    else
                    {
                        entity.Fornecedor = fornecedor;
                    }

                    var result = await _notaFiscalRepository.Add(entity).ConfigureAwait(false);
                    if (result)
                    {
                        transaction.Complete();
                        return true;
                    }
                    else
                    {
                        throw new BusinessException("Falha ao inserir a Nota Fiscal");
                    }
                }
                catch (ArgumentNullException ex)
                {
                    _logger.LogError(ex, "Argumento nulo ao inserir a nota fiscal");
                    throw new BusinessException($"Erro ao inserir a nota fiscal. Argumento nulo: {ex.Message}");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar o banco de dados ao inserir a nota fiscal");
                    throw new BusinessException($"Erro ao inserir a nota fiscal. Problema no banco de dados: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao inserir a nota fiscal");
                    throw new BusinessException($"Erro ao inserir a nota fiscal. {ex.Message}");
                }
            }
        }

        public async Task<bool> Delete(NotaFiscal entity)
        {
            try
            {
                var notaFiscal = await _notaFiscalRepository.GetById(entity.Id).ConfigureAwait(false);
                if (notaFiscal == null)
                {
                    throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                }

                return await _notaFiscalRepository.Remove(entity).ConfigureAwait(false);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Erro ao deletar a nota fiscal: Nota Fiscal não encontrada.");
                throw new KeyNotFoundException("Erro ao deletar a nota fiscal: Nota Fiscal não encontrada.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar a nota fiscal.");
                throw new KeyNotFoundException("Erro ao deletar a nota fiscal.", ex);
            }
        }

        public async Task<bool> Update(NotaFiscal entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _logger.LogInformation($"Iniciando atualização da nota fiscal com ID: {entity.Id}");

                    var existeNotaFiscal = await _notaFiscalRepository.GetById(entity.Id).ConfigureAwait(false);
                    if (existeNotaFiscal == null)
                    {
                        _logger.LogWarning($"Nota Fiscal com ID: {entity.Id} não encontrada.");
                        throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                    }

                    if (entity.Cliente != null && entity.Cliente.Id > 0)
                    {
                        var cliente = await _clienteService.GetById(entity.Cliente.Id).ConfigureAwait(false);
                        if (cliente == null)
                        {
                            await _clienteService.Insert(entity.Cliente).ConfigureAwait(false);
                        }
                        else
                        {
                            await _clienteService.Update(entity.Cliente).ConfigureAwait(false);
                        }
                        existeNotaFiscal.Cliente = entity.Cliente;
                    }

                    if (entity.Fornecedor != null && entity.Fornecedor.Id > 0)
                    {
                        var fornecedor = await _fornecedorService.GetById(entity.Fornecedor.Id).ConfigureAwait(false);
                        if (fornecedor == null)
                        {
                            await _fornecedorService.Insert(entity.Fornecedor).ConfigureAwait(false);
                        }
                        else
                        {
                            await _fornecedorService.Update(entity.Fornecedor).ConfigureAwait(false);
                        }
                        existeNotaFiscal.Fornecedor = entity.Fornecedor;
                    }

                    if (entity.NumeroNota > 0)
                    {
                        existeNotaFiscal.NumeroNota = entity.NumeroNota;
                    }

                    if (entity.ValorNota > 0)
                    {
                        existeNotaFiscal.ValorNota = entity.ValorNota;
                    }

                    var result = await _notaFiscalRepository.Update(existeNotaFiscal).ConfigureAwait(false);
                    if (result)
                    {
                        transaction.Complete();
                        return true;
                    }
                    else
                    {
                        throw new BusinessException("Falha ao atualizar a Nota Fiscal");
                    }
                }
                catch (ArgumentNullException ex)
                {
                    _logger.LogError(ex, "Argumento nulo ao atualizar a nota fiscal");
                    throw new BusinessException($"Erro ao atualizar a nota fiscal. Argumento nulo: {ex.Message}");
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar a nota fiscal: Nota Fiscal não encontrada");
                    throw new BusinessException($"Erro ao atualizar a nota fiscal: {ex.Message}");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar o banco de dados ao atualizar a nota fiscal");
                    throw new BusinessException($"Erro ao atualizar a nota fiscal. Problema no banco de dados: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao atualizar a nota fiscal com ID: {entity.Id}");
                    throw new BusinessException($"Erro ao atualizar a nota fiscal. {ex.Message}");
                }
            }
        }
    }
}