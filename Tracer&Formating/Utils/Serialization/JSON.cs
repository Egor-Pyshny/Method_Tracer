using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace Tracer_Formating.Utils.Serialization
{
    public class JSON : ISerialize
    {
        public string Serialize(object? obj)
        {
            string js = JsonConvert.SerializeObject(obj!);
            return JToken.Parse(js).ToString();
        }
    }
}
