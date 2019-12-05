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
    public class Persona
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Nombres")]
        public String Nombres { get; set; }
        [JsonProperty("Apellidos")]
        public String Apellidos { get; set; }
        [JsonProperty("Ci")]
        public String Ci { get; set; }
        [JsonProperty("Telefono")]
        public String Telefono { get; set; }
        [JsonProperty("Codigo")]
        public String Codigo { get; set; }
    }
}
