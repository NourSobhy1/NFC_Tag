using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NFC.Application.DTOs;
using NFC.Application.Exceptions;
using NFC.Application.Interfaces;
using NFC.Application.Interfaces.IServices;
using NFC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.Services
{
    public class ChemicalItemService(IGenericRepository<ChemicalItem> _chemicalRepo, IMapper _mapper, ICurrentUserService _currentUserService) : IChemicalItemService
    {
        public async Task<Guid> AddAsync(ChemicalItemRequestDto itemDto)
        {
            var chemicalitem = _mapper.Map<ChemicalItem>(itemDto);
            chemicalitem.AdminId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
            await _chemicalRepo.AddAsync(chemicalitem);
            await _chemicalRepo.SaveChangesAsync();
            return chemicalitem.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var chemicalitem = await _chemicalRepo.GetByIdAsync(id);
            if(chemicalitem== null)
            throw new NotFoundException("Chemical item not found");
            _chemicalRepo.Delete(chemicalitem);
            await _chemicalRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChemicalItemResultDto>> GetAllAsync()
        {
            var chemicalitems = await _chemicalRepo.GetAllWithIncludesAsync(c => c.SafetyInstructions);
            var filtered = chemicalitems.Where(item => item.AdminId == _currentUserService.UserId).ToList();
            return _mapper.Map<IEnumerable<ChemicalItemResultDto>>(filtered);
        }

        public async Task<ChemicalItemResultDto?> GetByIdAsync(Guid id)
        {
            var chemicalitem = await _chemicalRepo.GetByIdWithIncludesAsync(id, c => c.SafetyInstructions);
            if (chemicalitem == null)
                throw new NotFoundException("Chemical item not found");
            return _mapper.Map<ChemicalItemResultDto>(chemicalitem);
        }

        public async Task<ChemicalItemResultDto> UpdateAsync(ChemicalItemResultDto itemDto)
        {
            var chemicalitem = await _chemicalRepo.GetByIdAsync(itemDto.Id);
            if (chemicalitem == null)
                throw new NotFoundException("Chemical item not found");
            _mapper.Map(itemDto, chemicalitem);
            _chemicalRepo.Update(chemicalitem);
            await _chemicalRepo.SaveChangesAsync();
            return _mapper.Map<ChemicalItemResultDto>(chemicalitem);
        }
    }
}
