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
                if (id <= 0)
                {
                    throw new KeyNotFoundException("ID não informado.");
                }

                var cliente = await _clienteRepository.GetById(id);

                if (cliente == null || cliente.Id <= 0)
                {
                    throw new KeyNotFoundException("Cliente não encontrado.");
                }

                return cliente;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Cliente com ID: {id} não encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o Cliente com ID: {id}");
                throw new BusinessException($"Erro ao buscar o Cliente com ID: {id}. {ex.Message}");
            }
        }

        public async Task<bool> Insert(Cliente entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0 || string.IsNullOrEmpty(entity.Nome))
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                var result = await _clienteRepository.Add(entity);

                if (!result)
                {
                    throw new BusinessException("Falha ao inserir o Cliente.");
                }

                return true;
            }
            catch (ArgumentNullException aex)
            {
                _logger.LogWarning(aex, "Erro nos dados do cliente.");
                throw;
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Erro ao inserir o cliente.");
                throw;
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
                if (entity == null || entity.Id <= 0 || string.IsNullOrEmpty(entity.Nome))
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                var existeCliente = await _clienteRepository.GetById(entity.Id);

                if (existeCliente == null || existeCliente.Id <= 0)
                {
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
            catch (ArgumentNullException aex)
            {
                _logger.LogWarning(aex, "Erro nos dados do cliente.");
                throw;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Cliente com ID: {entity.Id} não encontrado.");
                throw;
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Erro ao atualizar o cliente.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o Cliente com ID: {entity.Id}");
                throw;
            }
        }

        public async Task<bool> Delete(Cliente entity)
        {
            try
            {
                if (entity == null || entity.Id <= 0)
                {
                    throw new ArgumentNullException("id", "ID do Cliente não informado.");
                }

                var cliente = await _clienteRepository.GetById(entity.Id);

                if (cliente == null || cliente.Id <= 0)
                {
                    throw new KeyNotFoundException("Cliente não encontrado.");
                }

                var result = await _clienteRepository.Remove(cliente);

                if (!result)
                {
                    throw new BusinessException("Falha ao excluir o Cliente.");
                }

                return true;
            }
            catch (ArgumentNullException aex)
            {
                _logger.LogWarning(aex, "Erro nos dados do cliente.");
                throw;
            }
            catch (KeyNotFoundException kex)
            {
                _logger.LogWarning($"Cliente com ID: {entity.Id} não encontrado.");
                throw;
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Erro ao excluir o cliente.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar o Cliente com ID: {entity.Id}");
                throw new BusinessException($"Erro ao deletar o Cliente com ID: {entity.Id}. {ex.Message}");
            }
        }
    }
}