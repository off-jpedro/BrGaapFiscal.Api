using BrGaapFiscal.Domain.Interfaces;

namespace BrGaapFiscal.Domain.Models
{
    public class NotaFiscal : IEntity
    {
        public int Id { get; set; }
        public int NumeroNota { get; set; }
        public decimal ValorNota { get; set; }
        public Cliente Cliente { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}
