using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TracerDll
{
    public class MethodResultSerializable
    {
        [JsonPropertyName("name")]
        [XmlAttribute("name")]
        public string? MethodName { get; set; }

        [JsonPropertyName("class")]
        [XmlAttribute("class")]
        public string? ClassName { get; set; }

        [JsonPropertyName("time")]
        [XmlAttribute("time")]
        public string? Time { get; set; }

        [JsonPropertyName("methods")]
        [XmlElement(ElementName = "method")]
        public MethodResultSerializable[]? Methods { get; set; }
        public MethodResultSerializable() { }
        public MethodResultSerializable(ReadOnlyMethodResult method)
        {
            MethodName = method.methodName;
            ClassName = method.className;
            Time = string.Concat(method.time.ToString(), "ms");

            int childMethods = method.childMethods.Count;
            if (childMethods > 0)
            {
                this.Methods = new MethodResultSerializable[method.childMethods.Count];
                for (int i = 0; i < method.childMethods.Count; i++)
                {
                    this.Methods[i] = new MethodResultSerializable(method.childMethods[i]);
                }
            }
        }
    }
}
