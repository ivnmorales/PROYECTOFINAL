using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pomodoro.Shared.Entities
{
    public class SesionPomodoro
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser al menos 1 minuto")]
        public int Duracion { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public DateTime FechaFin { get; set; }
        [MaxLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres")]
        public string Estado { get; set; }
        public int? ProyectoId { get; set; }
        [JsonIgnore]
        public virtual Proyecto Proyecto { get; set; }
        public int? TareaId { get; set; }
        [JsonIgnore]
        public virtual Tarea Tarea { get; set; }
        [JsonIgnore]
        public virtual ICollection<TecnicaEstudio> TecnicasEstudio { get; set; } = new List<TecnicaEstudio>();
    }
}
