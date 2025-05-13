using NFC.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.Interfaces.IServices
{
   public interface INfcTagService
    {
        Task<NfcTagDto?> GetByIdAsync(Guid id);
        Task<ChemicalItemResultDto?> GetChemicalItemByUIdAsync(string uid);
        Task<IEnumerable<NfcTagDto>> GetAllAsync();
        Task AddAsync(NfcTagRequestDto tagDto);
        Task<NfcTagDto> UpdateAsync(NfcTagDto tagDto);

    }
}
