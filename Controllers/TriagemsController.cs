using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriagemCaminhao.Data;
using TriagemCaminhaoAPI.Dto;
using TriagemCaminhaoAPI.Enums;

namespace TriagemCaminhaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriagemsController : ControllerBase
    {
        private readonly Shinra1Context _context;

        public TriagemsController(Shinra1Context context)
        {
            _context = context;
        }

        // GET: api/Triagems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TriagemDto>>> GetTriagems()
        {
            var triagens = await _context.Triagems
                .Include(t => t.Caminhao)
                .Include(t => t.Doca)
                .Include(t => t.StatusTriagem)
                .Include(t => t.PrioridadeTriagem)
                .ToListAsync();

            var triagemDtos = triagens.Select(triagem => new TriagemDto
            {
                Id = triagem.Id,
                Caminhao = new CaminhoesDto
                {
                    NomeTransportadora = triagem.Caminhao.NomeTransportadora,
                    Whatsapp = triagem.Caminhao.Whatsapp,
                    Mensagem = triagem.Caminhao.Mensagem // Se precisar adicionar uma mensagem
                },
                Doca = triagem.Doca != null ? new DocaDto
                {
                    NomeDoca = triagem.Doca.NomeDoca,
                    StatusDoca = triagem.Doca.StatusDoca
                } : null,
                StatusTriagem = new StatusTriagemDto
                {
                    Status = triagem.StatusTriagem.Status
                },
                PrioridadeTriagem = new PrioridadeTriagemDto
                {
                    IsPrioridade = (bool) triagem.PrioridadeTriagem.IsPrioridade,
                    Volume = triagem.PrioridadeTriagem.Volume,
                    Peso = triagem.PrioridadeTriagem.Peso,
                },
                DataChegada = triagem.DataChegada,
                DataAtendimento = triagem.DataAtendimento
            }).ToList();

            return Ok(triagemDtos);
        }


        // GET: api/Triagems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TriagemDto>> GetTriagem(int id)
        {
            var triagem = await _context.Triagems
                .Include(t => t.Caminhao)
                .Include(t => t.Doca)
                .Include(t => t.StatusTriagem)
                .Include(t => t.PrioridadeTriagem)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (triagem == null)
            {
                return NotFound();
            }

            var triagemDto = new TriagemDto
            {
                Id = triagem.Id,
                Caminhao = new CaminhoesDto
                {
                    NomeTransportadora = triagem.Caminhao.NomeTransportadora,
                    Whatsapp = triagem.Caminhao.Whatsapp,
                    Mensagem = triagem.Caminhao.Mensagem // Se precisar adicionar uma mensagem
                },
                Doca = triagem.Doca != null ? new DocaDto
                {
                    NomeDoca = triagem.Doca.NomeDoca,
                    StatusDoca = triagem.Doca.StatusDoca
                } : null,
                StatusTriagem = new StatusTriagemDto
                {
                    Status = triagem.StatusTriagem.Status
                },
                PrioridadeTriagem = new PrioridadeTriagemDto
                {
                    IsPrioridade = (bool)triagem.PrioridadeTriagem.IsPrioridade,
                    Volume = triagem.PrioridadeTriagem.Volume,
                    Peso = triagem.PrioridadeTriagem.Peso,
                },
                DataChegada = triagem.DataChegada,
                DataAtendimento = triagem.DataAtendimento
            };

            return Ok(triagemDto);
        }

        /// <summary>
        /// Atualiza Status para "Atendendo"
        /// </summary>
        // PUT: api/Triagems/atendimento/5
        [HttpPut("atendimento/{id}")]
        public async Task<IActionResult> PutAtendendoTriagem(int id, int docaId)
        {
            // Buscar a triagem existente no banco de dados
            var existingTriagem = await _context.Triagems
                .Include(t => t.StatusTriagem) // Incluir a entidade Caminhao para atualizar o status
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTriagem == null)
            {
                return NotFound();
            }

            // Atualizar a doca e a data de atendimento
            existingTriagem.DocaId = docaId;
            existingTriagem.DataAtendimento = DateTime.Now;

            // Atualizar o status do caminhão para "Atendido"
            existingTriagem.StatusTriagem.Status = StatusCaminhaoEnum.Atendendo.ToString();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TriagemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Define Triagem como "Prioridade"
        /// </summary>
        // PUT: api/Triagems/prioridade/5
        [HttpPut("prioridade/{id}")]
        public async Task<IActionResult> PutPrioridadeTriagem(int id, string volume, string peso)
        {
            // Buscar a triagem existente no banco de dados
            var existingTriagem = await _context.Triagems
                .Include(t => t.PrioridadeTriagem) // Incluir a entidade PrioridadeTriagem
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTriagem == null)
            {
                return NotFound();
            }

            // Atualizar a prioridade, volume e peso
            existingTriagem.PrioridadeTriagem.IsPrioridade = true;
            existingTriagem.PrioridadeTriagem.Volume = volume;
            existingTriagem.PrioridadeTriagem.Peso = peso;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TriagemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }





        /*
        // POST: api/Triagems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Triagem>> PostTriagem(Triagem triagem)
        {
            _context.Triagems.Add(triagem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTriagem", new { id = triagem.Id }, triagem);
        }
        */


        // DELETE: api/Triagems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTriagem(int id)
        {
            var triagem = await _context.Triagems.FindAsync(id);
            if (triagem == null)
            {
                return NotFound();
            }

            _context.Triagems.Remove(triagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TriagemExists(int id)
        {
            return _context.Triagems.Any(e => e.Id == id);
        }
    }
}
