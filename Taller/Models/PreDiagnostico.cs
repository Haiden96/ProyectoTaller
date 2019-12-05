using System;
using System.Collections.Generic;
using System.Text;

namespace Taller.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    public class PreDiagnostico
    {

        [JsonProperty("Id")]
        public int Id { get; set; }
        
        [JsonProperty("IdCaptura")]
        public int IdCaptura { get; set; }
        
        [JsonProperty("IdHistorial")]
        public int IdHistorial { get; set; }
        
        [JsonProperty("Resultado")]
        public String Resultado { get; set; }
        
        [JsonProperty("Probabilidad")]
        public Double Probabilidad { get; set; }
        
        [JsonProperty("Glosa")]
        public String Glosa { get; set; }
        
        [JsonProperty("FechaReg")]
        public DateTime FechaReg { get; set; }

        [JsonProperty("EstadoVerificacion")]
        public int EstadoVerificacion { get; set; }
    }

}
