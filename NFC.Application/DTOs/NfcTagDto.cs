using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.DTOs
{
    public class NfcTagDto
    {
        public Guid Id { get; set; }
        public string UId { get; set; } = String.Empty;
        public Guid? ChemicalItemId { get; set; }
        public string ChemicalItemName { get; set; }
    }
}
