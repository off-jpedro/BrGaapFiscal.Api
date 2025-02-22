using BrGaapFiscal.Api.Models;
using BrGaapFiscal.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
                await _clienteService.AddCliente(cliente);
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
        public async Task<IActionResult> GetAllCliente()
        {
            var clientes = await _clienteService.GetAllCliente();
            if (clientes == null || !clientes.Any())
            {
                return NotFound("Nenhum cliente encontrado.");
            }
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _clienteService.GetClienteById(id);
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
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Cliente cliente)
        {
            if (cliente == null || cliente.Id != id)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await _clienteService.UpdateCliente(cliente);
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
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                await _clienteService.DeleteClienteById(id);
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
