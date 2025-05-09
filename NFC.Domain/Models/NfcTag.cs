using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Domain.Models
{
   public class NfcTag 
    {
        [Key]
        public Guid Id { get; set; }
        public string UId { get; set; } = String.Empty;
        public Guid? ChemicalItemId { get; set; }
        public ChemicalItem? ChemicalItem { get; set; }

    }
}
