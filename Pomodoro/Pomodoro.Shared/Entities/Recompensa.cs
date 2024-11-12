using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Shared.Entities
{
    public class Recompensa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la recompensa es obligatorio")]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? ProyectoId { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
