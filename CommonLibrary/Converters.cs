using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace CommonLibrary
{
    public class Converters
    {
        public static T DeserializeFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string SerializeToJson<T>(T O)
        {
            return JsonConvert.SerializeObject(O, Newtonsoft.Json.Formatting.Indented);

        }
    }
}
