using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TracerDll
{
    public class ThreadResultSerializable
    {
        [JsonPropertyName("id")]
        [XmlAttribute("id")]
        public int ThreadId { get; set; }

        [JsonPropertyName("time")]
        [XmlAttribute("time")]
        public string? Time { get; set; }

        [JsonPropertyName("methods")]
        [XmlElement(ElementName = "method")]
        public MethodResultSerializable[]? Methods { get; set; }

        public ThreadResultSerializable() { }
        public ThreadResultSerializable(ReadOnlyThreadResult thread)
        {
            ThreadId = thread.threadId;
            Time = string.Concat(thread.time.ToString(), "ms");
            int childMethods = thread.childMethods.Count;
            if (childMethods > 0)
            {
                Methods = new MethodResultSerializable[childMethods];
                for (int i = 0; i < thread.childMethods.Count; i++)
                {
                    Methods[i] = new MethodResultSerializable(thread.childMethods[i]);
                }
            }
        }
    }
}

