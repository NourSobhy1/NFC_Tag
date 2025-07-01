//using AutoMapper;
//using Microsoft.Extensions.Configuration;
//using NFC.Application.DTOs;
//using NFC.Domain.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NFC.Application.MappingProfiles
//{
//    class ImageUrlResolver(IConfiguration _configuration) : IValueResolver<ChemicalItem, ChemicalItemResultDto, string>
//    {
       
//        public string Resolve(ChemicalItem source, ChemicalItemResultDto destination, string destMember, ResolutionContext context)
//        {
//            if (string.IsNullOrWhiteSpace(source.ImageUrl)) return string.Empty;

//            //return $"https://localhost:7185{source.ImageUrl}";
//            return $"{_configuration["baseUrl"]}{source.ImageUrl}";

//        }
//    }
//}
