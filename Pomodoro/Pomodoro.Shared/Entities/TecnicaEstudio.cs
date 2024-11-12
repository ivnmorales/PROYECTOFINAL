using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Shared.Entities
{
    public class TecnicaEstudio
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la técnica es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Beneficios { get; set; }
        public virtual ICollection<SesionPomodoro> SesionesPomodoro { get; set; }
    }
}
