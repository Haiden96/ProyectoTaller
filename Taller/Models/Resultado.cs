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

    public partial class Resultado
    {
        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("images_processed")]
        public long ImagesProcessed { get; set; }

        [JsonProperty("custom_classes")]
        public long CustomClasses { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("classifiers")]
        public List<Classifier> Classifiers { get; set; }

        [JsonProperty("source_url")]
        public Uri SourceUrl { get; set; }

        [JsonProperty("resolved_url")]
        public Uri ResolvedUrl { get; set; }
    }

    public partial class Classifier
    {
        [JsonProperty("classifier_id")]
        public string ClassifierId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("classes")]
        public List<Class> Classes { get; set; }
    }

    public partial class Class
    {
        [JsonProperty("class")]
        public string ClassClass { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public partial class Resultado
    {
        public static Resultado FromJson(string json) => JsonConvert.DeserializeObject<Resultado>(json, Taller.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Resultado self) => JsonConvert.SerializeObject(self, Taller.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
