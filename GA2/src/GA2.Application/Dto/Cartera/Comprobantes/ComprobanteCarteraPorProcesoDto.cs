using GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    public class DateFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(DateTime);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => DateTime.Parse(reader.Value?.ToString()!);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => writer.WriteValue(((DateTime)value).ToString("d/MM/yyyy"));
    }

    public class ComprobanteCarteraPorProcesoDTO
    {
        public int Tipo_Operacion { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Fecha_Comprobante { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Fecha_Transaccion { get; set; }

        public int Comprobante_Id { get; set; }
        public int? Comprobante_Seven_Id { get; set; }
        public int? Consecutivo_Seven_Id { get; set; }
        public string Tipo_Comprobante { get; set; }
        public bool Aprobado { get; set; }
        public string Estado { get; set; }
        public Guid? Detalle_Movimiento_Id { get; set; }
        public decimal Total_Comprobante_Credito { get; set; }
        public decimal Total_Comprobante_Debito { get; set; }

        public string Identificacion { get; set; }
        public List<MovimientoComprobantePorProcesoDto> LstMovimientoComprobante { get; set; } = new List<MovimientoComprobantePorProcesoDto>();
    }
}