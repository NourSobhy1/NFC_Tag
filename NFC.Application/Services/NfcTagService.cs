using AutoMapper;
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
    public class NfcTagService(INfcTagRepository _tagRepo,IGenericRepository<ChemicalItem> _chemicalRepo ,IMapper _mapper) : INfcTagService
    {
        public async Task AddAsync(NfcTagRequestDto tagDto)
        {
            var tag = _mapper.Map<NfcTag>(tagDto);
            await _tagRepo.AddAsync(tag);
            await _tagRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<NfcTagDto>> GetAllAsync()
        {
            var tags = await _tagRepo.GetAllAsync(asNoTracking: true);
            return tags.Select(tag => _mapper.Map<NfcTagDto>(tag));
        }

        public async Task<NfcTagDto?> GetByIdAsync(Guid id)
        {
            var tag = await _tagRepo.GetByIdAsync(id);
            if(tag== null)
              throw new NotFoundException("NFC Tag Not Found ");
            var dto = _mapper.Map<NfcTagDto>(tag);
            return dto;
        }

        public async Task<ChemicalItemResultDto?> GetChemicalItemByUIdAsync(string uid)
        {
            var tag = await _tagRepo.GetByUIdAsync(uid);
            if (tag == null || tag.ChemicalItemId == null)
                throw new NotFoundException("No chemical item associated with this tag");

            var chemical = await _chemicalRepo.GetByIdAsync(tag.ChemicalItemId.Value);

            if (chemical == null)
                throw new NotFoundException("Chemical Item not found");

            return _mapper.Map<ChemicalItemResultDto>(chemical);
        }

        public async Task<NfcTagDto> UpdateAsync(NfcTagDto tagDto)
        {
            var tag = await _tagRepo.GetByIdAsync(tagDto.Id);
            if (tag == null)
                throw new NotFoundException("NFC tag not found ");
            tag.UId = tagDto.UId;
            tag.ChemicalItemId = tagDto.ChemicalItemId;
            _tagRepo.Update(tag);
            await _tagRepo.SaveChangesAsync();
            return _mapper.Map<NfcTagDto>(tag);
        }
    }
}
