namespace GA2.Domain.Entities
{
    public class FieldChange
    {
        public string Field { get; set; }     // Nombre del campo
        public string OldValue { get; set; }  // Valor anterior (antes de la actualización)
        public string NewValue { get; set; }  // Valor nuevo (después de la actualización)
    }


}
