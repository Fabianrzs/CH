using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class ResultadoComparacionArchivoAplicacionPagoCSVDto
    {
        public IEnumerable<ArchivoAplicacionPagoCSVComparacionDto> ListaCoincidente { get; set; }
        public IEnumerable<ArchivoAplicacionPagoCSVComparacionDto> ListaNoCoincidente { get; set; }

        public ResultadoComparacionArchivoAplicacionPagoCSVDto()
        {
            
        }

        public ResultadoComparacionArchivoAplicacionPagoCSVDto(IEnumerable<ArchivoAplicacionPagoCSVComparacionDto> listaCoincidente,
            IEnumerable<ArchivoAplicacionPagoCSVComparacionDto> listaNoCoincidente)
        {
            ListaCoincidente = listaCoincidente;
            ListaNoCoincidente = listaNoCoincidente;
        }
    }
}
