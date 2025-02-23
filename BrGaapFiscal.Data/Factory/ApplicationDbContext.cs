using BrGaapFiscal.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BrGaapFiscal.Infra.Data.Factory
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
