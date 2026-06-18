using Microsoft.EntityFrameworkCore;
using SalasReunioes.Data;
using SalasReunioes.Models;

namespace SalasReunioes.Services
{
    public class SalaService
    {
        private readonly AppDbContext _context;

        public SalaService(AppDbContext context)
        {
            _context = context;
        }

        public List<Sala> ListarSalas(int pagina, string? busca)
        {
            var query = _context.Salas.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
                query = query.Where(s => s.Nome.Contains(busca));

            query = query.OrderBy(s => s.Andar);

            return query
                .Skip((pagina - 1) * 10)
                .Take(10)
                .ToList();
        }

        public Sala CriarSala(Sala sala)
        {
            _context.Salas.Add(sala);
            _context.SaveChanges();
            return sala;
        }

        public Sala? AtualizarSala(int id, Sala dadosNovos)
        {
            var existente = _context.Salas.Find(id);
            if (existente == null) return null;

            existente.Nome = dadosNovos.Nome;
            existente.Andar = dadosNovos.Andar;
            existente.QuantidadeAssentos = dadosNovos.QuantidadeAssentos;

            _context.SaveChanges();
            return existente;
        }

        public bool RemoverSala(int id)
        {
            var sala = _context.Salas.Find(id);
            if (sala == null) return false;

            _context.Salas.Remove(sala);
            _context.SaveChanges();
            return true;
        }
    }
}
