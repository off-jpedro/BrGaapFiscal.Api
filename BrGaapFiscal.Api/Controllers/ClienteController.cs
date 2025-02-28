using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrGaapFiscal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest("Cliente inválido.");
            }

            try
            {
                await _clienteService.Insert(cliente);
                return CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao adicionar cliente: {ex.Message}");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCliente([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var clientes = await _clienteService.GetAll(pageNumber, pageSize);
            if (clientes == null || !clientes.Any())
            {
                return NotFound("Nenhum cliente encontrado.");
            }
            return Ok(clientes);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClienteById(long id)
        {
            var cliente = await _clienteService.GetById(id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }
            return Ok(cliente);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCliente(long id, [FromBody] Cliente cliente)
        {
            if (cliente == null || cliente.Id != id)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await _clienteService.Update(cliente);
                return Ok(cliente);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Cliente não encontrado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar cliente: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCliente(long id)
        {
            try
            {
                var cliente = await _clienteService.GetById(id);
                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado.");
                }

                await _clienteService.Delete(cliente);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Cliente não encontrado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar cliente: {ex.Message}");
            }
        }
    }
}