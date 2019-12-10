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
    public class Medico
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Ci")]
        public String Ci { get; set; }

        [JsonProperty("Nombre")]
        public String Nombres { get; set; }

        [JsonProperty("Apellido")]
        public String Apellidos { get; set; }

        [JsonProperty("Telefono")]
        public String Telefono { get; set; }

        [JsonProperty("Especialidad")]
        public String Especialidad { get; set; }

        [JsonProperty("Codigo")]
        public String Codigo { get; set; }

        [JsonProperty("Password")]
        public String Password { get; set; }

        public Medico(int id, string ci, string nombres, string apellidos, string telefono, string especialidad, string codigo, string password)
        {
            Id = id;
            Ci = ci;
            Nombres = nombres;
            Apellidos = apellidos;
            Telefono = telefono;
            Especialidad = especialidad;
            Codigo = codigo;
            Password = password;
        }
    }
}
