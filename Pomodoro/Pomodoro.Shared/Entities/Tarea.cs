using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pomodoro.Shared.Entities
{
    public class Tarea
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "La fecha límite es obligatoria")]
        public DateTime FechaLimite { get; set; }
        public int? ProyectoId { get; set; }
        [JsonIgnore]
        public virtual Proyecto Proyecto { get; set; }
        [JsonIgnore]
        public virtual ICollection<SesionPomodoro> SesionesPomodoro { get; set; } = new List<SesionPomodoro>();
    }
}
