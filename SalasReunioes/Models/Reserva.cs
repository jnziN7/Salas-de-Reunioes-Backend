using System.ComponentModel.DataAnnotations;

namespace SalasReunioes.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fim { get; set; }

        [Required]
        public int SalaId { get; set; }

        public Sala? Sala { get; set; }
    }
}
