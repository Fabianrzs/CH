using Microsoft.AspNetCore.Http;

namespace GA2.Application.Dto
{
    public class CargaNominaDTO
    {
        public IFormFile File { get; set; }
        //public int? Frz_Id { get; set; }
        public int? Uej_Id { get; set; }
        public int? Tdc_Id { get; set; }
        public int? Esd_Id { get; set; }
        public int? Dct_Id { get; set; }
        public string Dct_FechaCreacion { get; set; }
        public string Dct_Nombre { get; set; }

        public bool enviadoga2app { get; set; }
    }
}
