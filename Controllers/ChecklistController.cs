using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMoveisMedeiros.Api.Data;
using ProjetoMoveisMedeiros.Api.Models.DTOs;

namespace ProjetoMoveisMedeiros.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChecklistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChecklistController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Checklist/listar-por-obra/{obraId}
        [HttpGet("listar-por-obra/{obraId}")]
        [Authorize(Roles = "administrador,inspetor,montador")]
        public async Task<ActionResult<List<ItemChecklistObraDto>>> GetChecklistPorObra(int obraId)
        {
            var itens = await _context.ItensChecklistObra
                .Where(i => i.ObraId == obraId)
                .OrderBy(i => i.Ordem)
                .Select(i => new ItemChecklistObraDto
                {
                    Id = i.Id,
                    ObraId = i.ObraId,
                    Ordem = i.Ordem,
                    Titulo = i.Titulo,
                    Descricao = i.Descricao,
                    Status = i.Status,
                    Observacao = i.Observacao,
                    FotoUrl = i.FotoUrl
                })
                .ToListAsync();

            return Ok(itens);
        }

        // PUT: api/Checklist/editar-item/{id}
        [HttpPut("editar-item/{id}")]
        [Authorize(Roles = "administrador,inspetor,montador")]
        public async Task<IActionResult> EditarItem(int id, [FromBody] ItemChecklistObraDto dto)
        {
            var item = await _context.ItensChecklistObra.FindAsync(id);
            if (item == null) return NotFound(new { Mensagem = "Item do checklist não encontrado." });

            var statusNormalizado = dto.Status?.Trim().ToUpper();
            var statusPermitidos = new[] { "PENDENTE", "OK", "NOK", "NA" };

            if (string.IsNullOrEmpty(statusNormalizado) || !statusPermitidos.Contains(statusNormalizado))
            {
                return BadRequest(new { Mensagem = "Status inválido. Valores aceitos: PENDENTE, OK, NOK ou NA." });
            }

            if (statusNormalizado == "NOK")
            {
                if (string.IsNullOrWhiteSpace(dto.Observacao))
                {
                    return BadRequest(new { Mensagem = "A observação é obrigatória quando o item está marcado como NOK." });
                }

                if (string.IsNullOrWhiteSpace(dto.FotoUrl))
                {
                    return BadRequest(new { Mensagem = "A foto de evidência é obrigatória quando o item está marcado como NOK." });
                }
            }

            item.Status = statusNormalizado;
            item.Observacao = dto.Observacao;
            item.FotoUrl = dto.FotoUrl;

            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Item do checklist atualizado com sucesso!" });
        }
    }
}