using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrGaapFiscal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalService _notaFiscalService;

        public NotaFiscalController(INotaFiscalService notaFiscalService)
        {
            _notaFiscalService = notaFiscalService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNotaFiscal([FromBody] NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
            {
                return BadRequest("Nota fiscal inválida.");
            }

            try
            {
                await _notaFiscalService.Insert(notaFiscal);
                return CreatedAtAction(nameof(GetNotaFiscalById), new { id = notaFiscal.Id }, notaFiscal);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao adicionar nota fiscal: {ex.Message}");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllNotaFiscal()
        {
            var notaFiscais = await _notaFiscalService.GetAll();
            if (notaFiscais == null || !notaFiscais.Any())
            {
                return NotFound("Nenhuma nota fiscal encontrada.");
            }
            return Ok(notaFiscais);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNotaFiscalById(long id)
        {
            var notaFiscal = await _notaFiscalService.GetById(id);
            if (notaFiscal == null)
            {
                return NotFound("Nota fiscal não encontrada.");
            }
            return Ok(notaFiscal);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNotaFiscal(long id, [FromBody] NotaFiscal notaFiscal)
        {
            if (notaFiscal == null || notaFiscal.Id != id)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await _notaFiscalService.Update(notaFiscal);
                return Ok(notaFiscal);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Nota fiscal não encontrada.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar nota fiscal: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNotaFiscal(long id)
        {
            try
            {
                var notaFiscal = await _notaFiscalService.GetById(id);
                if (notaFiscal == null)
                {
                    return NotFound("Nota fiscal não encontrada.");
                }

                await _notaFiscalService.Delete(notaFiscal);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Nota fiscal não encontrada.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar nota fiscal: {ex.Message}");
            }
        }
    }
}