using BrGaapFiscal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BrGaapFiscal.Api.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NotaFiscal> NotaFiscais { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

    }
}
