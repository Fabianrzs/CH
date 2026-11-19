using System;
using System.ComponentModel.DataAnnotations;

namespace GA2.Domain.Entities
{
    public class SLFZUN_FUERZA_UNIDAD_USUARIO
    {
        [Key]
        public int SLFZUN_ID { get; set; }
        public int FRZ_ID { get; set; }
        public int UEJ_ID { get; set; }
        public Guid USR_ID { get; set; }        
    }
}
