namespace GA2.Domain.Entities.Proyecto
{
    public class AdjudicacionVivienda
    {
        public AdjudicacionProyecto adjudicacion { get; set; }
        public LogisticaAdjudicacion logistica { get; set; }
        public EntregaViviendaAdjudicacion EntregaVivienda { get; set; }
    }
}
