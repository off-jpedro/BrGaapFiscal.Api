﻿using BrGaapFiscal.Domain.Core.Interfaces;

namespace BrGaapFiscal.Domain.Models

{
    public class Cliente : IEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; }
     
    }
}
