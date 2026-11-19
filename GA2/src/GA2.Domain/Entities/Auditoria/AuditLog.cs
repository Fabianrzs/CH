using System;

namespace GA2.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; } // Tabla afectada
        public string RecordId { get; set; } // ID del registro
        public int TranstationId { get; set; } // Tipo de operación: CREATE, UPDATE, DELETE
        public string ChangedFields { get; set; } // Campos modificados en formato JSON
        public string UserName { get; set; } // Usuario que realiza la operación
        public DateTime Timestamp { get; set; } // Fecha y hora de la operación
    }


}
