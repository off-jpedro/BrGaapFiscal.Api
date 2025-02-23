using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Repositores.Interfaces;
using System.Transactions;

namespace BrGaapFiscal.Api.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<bool> Insert(Cliente entity)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var clienteExistente = await _clienteRepository.GetById(entity.Id);
                    if (clienteExistente != null)
                    {
                        throw new ArgumentException("Cliente já existe.");
                    }

                    var result = await _clienteRepository.Add(entity);
                    transaction.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new KeyNotFoundException($"Erro ao inserir o cliente. {ex.Message}");
                }
            }
        }

        public async Task<bool> Delete(Cliente entity)
        {
            var cliente = await _clienteRepository.GetById(entity.Id);
            if (cliente == null)
            {
                throw new KeyNotFoundException("Cliente não encontrado(a).");
            }

            return await _clienteRepository.Remove(entity);
        }

        public async Task<IEnumerable<Cliente>> GetAll()
        {
            try
            {
                var clientes = await _clienteRepository.GetAll();
                if (clientes == null || !clientes.Any())
                {
                    throw new KeyNotFoundException("Nenhum cliente encontrado.");
                }
                return clientes;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Erro ao buscar clientes. {ex.Message}");
            }
        }

        public async Task<Cliente> GetById(long id)
        {
            var cliente = await _clienteRepository.GetById(id);
            if (cliente == null)
            {
                throw new KeyNotFoundException("Cliente não encontrado(a).");
            }

            return cliente;
        }

        public async Task<bool> Update(Cliente entity)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var existeCliente = await _clienteRepository.GetById(entity.Id);
                    if (existeCliente == null)
                    {
                        throw new KeyNotFoundException("Cliente não encontrado(a).");
                    }

                    existeCliente.Nome = entity.Nome;

                    var result = await _clienteRepository.Update(existeCliente);
                    transaction.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new KeyNotFoundException($"Erro ao atualizar o cliente. {ex.Message}");
                }
            }
        }
    }
}