using Microsoft.EntityFrameworkCore;
using SalasReunioes.Models;

namespace SalasReunioes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sala> Salas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
    }
}
