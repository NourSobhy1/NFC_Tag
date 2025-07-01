using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.DTOs
{
   public class NfcTagRequestDto
    {
        public string UId { get; set; } = String.Empty;
        public Guid ChemicalItemId { get; set; }
    }
}
