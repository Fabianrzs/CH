using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class EntrevistaDto
    {
        public Guid? EFS_ID { get; set; } = null;
        public Guid EFS_ID_SOLICITUD { get; set; }
        public string? EFS_VIVIENDA_PROPIA { get; set; }
        public string? EFS_TIPO_VIVIENDA { get; set; }
        public int? EFS_HABITANTE { get; set; }
        public string? EFS_INFORMATIVO_ADMIN { get; set; }
        public string? EFS_CONCEPTO_PSICOSOCIAL { get; set; }
        public string? EFS_CARCTERIZACION { get; set; }
        public string? EFS_OBSERVACION { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<PostulanteDto> postulantes { get; set; }

    }
}
