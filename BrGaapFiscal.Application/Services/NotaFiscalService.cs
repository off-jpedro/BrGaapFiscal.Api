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
                var notaFiscal = await _notaFiscalRepository.GetById(id).ConfigureAwait(false);
                if (notaFiscal == null || notaFiscal.Id <= 0)
                {
                    _logger.LogWarning($"Nota fiscal com ID: {id} não encontrada.");
                    throw new KeyNotFoundException("Nota fiscal não encontrada.");
                }
                return notaFiscal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar a nota fiscal com ID: {id}");
                throw new BusinessException($"Erro ao buscar a nota fiscal com ID: {id}. {ex.Message}");
            }
        }

        public async Task<bool> Insert(NotaFiscal entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Cliente == null || entity.Cliente.Id <= 0 || string.IsNullOrEmpty(entity.Cliente.Nome))
                throw new ArgumentException("Cliente inválido. Veja se está preenchendo os campos obrigatórios");
            if (entity.Fornecedor == null || entity.Fornecedor.Id <= 0 || string.IsNullOrEmpty(entity.Fornecedor.Nome))
                throw new ArgumentException("Fornecedor inválido. Veja se está preenchendo os campos obrigatórios");

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var cliente = await _clienteService.GetById(entity.Cliente.Id).ConfigureAwait(false);
                    if (cliente == null || cliente.Id <= 0)
                    {
                        await _clienteService.Insert(entity.Cliente).ConfigureAwait(false);
                    }
                    else
                    {
                        entity.Cliente = cliente;
                    }

                    var fornecedor = await _fornecedorService.GetById(entity.Fornecedor.Id).ConfigureAwait(false);
                    if (fornecedor == null || fornecedor.Id <= 0)
                    {
                        await _fornecedorService.Insert(entity.Fornecedor).ConfigureAwait(false);
                    }
                    else
                    {
                        entity.Fornecedor = fornecedor;
                    }

                    var result = await _notaFiscalRepository.Add(entity).ConfigureAwait(false);
                    if (!result)
                    {
                        throw new BusinessException("Falha ao inserir a Nota Fiscal");
                    }

                    transaction.Complete();
                    return true;
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
                if (notaFiscal == null || notaFiscal.Id <= 0)
                {
                    _logger.LogWarning($"Nota Fiscal com ID: {entity.Id} não encontrada.");
                    throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                }

                return await _notaFiscalRepository.Remove(entity).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar a nota fiscal com ID: {entity.Id}");
                throw new BusinessException($"Erro ao deletar a nota fiscal com ID: {entity.Id}. {ex.Message}");
            }
        }

        public async Task<bool> Update(NotaFiscal entity)
        {
            if (entity == null || entity.Id <= 0) throw new ArgumentNullException(nameof(entity));

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _logger.LogInformation($"Iniciando atualização da nota fiscal com ID: {entity.Id}");

                    var existeNotaFiscal = await _notaFiscalRepository.GetById(entity.Id).ConfigureAwait(false);
                    if (existeNotaFiscal == null || existeNotaFiscal.Id <= 0)
                    {
                        _logger.LogWarning($"Nota Fiscal com ID: {entity.Id} não encontrada.");
                        throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                    }

                    var result = await _notaFiscalRepository.Update(existeNotaFiscal).ConfigureAwait(false);
                    if (!result)
                    {
                        throw new BusinessException("Falha ao atualizar a Nota Fiscal");
                    }

                    transaction.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao atualizar a nota fiscal com ID: {entity.Id}");
                    throw new BusinessException($"Erro ao atualizar a nota fiscal com ID: {entity.Id}. {ex.Message}");
                }
            }
        }

    }
}