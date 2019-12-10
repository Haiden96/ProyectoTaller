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
    public class Historial
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("IdPaciente")]
        public int IdPaciente { get; set; }
        [JsonProperty("FechaReg")]
        public DateTime FechaReg { get; set; }
        [JsonProperty("Detalle")]
        public String Detalle { get; set; }

        public Historial(int id, int idPaciente, DateTime fechaReg, string detalle)
        {
            Id = id;
            IdPaciente = idPaciente;
            FechaReg = fechaReg;
            Detalle = detalle;
        }
    }
}
