using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Taller.Models
{
    public class Captura
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("IdPaciente")]
        public int IdPaciente { get; set; }

        [JsonProperty("Imagen")]
        public byte[] Imagen { get; set; }

        [JsonProperty("Descripcion")]
        public String Descripcion { get; set; }

        public Captura(int id, int idPaciente, byte[] imagen, string descripcion)
        {
            Id = id;
            IdPaciente = idPaciente;
            Imagen = imagen;
            Descripcion = descripcion;
        }
    }
}
