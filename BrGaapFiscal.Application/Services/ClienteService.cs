using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Repositores.Interfaces;

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
            return await _clienteRepository.Add(entity);
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
            return await _clienteRepository.GetAll();
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
            var existeCliente = await _clienteRepository.GetById(entity.Id);
            if (existeCliente == null)
            {
                throw new KeyNotFoundException("Cliente não encontrado(a).");
            }

            existeCliente.Nome = entity.Nome;

            return await _clienteRepository.Update(existeCliente);
        }
    }
}