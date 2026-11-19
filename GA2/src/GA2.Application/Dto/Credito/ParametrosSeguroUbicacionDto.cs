using GA2.Domain.Entities;
using System;
using System.Runtime.Intrinsics.Arm;

namespace GA2.Application.Dto
{
    public class ParametrosSeguroUbicacionDto
    {
        public int Id { get; set; }
        public string SeguroTodoRiesgo { get; set; }
        public string PdpId { get; set; }
        public int Dpcid { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int ModificadoPor { get; set; }
        public int SeguroEstado { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
    }
}


	