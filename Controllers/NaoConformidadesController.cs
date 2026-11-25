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
    public class NaoConformidadesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NaoConformidadesController(AppDbContext context)
        {
            _context = context;
        }

        // Apenas administrador e inspetor podem registrar não conformidade
        [HttpPost("criar-nao-conformidade")]
        [Authorize(Roles = "administrador,inspetor")]
        public async Task<IActionResult> CriarNaoConformidade([FromBody] CreateNaoConformidadeDto dto)
        {
            var obraExiste = await _context.Obras.AnyAsync(o => o.Id == dto.ObraId);
            if (!obraExiste)
            {
                return BadRequest(new { Mensagem = $"Obra com ID {dto.ObraId} não encontrada." });
            }

            var novaNc = new NaoConformidade
            {
                ObraId = dto.ObraId,
                PontoIdentificado = dto.PontoIdentificado,
                DescricaoOcorrencia = dto.DescricaoOcorrencia,
                AcaoCorretiva = dto.AcaoCorretiva,
                FotosUrls = dto.FotosUrls,
                Status = dto.Status,
                GrauSeveridade = dto.GrauSeveridade
            };

            _context.NaoConformidades.Add(novaNc);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Não conformidade registrada com sucesso", Id = novaNc.Id });
        }

        // Administrador, inspetor e montador podem visualizar
        [HttpGet("listar-nao-conformidades")]
        [Authorize(Roles = "administrador,inspetor,montador")]
        public async Task<ActionResult<List<NaoConformidadeDto>>> GetNaoConformidades()
        {
            var lista = await _context.NaoConformidades
                .Include(nc => nc.Obra)
                .Select(nc => new NaoConformidadeDto
                {
                    Id = nc.Id,
                    ObraId = nc.ObraId,
                    ObraNome = nc.Obra != null ? nc.Obra.NomeProjeto : string.Empty,
                    PontoIdentificado = nc.PontoIdentificado,
                    DescricaoOcorrencia = nc.DescricaoOcorrencia,
                    AcaoCorretiva = nc.AcaoCorretiva,
                    FotosUrls = nc.FotosUrls,
                    Status = nc.Status,
                    GrauSeveridade = nc.GrauSeveridade
                }).ToListAsync();

            return Ok(lista);
        }

        // Apenas administrador e inspetor podem editar
        [HttpPut("editar-nao-conformidade/{id}")]
        [Authorize(Roles = "administrador,inspetor")]
        public async Task<IActionResult> EditarNaoConformidade(int id, [FromBody] CreateNaoConformidadeDto dto)
        {
            var nc = await _context.NaoConformidades.FindAsync(id);
            if (nc == null) return NotFound(new { Mensagem = "Não conformidade não encontrada" });

            nc.ObraId = dto.ObraId;
            nc.PontoIdentificado = dto.PontoIdentificado;
            nc.DescricaoOcorrencia = dto.DescricaoOcorrencia;
            nc.AcaoCorretiva = dto.AcaoCorretiva;
            nc.FotosUrls = dto.FotosUrls;
            nc.Status = dto.Status;
            nc.GrauSeveridade = dto.GrauSeveridade;

            await _context.SaveChangesAsync();
            return Ok(new { Mensagem = "Não conformidade atualizada com sucesso" });
        }

        [HttpDelete("deletar-nao-conformidade/{id}")]
        [Authorize(Roles = "administrador,inspetor")]
        public async Task<IActionResult> DeletarNaoConformidade(int id)
        {
            var nc = await _context.NaoConformidades.FindAsync(id);
            if (nc == null) return NotFound(new { Mensagem = "Não conformidade não encontrada" });

            _context.NaoConformidades.Remove(nc);
            await _context.SaveChangesAsync();

            return Ok(new { Mensagem = "Não conformidade removida com sucesso" });
        }
    }
}