﻿namespace TallerMecanico.Dtos
{
    public class RegistroDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}