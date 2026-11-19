namespace GA2.Application.Dto
{
    public class DocumentoIdCuenta
    {
        public int idCuenta { get; set; }
    }

    public class ConsultaConceptos
    {
        public int clienteId { get; set; }

        public int cuentaId { get; set; }

        public int conceptoId { get; set; }
    }
}
