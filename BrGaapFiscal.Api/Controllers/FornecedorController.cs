using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrGaapFiscal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorService _fornecedorService;

        public FornecedorController(IFornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFornecedor([FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null)
            {
                return BadRequest("Fornecedor inválido.");
            }

            try
            {
                await _fornecedorService.Insert(fornecedor);
                return CreatedAtAction(nameof(GetFornecedorById), new { id = fornecedor.Id }, fornecedor);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao adicionar fornecedor: {ex.Message}");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFornecedor([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var fornecedores = await _fornecedorService.GetAll(pageNumber, pageSize);
            if (fornecedores == null || !fornecedores.Any())
            {
                return NotFound("Nenhum fornecedor encontrado.");
            }
            return Ok(fornecedores);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFornecedorById(long id)
        {
            var fornecedor = await _fornecedorService.GetById(id);
            if (fornecedor == null)
            {
                return NotFound("Fornecedor não encontrado.");
            }
            return Ok(fornecedor);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFornecedor(long id, [FromBody] Fornecedor fornecedor)
        {
            if (fornecedor == null || fornecedor.Id != id)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await _fornecedorService.Update(fornecedor);
                return Ok(fornecedor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Fornecedor não encontrado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar fornecedor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFornecedor(long id)
        {
            try
            {
                var fornecedor = await _fornecedorService.GetById(id);
                if (fornecedor == null)
                {
                    return NotFound("Fornecedor não encontrado.");
                }

                await _fornecedorService.Delete(fornecedor);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Fornecedor não encontrado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar fornecedor: {ex.Message}");
            }
        }
    }
}