using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pomodoro.Shared.Entities
{
    public class Proyecto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del proyecto es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre del proyecto no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La descripción del proyecto es obligatoria")]
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        [JsonIgnore]
        public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
        [JsonIgnore]
        public virtual ICollection<SesionPomodoro> SesionesPomodoro { get; set; } = new List<SesionPomodoro>();
        [JsonIgnore]
        public virtual ICollection<Recompensa> Recompensas { get; set; } = new List<Recompensa>();
        [JsonIgnore]
        public virtual ICollection<HistorialSesion> HistorialSesiones { get; set; } = new List<HistorialSesion>();
    }
}
