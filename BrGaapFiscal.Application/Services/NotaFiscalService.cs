using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Repositores.Interfaces;

namespace BrGaapFiscal.Api.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _notaFiscalRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public NotaFiscalService(
            INotaFiscalRepository notaFiscalRepository,
            IClienteRepository clienteRepository,
            IFornecedorRepository fornecedorRepository)
        {
            _notaFiscalRepository = notaFiscalRepository;
            _clienteRepository = clienteRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<IEnumerable<NotaFiscal>> GetAll()
        {
            return await _notaFiscalRepository.GetAll();
        }

        public async Task<NotaFiscal> GetById(long id)
        {
            return await _notaFiscalRepository.GetById(id);
        }

        public async Task<bool> Insert(NotaFiscal entity)
        {
            await _clienteRepository.Add(entity.Cliente);
            await _fornecedorRepository.Add(entity.Fornecedor);
            return await _notaFiscalRepository.Add(entity);
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
            var existeNotaFiscal = await _notaFiscalRepository.GetById(entity.Id);
            if (existeNotaFiscal == null)
            {
                throw new KeyNotFoundException("Nota Fiscal não encontrada.");
            }

            existeNotaFiscal.Cliente = entity.Cliente;
            existeNotaFiscal.Fornecedor = entity.Fornecedor;
            existeNotaFiscal.NumeroNota = entity.NumeroNota;
            existeNotaFiscal.ValorNota = entity.ValorNota;

            return await _notaFiscalRepository.Update(existeNotaFiscal);
        }
    }
}