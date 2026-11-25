using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMoveisMedeiros.Api.Data;
using ProjetoMoveisMedeiros.Api.Models;
using ProjetoMoveisMedeiros.Api.Models.DTOs;

namespace ProjetoMoveisMedeiros.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "administrador")] // <-- Corrigido de "Admin" para "administrador"
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("criar-cliente")]
        public async Task<IActionResult> CriarCliente([FromBody] CreateClienteDto dto)
        {
            var novoCliente = new Cliente
            {
                TipoPessoa = dto.TipoPessoa,
                Nome = dto.Nome,
                Documento = dto.Documento,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Cep = dto.Cep,
                Endereco = dto.Endereco
            };

            _context.Clientes.Add(novoCliente);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Cliente cadastrado com sucesso", Id = novoCliente.Id });
        }

        [HttpGet("listar-clientes")]
        public async Task<ActionResult<List<ClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes.Select(c => new ClienteDto
            {
                Id = c.Id,
                TipoPessoa = c.TipoPessoa,
                Nome = c.Nome,
                Documento = c.Documento,
                DataNascimento = c.DataNascimento,
                Email = c.Email,
                Telefone = c.Telefone,
                Cep = c.Cep,
                Endereco = c.Endereco
            }).ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("obter-cliente/{id}")]
        public async Task<ActionResult<ClienteDto>> GetClientePorId(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound(new { Mensagem = "Cliente não encontrado" });

            return Ok(new ClienteDto
            {
                Id = c.Id,
                TipoPessoa = c.TipoPessoa,
                Nome = c.Nome,
                Documento = c.Documento,
                DataNascimento = c.DataNascimento,
                Email = c.Email,
                Telefone = c.Telefone,
                Cep = c.Cep,
                Endereco = c.Endereco
            });
        }

        [HttpPut("editar-cliente/{id}")]
        public async Task<IActionResult> EditarCliente(int id, [FromBody] CreateClienteDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound(new { Mensagem = "Cliente não encontrado" });

            cliente.TipoPessoa = dto.TipoPessoa;
            cliente.Nome = dto.Nome;
            cliente.Documento = dto.Documento;
            cliente.DataNascimento = dto.DataNascimento;
            cliente.Email = dto.Email;
            cliente.Telefone = dto.Telefone;
            cliente.Cep = dto.Cep;
            cliente.Endereco = dto.Endereco;

            await _context.SaveChangesAsync();
            return Ok(new { Mensagem = "Cliente atualizado com sucesso" });
        }

        [HttpDelete("deletar-cliente/{id}")]
        public async Task<IActionResult> DeletarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound(new { Mensagem = "Cliente não encontrado" });

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Cliente removido com sucesso" });
        }
    }
}