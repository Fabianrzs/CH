using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Extractos
{
    public class ReportFondoSolidaridadRequetsDto
    {
        public ReportModelFondoSolidaria? ReportModel { get; set; } = null;
        public ReportStatusFondoSolidaridad? ReportStatus { get; set; } = null;
    }
}
