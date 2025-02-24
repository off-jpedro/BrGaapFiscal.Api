using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Application.Exceptions;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Repositores.Interfaces;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace BrGaapFiscal.Api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IClienteRepository clienteRepository, ILogger<ClienteService> logger)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
        }

        public async Task<bool> Insert(Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var clienteExistente = await _clienteRepository.GetById(entity.Id).ConfigureAwait(false);
                    if (clienteExistente != null)
                    {
                        _logger.LogWarning("Cliente já existe com o ID: {ClienteId}", entity.Id);
                        throw new ArgumentException("Cliente já existe.");
                    }

                    var result = await _clienteRepository.Add(entity).ConfigureAwait(false);
                    if (!result)
                    {
                        throw new BusinessException("Falha ao inserir o Cliente.");
                    }

                    transaction.Complete();
                    return true;
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao inserir o cliente.");
                    throw new BusinessException($"Erro ao inserir o cliente. {ex.Message}");
                }
            }
        }

        public async Task<bool> Delete(Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                var cliente = await _clienteRepository.GetById(entity.Id).ConfigureAwait(false);
                if (cliente == null || cliente.Id <= 0)
                {
                    _logger.LogWarning("Cliente não encontrado com o ID: {ClienteId}", entity.Id);
                    throw new KeyNotFoundException("Cliente não encontrado(a).");
                }

                return await _clienteRepository.Remove(entity).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar o cliente.");
                throw new BusinessException($"Erro ao deletar o cliente. {ex.Message}");
            }
        }

        public async Task<IEnumerable<Cliente>> GetAll()
        {
            try
            {
                return await _clienteRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pesquisar os clientes!");
                throw new BusinessException($"Erro ao pesquisar os clientes! {ex.Message}");
            }
        }

        public async Task<Cliente> GetById(long id)
        {
            try
            {
                var cliente = await _clienteRepository.GetById(id).ConfigureAwait(false);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado com o ID: {ClienteId}", id);
                    throw new KeyNotFoundException("Cliente não encontrado(a).");
                }

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o cliente.");
                throw new BusinessException($"Erro ao buscar o cliente. {ex.Message}");
            }
        }

        public async Task<bool> Update(Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existeCliente = await _clienteRepository.GetById(entity.Id).ConfigureAwait(false);
                    if (existeCliente == null)
                    {
                        _logger.LogWarning("Cliente não encontrado com o ID: {ClienteId}", entity.Id);
                        throw new KeyNotFoundException("Cliente não encontrado(a).");
                    }

                    existeCliente.Nome = entity.Nome;

                    var result = await _clienteRepository.Add(entity).ConfigureAwait(false);
                    if (!result)
                    {
                        throw new BusinessException("Falha ao inserir o Cliente.");
                    }

                    transaction.Complete();
                    return true;

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar o cliente.");
                    throw new BusinessException($"Erro ao atualizar o cliente. {ex.Message}");
                }
            }
        }
    }
}