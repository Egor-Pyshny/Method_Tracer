using Tracer_Formating.Tracer;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Tracer_Formating.Utils.Serialization
{
    public class XML : ISerialize
    {
        public string Serialize(object? obj)
        {
            Type t = obj!.GetType();
            var sb = new StringBuilder();
            if (t.Name == "TracerResult")
            {
                var dcs = new DataContractSerializer(typeof(TracerResult));
                
                var writerSettings = new XmlWriterSettings()
                {
                    Indent = true,
                };
                using (var writer = XmlWriter.Create(sb, writerSettings))
                {

                    dcs.WriteObject(writer, obj);
                }
            }
            return sb.ToString();
        }
    }
}
