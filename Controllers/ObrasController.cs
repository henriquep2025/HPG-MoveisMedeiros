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
    [Authorize]
    public class ObrasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ObrasController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Obras/criar-obra
        [HttpPost("criar-obra")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> CriarObra([FromBody] CreateObraDto dto)
        {
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest(new { Mensagem = $"Cliente com ID {dto.ClienteId} não encontrado." });
            }

            var novaObra = new Obra
            {
                ClienteId = dto.ClienteId,
                NomeProjeto = dto.NomeProjeto,
                Endereco = dto.Endereco,
                Status = dto.Status,
                DataPrevistaInicio = dto.DataPrevistaInicio,
                DataEstimadaEntrega = dto.DataEstimadaEntrega,
                AmbientesContratados = dto.AmbientesContratados,
                MateriaisPrevistos = dto.MateriaisPrevistos,
                Preco = dto.Preco
            };

            _context.Obras.Add(novaObra);
            await _context.SaveChangesAsync();

            var itensPadrao = new List<ItemChecklistObra>
{
    new ItemChecklistObra { ObraId = novaObra.Id, Ordem = 1, Titulo = "Chapa e Substrato", Descricao = "Inspeção de empenamento, espessura e umidade aparente do MDF.", Status = "OK" },
    new ItemChecklistObra { ObraId = novaObra.Id, Ordem = 2, Titulo = "Cor e Textura", Descricao = "Uniformidade tonal sob iluminação da obra e compatibilidade com projeto.", Status = "OK" },
    new ItemChecklistObra { ObraId = novaObra.Id, Ordem = 3, Titulo = "Fita de Borda", Descricao = "Aderência, excesso de cola, acabamento de refilamento e cantoneiras.", Status = "OK" },
    new ItemChecklistObra { ObraId = novaObra.Id, Ordem = 4, Titulo = "Junções e Emendas", Descricao = "Alinhamento de 45 graus, prumo, nível de montantes e esquadro geral.", Status = "OK" },
    new ItemChecklistObra { ObraId = novaObra.Id, Ordem = 5, Titulo = "Outros", Descricao = "Materiais e acabamentos complementares não listados acima.", Status = "OK" }
};

            _context.ItensChecklistObra.AddRange(itensPadrao);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Obra criada com sucesso", Id = novaObra.Id });
        }

        // GET: api/Obras/listar-obras
        [HttpGet("listar-obras")]
        [Authorize(Roles = "administrador,inspetor,montador")]
        public async Task<ActionResult<List<ObraDto>>> GetObras()
        {
            bool isAdmin = User.IsInRole("administrador");

            var obras = await _context.Obras
                .Include(o => o.Cliente)
                .Select(o => new ObraDto
                {
                    Id = o.Id,
                    ClienteId = o.ClienteId,
                    ClienteNome = o.Cliente != null ? o.Cliente.Nome : string.Empty,
                    NomeProjeto = o.NomeProjeto,
                    Endereco = o.Endereco,
                    Status = o.Status,
                    DataPrevistaInicio = o.DataPrevistaInicio,
                    DataEstimadaEntrega = o.DataEstimadaEntrega,
                    AmbientesContratados = o.AmbientesContratados,
                    MateriaisPrevistos = o.MateriaisPrevistos,
                    Preco = isAdmin ? o.Preco : 0
                })
                .ToListAsync();

            return Ok(obras);
        }

        // GET: api/Obras/obter-obra/{id}
        [HttpGet("obter-obra/{id}")]
        [Authorize(Roles = "administrador,inspetor,montador")]
        public async Task<ActionResult<ObraDto>> GetObraPorId(int id)
        {
            bool isAdmin = User.IsInRole("administrador");

            var obra = await _context.Obras
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null) return NotFound(new { Mensagem = "Obra não encontrada" });

            return Ok(new ObraDto
            {
                Id = obra.Id,
                ClienteId = obra.ClienteId,
                ClienteNome = obra.Cliente != null ? obra.Cliente.Nome : string.Empty,
                NomeProjeto = obra.NomeProjeto,
                Endereco = obra.Endereco,
                Status = obra.Status,
                DataPrevistaInicio = obra.DataPrevistaInicio,
                DataEstimadaEntrega = obra.DataEstimadaEntrega,
                AmbientesContratados = obra.AmbientesContratados,
                MateriaisPrevistos = obra.MateriaisPrevistos,
                Preco = isAdmin ? obra.Preco : 0
            });
        }

        // PUT: api/Obras/editar-obra/{id}
        [HttpPut("editar-obra/{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> EditarObra(int id, [FromBody] CreateObraDto dto)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null) return NotFound(new { Mensagem = "Obra não encontrada" });

            obra.NomeProjeto = dto.NomeProjeto;
            obra.Endereco = dto.Endereco;
            obra.Status = dto.Status;
            obra.DataPrevistaInicio = dto.DataPrevistaInicio;
            obra.DataEstimadaEntrega = dto.DataEstimadaEntrega;
            obra.AmbientesContratados = dto.AmbientesContratados;
            obra.MateriaisPrevistos = dto.MateriaisPrevistos;
            obra.Preco = dto.Preco;

            await _context.SaveChangesAsync();
            return Ok(new { Mensagem = "Obra atualizada com sucesso" });
        }

        // DELETE: api/Obras/deletar-obra/{id}
        [HttpDelete("deletar-obra/{id}")]
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> DeletarObra(int id)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null) return NotFound(new { Mensagem = "Obra não encontrada" });

            _context.Obras.Remove(obra);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Obra excluída com sucesso" });
        }
    }
}