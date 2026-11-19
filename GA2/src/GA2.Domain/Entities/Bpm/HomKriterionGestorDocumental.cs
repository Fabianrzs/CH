namespace GA2.Domain.Entities.Bpm
{
    using System;
    using System.Collections.Generic;
    public class HomKriterionGestorDocumental
    {
        public Guid PCS_ID_KRITERION { get; set; }
        public Guid TPS_SUB_ID_KRITERION { get; set; }
        public Guid PRM_ID_TIPO_SOLICITUD { get; set; }
        public string PCS_ID_GESTOR_DOC { get; set; }
        public string PCS_NOMBRE_GESTOR_DOC { get; set; }
        public string TPS_SUB_ID_GESTOR_DOC { get; set; }
        public string TPS_SUB_NOMBRE_GESTOR_DOC { get; set; }
    }
}
