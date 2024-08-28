using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriagemCaminhao.Data;
using TriagemCaminhaoAPI.Dto;

namespace TriagemCaminhaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocasController : ControllerBase
    {
        private readonly Shinra1Context _context;

        public DocasController(Shinra1Context context)
        {
            _context = context;
        }

        // GET: api/Docas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocaDto>>> GetDocas()
        {
            // Busca as docas do banco de dados
            var docas = await _context.Docas.ToListAsync();

            // Mapeia cada Doca para DocaDto
            var docaDtos = docas.Select(d => new DocaDto
            {
                Id = d.Id,
                NomeDoca = d.NomeDoca,
                StatusDoca = d.StatusDoca
            }).ToList();

            // Retorna a lista de DocaDto
            return Ok(docaDtos);
        }

        /*
        // GET: api/Docas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doca>> GetDoca(int id)
        {
            var doca = await _context.Docas.FindAsync(id);

            if (doca == null)
            {
                return NotFound();
            }

            return doca;
        }
        */

        // PUT: api/Docas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoca(int id, DocaDto docaDto)
        {
            // Verifica se o ID da URL corresponde ao ID do DTO
            if (id != docaDto.Id)
            {
                return BadRequest();
            }

            // Busca a Doca existente no banco de dados
            var existingDoca = await _context.Docas.FindAsync(id);
            if (existingDoca == null)
            {
                return NotFound();
            }

            // Atualiza os valores da Doca existente com os valores do DTO
            existingDoca.NomeDoca = docaDto.NomeDoca;
            existingDoca.StatusDoca = docaDto.StatusDoca;

            // Marca a entidade como modificada
            _context.Entry(existingDoca).State = EntityState.Modified;

            try
            {
                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retorna um status 204 No Content
            return NoContent();
        }


        // POST: api/Docas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DocaDto>> PostDoca(DocaDto docaDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            int docaId;
            Doca doca;

            try
            {
                // Criar uma nova entidade Caminhao
                doca = new Doca
                {
                    NomeDoca = docaDto.NomeDoca,
                    StatusDoca = docaDto.StatusDoca,
                };

                // Adicionar o caminhão à tabela Caminhoes
                _context.Docas.Add(doca);
                await _context.SaveChangesAsync();

                docaId = doca.Id;

                await transaction.CommitAsync(); // Confirma a transação
            }
            catch 
            {
                await transaction.RollbackAsync(); // Reverte a transação em caso de erro
                throw;
            }

            return CreatedAtAction("GetDoca", new { id = doca.Id }, doca);
        }

        // DELETE: api/Docas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoca(int id)
        {
            var doca = await _context.Docas.FindAsync(id);
            if (doca == null)
            {
                return NotFound();
            }

            _context.Docas.Remove(doca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocaExists(int id)
        {
            return _context.Docas.Any(e => e.Id == id);
        }
    }
}
