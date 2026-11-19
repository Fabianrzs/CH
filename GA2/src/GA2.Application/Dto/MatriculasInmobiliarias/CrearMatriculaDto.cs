using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class CrearMatriculaDto
    {
        public string NumeroMatricula { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public string Direccion { get; set; }
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string RazonSocial { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public int IdOficinaRegistro { get; set; }
    }
    public class CrearMatriculRespondeDto
    {
        public Guid MAI_ID { get; set; }
        public Guid PRP_ID { get; set; }
        public DateTime MAI_CREATEDOPOR { get; set; }
    }
}
