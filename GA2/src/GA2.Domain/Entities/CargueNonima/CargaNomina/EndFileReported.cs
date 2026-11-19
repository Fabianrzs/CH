

namespace GA2.Domain.Entities
{
    public class EndFileReported
    {
        public int CONSECUTIVE { get; set; }
        public string FILE_CONTROL_INDICATOR { get; set; }
        public decimal SUM_CONTRIBUTIONS { get; set; }
        public int END_FILE_CODE { get; set; }
    }
}
