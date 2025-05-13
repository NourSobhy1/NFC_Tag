using NFC.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.Interfaces.IServices
{
   public interface IChemicalItemService
    {
        Task<ChemicalItemResultDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ChemicalItemResultDto>> GetAllAsync();
        Task<Guid> AddAsync(ChemicalItemRequestDto itemDto);
        Task<ChemicalItemResultDto> UpdateAsync(ChemicalItemResultDto itemDto);
        Task DeleteAsync(Guid id);
    }
}
