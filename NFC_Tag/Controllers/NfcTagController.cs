using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFC.Application.DTOs;
using NFC.Application.Interfaces.IServices;

namespace NFC_Tag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NfcTagController : ControllerBase
    {
        private readonly INfcTagService _nfcTagService;

        public NfcTagController(INfcTagService nfcTagService)
        {
            _nfcTagService = nfcTagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _nfcTagService.GetAllAsync();
            return Ok(tags);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tag = await _nfcTagService.GetByIdAsync(id);
            return Ok(tag);
        }

        [HttpGet("Chemical-by-Uid/{uid}")]
        public async Task<IActionResult> GetChemicalItemByUid(string uid)
        {
            var chemicalItem = await _nfcTagService.GetChemicalItemByUIdAsync(uid);
            return Ok(chemicalItem);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NfcTagRequestDto tagDto)
        {
            await _nfcTagService.AddAsync(tagDto);
            return Ok("Added Successfully");
        }

    }
}
