using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.DTOs
{
   public class ChemicalItemRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string TradeName { get; set; } = string.Empty;
        public string ChemicalFormula { get; set; } = string.Empty;
        public string Casnumber { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string StorageInstruction { get; set; } = string.Empty;
        public string? Disposalmethod { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal BoilingPoint { get; set; }
        public decimal MeltingPoint { get; set; }
        public decimal Density { get; set; }
        public string? SpecialWarning { get; set; }
        public string? Solubility { get; set; }
        public List<string> SafetyInstructions { get; set; } = new();
    }
}
