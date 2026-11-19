using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito.Simulador.Documento
{
    public enum EnumDocumentoSimulacionParams
    {
        [Description("@DSC_ID")]
        idDocumentoSimulacion,

        [Description("@SLS_ID")]
        idSimulacion,

        [Description("@TDC_ID")]
        idTipoDocumento,

        [Description("@DSC_CODIGO_BARRAS")]
        codigoBarras,

        [Description("@DSC_FECHA_DOCUMENTO")]
        fechaDocumento,

        [Description("@DSC_NUMERO_FOLIOS")]
        numeroFolios,

        [Description("@DSC_TAMAÑO")]
        tamaño,

        [Description("@DSC_RUTA_IMAGEN")]
        rutaImagen,

        [Description("@DSC_ESTADO")]
        estado,

        [Description("@DSC_CREADO_POR")]
        creadoPor,

        [Description("@DSC_FECHA_CREACION")]
        fechaCreacion,

        [Description("@IND")]
        Indicador
    }
}
