﻿using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Domain.Models;
using BrGaapFiscal.Infra.Data.Factory;
using Microsoft.EntityFrameworkCore;

namespace BrGaapFiscal.Api.Repositores
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly ApplicationDbContext _context;

        public FornecedorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetMaxId()
        {
            return await _context.Fornecedores.MaxAsync(f => (int?)f.Id) ?? 0;
        }


        public async Task<bool> Add(Fornecedor entity)
        {
            await _context.Fornecedores.AddAsync(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Remove(Fornecedor entity)
        {
            _context.Fornecedores.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Update(Fornecedor entity)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(entity.Id);

            _context.Entry(fornecedor).CurrentValues.SetValues(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Fornecedor>> GetAll(int pageNumber, int pageSize)
        {
            return await _context.Fornecedores
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Fornecedor> GetById(long id)
        {
            return await _context.Fornecedores.FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}