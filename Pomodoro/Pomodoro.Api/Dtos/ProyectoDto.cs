﻿namespace Pomodoro.API.Dtos
{
    public class ProyectoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CrearProyectoDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class ActualizarProyectoDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
