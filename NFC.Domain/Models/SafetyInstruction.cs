using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Domain.Models
{
   public class SafetyInstruction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Instruction { get; set; } = string.Empty;
        public Guid ChemicalItemId { get; set; }
        public ChemicalItem ChemicalItem { get; set; }

    }
}
