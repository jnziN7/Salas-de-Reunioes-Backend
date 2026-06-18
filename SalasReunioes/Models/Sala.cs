using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalasReunioes.Models
{
    public class Sala
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = "";

        [Required]
        public int Andar { get; set; }

        [Required]
        public int QuantidadeAssentos { get; set; }

        [JsonIgnore]
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
