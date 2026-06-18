using Microsoft.AspNetCore.Mvc;
using SalasReunioes.Models;
using SalasReunioes.Services;

namespace SalasReunioes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalasController : ControllerBase
    {
        private readonly SalaService _salaService;

        public SalasController(SalaService salaService)
        {
            _salaService = salaService;
        }

        [HttpGet]
        public IActionResult GetSalas(int pagina = 1, string? busca = null)
        {
            var salas = _salaService.ListarSalas(pagina, busca);
            return Ok(salas);
        }

        [HttpPost]
        public IActionResult PostSala([FromBody] Sala sala)
        {
            var criada = _salaService.CriarSala(sala);
            return Ok(criada);
        }

        [HttpPut("{id}")]
        public IActionResult PutSala(int id, [FromBody] Sala sala)
        {
            var atualizada = _salaService.AtualizarSala(id, sala);
            if (atualizada == null) return NotFound();
            return Ok(atualizada);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSala(int id)
        {
            var removida = _salaService.RemoverSala(id);
            if (!removida) return NotFound();
            return Ok(new { mensagem = "Sala removida" });
        }
    }
}
