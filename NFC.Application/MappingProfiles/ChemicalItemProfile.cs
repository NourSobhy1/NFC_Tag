using AutoMapper;
using NFC.Application.DTOs;
using NFC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.MappingProfiles
{
   public class ChemicalItemProfile : Profile
    {
        public ChemicalItemProfile()
        {
            CreateMap<ChemicalItem, ChemicalItemResultDto>()
                .ForMember(
                    dest => dest.SafetyInstructions,
                    opt => opt.MapFrom(src => src.SafetyInstructions.Select(si => si.Instruction).ToList())
                )
                .ForMember(dest => dest.ImageUrl,
                           opt => opt.MapFrom<ImageUrlResolver>()
                );
            //BaseUrl/images/sulfuric_acid.jpg

            CreateMap<NfcTag, NfcTagDto>()
                .ForMember(d => d.ChemicalItemName, options => options.MapFrom(s => s.ChemicalItem.Name));

            CreateMap<ChemicalItemRequestDto, ChemicalItem>()
                .ForMember(dest => dest.SafetyInstructions, opt => opt.MapFrom(src => src.SafetyInstructions.Select(instr => new SafetyInstruction { Instruction = instr })));

            CreateMap<ChemicalItemResultDto, ChemicalItem>()
                .ForMember(dest => dest.SafetyInstructions, opt => opt.Ignore());

            CreateMap<NfcTagRequestDto, NfcTag>();
        }
    }
}
