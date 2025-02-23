using BrGaapFiscal.Domain.Interfaces;

namespace BrGaapFiscal.Domain.Models

{
    public class Cliente : IEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; }
     
    }
}
