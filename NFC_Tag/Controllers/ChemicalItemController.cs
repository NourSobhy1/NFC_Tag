using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFC.Application.DTOs;
using NFC.Application.Interfaces.IServices;
using NFC.Application.Services;

namespace NFC_Tag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChemicalItemController(IChemicalItemService _itemService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add(ChemicalItemRequestDto dto)
        {
            var id = await _itemService.AddAsync(dto);
            return Ok(new { message = "Item added Successfully ", Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChemicalItem(ChemicalItemResultDto dto)
        {
            await _itemService.UpdateAsync(dto);
            return Ok("updated successfully");

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _itemService.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _itemService.DeleteAsync(id);
            return Ok("Deleted Successfully");
        }
    }
}
