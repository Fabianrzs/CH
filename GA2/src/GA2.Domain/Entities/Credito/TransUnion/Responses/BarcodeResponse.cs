using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito.TransUnion.Responses
{
    public class Data
    {
        public FormInsertResult form_InsertResult { get; set; }
    }

    public class FormInsertResult
    {
        public object barCode { get; set; }
        public object fileCodes { get; set; }
        public string message { get; set; }
        public int recordId { get; set; }
        public bool success { get; set; }
        public int workflowId { get; set; }
    }
}
