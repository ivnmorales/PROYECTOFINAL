using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Shared.Entities
{
    public class HistorialSesion
    {
        public int Id { get; set; }
        public int? ProyectoId { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public DateTime Fecha { get; set; }
        public int Duracion { get; set; }
        public int? SesionId { get; set; }
        public virtual SesionPomodoro SesionPomodoro { get; set; }
    }
}
