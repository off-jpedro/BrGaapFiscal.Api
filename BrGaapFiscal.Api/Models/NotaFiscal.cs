namespace BrGaapFiscal.Api.Models
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int NumeroNota { get; set; }
        public decimal ValorNota { get; set; }
        public Cliente Cliente { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}
