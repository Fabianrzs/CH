using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.TusDatos.Models
{
    public class VigiaResponse
    {
        [JsonProperty("identification")]
        public string Identification { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("riskLAFT")]
        public string RiskLAFT { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType { get; set; }
    }
}
