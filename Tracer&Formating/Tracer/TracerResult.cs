using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Tracer_Formating.Tracer
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TracerResult : IXmlSerializable
    {
        [JsonProperty("Threads")]
        internal List<ThreadNode> _threadNodes;
        
        public IReadOnlyList<ThreadNode> threadNodes => _threadNodes;

        public TracerResult() => _threadNodes = [];

        public TracerResult(List<ThreadNode> thNodes) => _threadNodes = thNodes;

        public XmlSchema? GetSchema() { return null; }
        public void ReadXml(XmlReader reader) { }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var thread in threadNodes)
            {
                thread.WriteXml(writer);
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MethodNode : IXmlSerializable
    {
        [JsonProperty("Method_name", Order = 0)]
        public string? _name;

        internal string? __sysName;

        [JsonProperty("Method_duration", Order = 2)]
        public long duration;

        [JsonProperty("Method_class", Order = 1)]
        public string? _class;

        [XmlIgnore]
        internal bool isSystem;

        [XmlIgnore]
        internal bool closed = false;

        [XmlIgnore]
        internal MethodNode? parent;

        [XmlIgnore]
        internal Stopwatch? stopwatch;

        [JsonProperty("Nested_methods", Order = 3)]
        public List<MethodNode> methods;

        public MethodNode() => methods = []; 

        public MethodNode(string? _name, string? _class, bool isSystem = false, string? sysName = null) {
            this._name = _name; 
            this._class = _class;
            this.stopwatch = new Stopwatch();
            this.isSystem = isSystem;
            this.__sysName = sysName;      
            methods = [];
        }

        public XmlSchema? GetSchema() { return null; }
        public void ReadXml(XmlReader reader) { }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("MethodNode");
            writer.WriteAttributeString("Method_name", _name!.ToString());
            writer.WriteAttributeString("Method_duration", duration.ToString());
            writer.WriteAttributeString("Method_class", _class!.ToString());
            foreach (var method in methods)
            {
                method.WriteXml(writer);
            }
            writer.WriteEndElement();
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ThreadNode : IXmlSerializable
    {
        [JsonProperty("Thread_id")]
        public int tid;

        [JsonProperty("Thread_duration")]
        public long duration;

        [JsonProperty("Nested_methods")]
        public List<MethodNode> methods;

        public ThreadNode() => methods = [];

        public XmlSchema? GetSchema() { return null; }
        public void ReadXml(XmlReader reader) { }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("ThreadNode");
            writer.WriteAttributeString("Thread_id", tid.ToString());
            writer.WriteAttributeString("Thread_duration", duration.ToString());
            foreach (var method in methods) {
                method.WriteXml(writer);
            }
            writer.WriteEndElement();
        }
    }
}
