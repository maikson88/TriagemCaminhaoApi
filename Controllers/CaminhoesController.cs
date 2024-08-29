using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriagemCaminhao.Data;
using TriagemCaminhaoAPI.Data;
using TriagemCaminhaoAPI.Dto;
using TriagemCaminhaoAPI.Enums;

namespace TriagemCaminhaoAPI.Controllers
{
    /// <summary>
    /// Cria Linha Caminhão e Insere Caminhão na tabela fato Triagem
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CaminhoesController : ControllerBase
    {
        private readonly Shinra1Context _context;

        public CaminhoesController(Shinra1Context context)
        {
            _context = context;
        }

        /*
        // GET: api/Caminhoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caminhoes>>> GetCaminhoes()
        {
            return await _context.Caminhoes.ToListAsync();
        }
        */

        
        [HttpPost]
        public async Task<IActionResult> EntradaCaminhao(CaminhoesDto caminhaoDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            int caminhaoId;
            Caminhoes caminhao;

            try
            {
                //Adiciona Caminhão
                caminhao = new Caminhoes
                {
                    NomeTransportadora = caminhaoDto.NomeTransportadora,
                    Whatsapp = caminhaoDto.Whatsapp,
                };

                _context.Caminhoes.Add(caminhao);
                await _context.SaveChangesAsync();

                caminhaoId = caminhao.Id;

                //Adiciona StatusTriagem
                var statusCaminhao = new StatusTriagem
                {
                    Status = StatusCaminhaoEnum.Aguardando.ToString(),
                };

                _context.StatusTriagems.Add(statusCaminhao);
                await _context.SaveChangesAsync();

                //Adiciona PrioridadeTriagem
                var prioridadeTriagem = new PrioridadeTriagem
                {
                    IsPrioridade = false
                };

                _context.PrioridadeTriagems.Add(prioridadeTriagem);
                await _context.SaveChangesAsync();


                //Adiciona a Triagem
                var triagem = new Triagem
                {
                    CaminhaoId = caminhao.Id,
                    StatusId = statusCaminhao.Id,
                    PrioridadeID = prioridadeTriagem.Id,
                    DataChegada = DateTime.Now
                };

                _context.Triagems.Add(triagem);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync(); // Confirma a transação

                return Ok(new { message = "Caminhão criado com sucesso" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Reverte a transação em caso de erro
                return BadRequest(new { message = "Erro ao criar o caminhão", details = ex.Message });
            }
        }
        
        /*
        // GET: api/Caminhoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Caminhoes>> GetCaminhoes(int id)
        {
            var caminhoes = await _context.Caminhoes.FindAsync(id);

            if (caminhoes == null)
            {
                return NotFound();
            }

            return caminhoes;
        }
        */


        /*
        // PUT: api/Caminhoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaminhoes(int id, Caminhoes caminhoes)
        {
            if (id != caminhoes.Id)
            {
                return BadRequest();
            }

            _context.Entry(caminhoes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaminhoesExists(id))
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
        */

        /*
        // POST: api/Caminhoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Caminhoes>> PostCaminhoes(Caminhoes caminhoes)
        {
            _context.Caminhoes.Add(caminhoes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaminhoes", new { id = caminhoes.Id }, caminhoes);
        }
        */

        /*
        // DELETE: api/Caminhoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaminhoes(int id)
        {
            var caminhoes = await _context.Caminhoes.FindAsync(id);
            if (caminhoes == null)
            {
                return NotFound();
            }

            _context.Caminhoes.Remove(caminhoes);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool CaminhoesExists(int id)
        {
            return _context.Caminhoes.Any(e => e.Id == id);
        }
    }
}
