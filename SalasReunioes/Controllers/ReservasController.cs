using Microsoft.AspNetCore.Mvc;
using SalasReunioes.Models;
using SalasReunioes.Services;

namespace SalasReunioes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly ReservaService _reservaService;

        public ReservasController(ReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public IActionResult GetReservas()
        {
            var reservas = _reservaService.ListarReservas();
            return Ok(reservas);
        }

        [HttpPost]
        public IActionResult PostReserva([FromBody] Reserva reserva)
        {
            var (criada, erro) = _reservaService.CriarReserva(reserva);
            if (erro != null) return BadRequest(erro);
            return Ok(criada);
        }

        [HttpPut("{id}/reagendar")]
        public IActionResult ReagendarReserva(int id, [FromBody] Reserva novaReserva)
        {
            var (atualizada, erro) = _reservaService.ReagendarReserva(id, novaReserva.Inicio, novaReserva.Fim);
            if (erro == "not_found") return NotFound();
            if (erro != null) return BadRequest(erro);
            return Ok(atualizada);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReserva(int id)
        {
            var erro = _reservaService.CancelarReserva(id);
            if (erro == "not_found") return NotFound();
            if (erro != null) return BadRequest(erro);
            return Ok(new { mensagem = "Reserva cancelada" });
        }

        
        [HttpGet("resumo")]
        public IActionResult GetResumo()
        {
            var total = _reservaService.TotalReservasUltimos7Dias();
            return Ok(new { totalReservasUltimos7Dias = total });
        }

        
        [HttpGet("horas-livres")]
        public IActionResult GetHorasLivres(DateTime inicio, DateTime fim)
        {
            if (fim <= inicio)
                return BadRequest("A data de fim deve ser depois da data de início.");

            var resultado = _reservaService.HorasLivresPorSala(inicio, fim);
            return Ok(resultado);
        }
    }
}
