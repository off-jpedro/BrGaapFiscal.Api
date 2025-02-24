using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Application.Exceptions;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Repositores.Interfaces;
using Microsoft.Extensions.Logging;

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
                var cliente = await _clienteRepository.GetById(id);
                if (cliente == null || cliente.Id <= 0)
                {
                    _logger.LogWarning("Cliente não encontrado com o ID: {ClienteId}", id);
                    throw new KeyNotFoundException("Cliente não encontrado.");
                }
                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o cliente com ID: {ClienteId}", id);
                throw new BusinessException($"Erro ao buscar o cliente com ID: {id}. {ex.Message}");
            }
        }

        public async Task<bool> Insert(Cliente entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                var clienteExistente = await _clienteRepository.GetById(entity.Id);
                if (clienteExistente != null && clienteExistente.Id > 0)
                {
                    _logger.LogWarning("Cliente já existe com o ID: {ClienteId}", entity.Id);
                    throw new ArgumentException("Cliente já existe.");
                }

                var result = await _clienteRepository.Add(entity);
                if (!result)
                {
                    throw new BusinessException("Falha ao inserir o Cliente.");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir o cliente.");
                throw;
            }
        }

        public async Task<bool> Update(Cliente entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                    throw new ArgumentNullException(nameof(entity));

                var existeCliente = await _clienteRepository.GetById(entity.Id);
                if (existeCliente == null || existeCliente.Id <= 0)
                {
                    _logger.LogWarning("Cliente não encontrado com o ID: {ClienteId}", entity.Id);
                    throw new KeyNotFoundException("Cliente não encontrado.");
                }

                existeCliente.Nome = entity.Nome;

                var result = await _clienteRepository.Update(existeCliente);
                if (!result)
                {
                    throw new BusinessException("Falha ao atualizar o Cliente.");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o cliente com ID: {ClienteId}", entity.Id);
                throw;
            }
        }

        public async Task<bool> Delete(Cliente entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                    throw new ArgumentNullException("id", "ID do Cliente não informado.");

                var cliente = await _clienteRepository.GetById(entity.Id);
                if (cliente == null || cliente.Id <= 0)
                {
                    _logger.LogWarning("Cliente não encontrado com o ID: {ClienteId}", entity.Id);
                    throw new KeyNotFoundException("Cliente não encontrado.");
                }

                return await _clienteRepository.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar o cliente com ID: {ClienteId}", entity.Id);
                throw new BusinessException($"Erro ao deletar o cliente com ID: {entity.Id}. {ex.Message}");
            }
        }
    }
}
