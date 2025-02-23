using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;

namespace BrGaapFiscal.Api.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _repository;

        public FornecedorService(IFornecedorRepository fornecedorRepository)
        {
            _repository = fornecedorRepository;
        }

        public async Task<bool> Insert(Fornecedor entity)
        {
            return await _repository.Add(entity);
        }

        public async Task<bool> Delete(Fornecedor entity)
        {
            var fornecedor = await _repository.GetById(entity.Id);
            if (fornecedor == null)
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }

            return await _repository.Remove(entity);
        }

        public async Task<IEnumerable<Fornecedor>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Fornecedor> GetById(long id)
        {
            var fornecedor = await _repository.GetById(id);
            if (fornecedor == null)
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }

            return fornecedor;
        }

        public async Task<bool> Update(Fornecedor entity)
        {
            var existeFornecedor = await _repository.GetById(entity.Id);
            if (existeFornecedor == null)
            {
                throw new KeyNotFoundException("Fornecedor não encontrado(a).");
            }

            existeFornecedor.Nome = entity.Nome;

            return await _repository.Update(existeFornecedor);
        }
    }
}