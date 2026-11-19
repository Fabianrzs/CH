using System;

public class BeneficiarioProyectoDto
{
    public int BP_TIPO_DOCUMENTO { get; set; }
    public string BP_NUMERO_DOCUMENTO { get; set; }
    public int BP_DIGITO_VEFICACION { get; set; }
    public string BP_RAZON_SOCIAL { get; set; }
    public int BP_TIPO_CUENTA { get; set; }
    public string BP_TIPO_ENTIDAD { get; set; }
    public string BP_ACTIVIDAD_CUENTA { get; set; }
    public string BP_DIRECCION { get; set; }
    public string BP_NUMERO_CUENTA { get; set; }
    public Guid? PC_PROYECTO_ID { get; set; } = Guid.Empty;

}
