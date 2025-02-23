using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using System.Transactions;
using Microsoft.Extensions.Logging;

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
                    throw new KeyNotFoundException("Nenhuma nota fiscal encontrada.");
                }
                return notasFiscais;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Erro ao buscar as notas fiscais. {ex.Message}");
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
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var notaExistente = await _notaFiscalRepository.GetById(entity.Id);
                    if (notaExistente != null)
                    {
                        throw new ArgumentException("A nota fiscal com esse Id já existe.");
                    }

                    var cliente = await _clienteService.GetById(entity.Cliente.Id);
                    if (cliente == null)
                    {
                        await _clienteService.Insert(entity.Cliente);
                    }
                    else
                    {
                        entity.Cliente = cliente;
                    }

                    var fornecedor = await _fornecedorService.GetById(entity.Fornecedor.Id);
                    if (fornecedor == null)
                    {
                        await _fornecedorService.Insert(entity.Fornecedor);
                    }
                    else
                    {
                        entity.Fornecedor = fornecedor;
                    }

                    var result = await _notaFiscalRepository.Add(entity);
                    transaction.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new KeyNotFoundException($"Erro ao inserir a nota fiscal. {ex.Message}");
                }
            }
        }

        public async Task<bool> Delete(NotaFiscal entity)
        {
            var notaFiscal = await _notaFiscalRepository.GetById(entity.Id);
            if (notaFiscal == null)
            {
                throw new KeyNotFoundException("Nota Fiscal não encontrada.");
            }

            return await _notaFiscalRepository.Remove(entity);
        }

        public async Task<bool> Update(NotaFiscal entity)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _logger.LogInformation($"Iniciando atualização da nota fiscal com ID: {entity.Id}");

                    var existeNotaFiscal = await _notaFiscalRepository.GetById(entity.Id);
                    if (existeNotaFiscal == null)
                    {
                        _logger.LogWarning($"Nota Fiscal com ID: {entity.Id} não encontrada.");
                        throw new KeyNotFoundException("Nota Fiscal não encontrada.");
                    }

                    if (entity.Cliente != null && entity.Cliente.Id > 0)
                    {
                        var cliente = await _clienteService.GetById(entity.Cliente.Id);
                        if (cliente == null)
                        {
                            await _clienteService.Insert(entity.Cliente);
                        }
                        else
                        {
                            await _clienteService.Update(entity.Cliente);
                        }
                        existeNotaFiscal.Cliente = entity.Cliente;
                    }

                    if (entity.Fornecedor != null && entity.Fornecedor.Id > 0)
                    {
                        var fornecedor = await _fornecedorService.GetById(entity.Fornecedor.Id);
                        if (fornecedor == null)
                        {
                            await _fornecedorService.Insert(entity.Fornecedor);
                        }
                        else
                        {
                            await _fornecedorService.Update(entity.Fornecedor);
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

                    var result = await _notaFiscalRepository.Update(existeNotaFiscal);
                    transaction.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao atualizar a nota fiscal com ID: {entity.Id}");
                    throw new KeyNotFoundException($"Erro ao atualizar a nota fiscal. {ex.Message}");
                }
            }
        }
    }
}