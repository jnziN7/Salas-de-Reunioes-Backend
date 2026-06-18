using Microsoft.EntityFrameworkCore;
using SalasReunioes.Data;
using SalasReunioes.Models;

namespace SalasReunioes.Services
{
    public class ReservaService
    {
        private readonly AppDbContext _context;

        public ReservaService(AppDbContext context)
        {
            _context = context;
        }

        public List<object> ListarReservas()
        {
            var reservas = _context.Reservas
                .Include(r => r.Sala)
                .Select(r => (object)new
                {
                    r.Id,
                    r.Inicio,
                    r.Fim,
                    r.SalaId,
                    Sala = new
                    {
                        r.Sala!.Id,
                        r.Sala.Nome,
                        r.Sala.Andar,
                        r.Sala.QuantidadeAssentos
                    }
                })
                .ToList();

            return reservas;
        }

        
        public (Reserva? reserva, string? erro) CriarReserva(Reserva reserva)
        {
            if (reserva.Inicio.TimeOfDay < new TimeSpan(8, 0, 0) || reserva.Fim.TimeOfDay > new TimeSpan(19, 0, 0))
                return (null, "Horário inválido. Permitido apenas entre 08:00 e 19:00.");

            if (reserva.Fim <= reserva.Inicio)
                return (null, "O horário de fim deve ser depois do início.");

            var conflito = _context.Reservas.Any(r =>
                r.SalaId == reserva.SalaId &&
                r.Inicio < reserva.Fim &&
                r.Fim > reserva.Inicio);

            if (conflito)
                return (null, "Sala já reservada neste horário.");

            _context.Reservas.Add(reserva);
            _context.SaveChanges();
            return (reserva, null);
        }

        public string? CancelarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null) return "not_found";

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();
            return null;
        }

        public (Reserva? reserva, string? erro) ReagendarReserva(int id, DateTime novoInicio, DateTime novoFim)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null) return (null, "not_found");

            if (reserva.Inicio <= DateTime.Now)
                return (null, "Só é possível reagendar reservas futuras.");

            if (novoInicio.TimeOfDay < new TimeSpan(8, 0, 0) || novoFim.TimeOfDay > new TimeSpan(19, 0, 0))
                return (null, "Horário inválido. Permitido apenas entre 08:00 e 19:00.");

            if (novoFim <= novoInicio)
                return (null, "O horário de fim deve ser depois do início.");

            var conflito = _context.Reservas.Any(r =>
                r.SalaId == reserva.SalaId &&
                r.Id != id &&
                r.Inicio < novoFim &&
                r.Fim > novoInicio);

            if (conflito)
                return (null, "Sala já reservada neste horário.");

            reserva.Inicio = novoInicio;
            reserva.Fim = novoFim;
            _context.SaveChanges();
            return (reserva, null);
        }

        
        public int TotalReservasUltimos7Dias()
        {
            var limite = DateTime.Now.AddDays(-7);
            return _context.Reservas.Count(r => r.Inicio >= limite);
        }

        
        public List<object> HorasLivresPorSala(DateTime inicio, DateTime fim)
        {
            var salas = _context.Salas.ToList();
            var reservas = _context.Reservas
                .Where(r => r.Inicio < fim && r.Fim > inicio)
                .ToList();

            
            int totalDias = (int)(fim.Date - inicio.Date).TotalDays + 1;
            double horasDisponiveisPorDia = 11;
            double totalHorasDisponiveis = totalDias * horasDisponiveisPorDia;

            var resultado = salas.Select(sala =>
            {
                var reservasDaSala = reservas.Where(r => r.SalaId == sala.Id);

                double horasOcupadas = reservasDaSala
                    .Sum(r => (r.Fim - r.Inicio).TotalHours);

                double horasLivres = Math.Max(0, totalHorasDisponiveis - horasOcupadas);

                return (object)new
                {
                    SalaId = sala.Id,
                    NomeSala = sala.Nome,
                    Andar = sala.Andar,
                    HorasLivres = Math.Round(horasLivres, 2)
                };
            }).ToList();

            return resultado;
        }
    }
}
