using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Entidad educativa
    /// </summary>
    public class SedeEntidadEducativa
    {


        public Guid SNE_SEDE_ID { get; set; }
        public Guid ENE_ID { get; set; }
        public string SNE_SEDE_NOMBRE_RAZON_SOCIAL { get; set; }
        public string ENE_CORREO_ELECTRONICO { get; set; }
        public int SNE_SEDE_DPC_ID { get; set; }
        public bool SNE_SEDE_ESTADO { get; set; }
        public int DPC_ID { get; set; }
        public int DPC_CODIGO { get; set; }
        public string DPC_NOMBRE { get; set; }
        public Guid SNE_CREADOPOR { get; set; }
        public DateTime SNE_FECHA_CREACION { get; set; }
        public Guid SNE_MODIFICADOPOR { get; set; }
        public DateTime SNE_FECHA_MODIFICACION { get; set; }

    }

    public class crearAnotacion
    {
        public Guid ENE_ID_FK { get; set; }
        public Guid ENE_CREADOPOR { get; set; }
        public string ENE_ANOTACION { get; set; }
        public DateTime ENE_FECHA_REGISTRO { get; set; }
        
    }

    public class RespuestaAnotacion
    {

        public string ENE_CREADOPOR { get; set; }
        public string ENE_ANOTACION { get; set; }
        public DateTime ENE_FECHA_REGISTRO { get; set; }

    }
}