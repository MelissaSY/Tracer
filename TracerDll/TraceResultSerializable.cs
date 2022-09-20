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
    [XmlRoot("root")]
    public class TraceResultSerializable
    {

        [JsonPropertyName("threads")]
        [XmlElement(ElementName = "thread")]
        public ThreadResultSerializable[]? Result { get; set; }
        public TraceResultSerializable() { }
        public TraceResultSerializable(TraceResult result) 
        {
            int threads = result.result.Count;
            if (threads > 0)
            {
                this.Result = new ThreadResultSerializable[result.result.Count];
                for (int i = 0; i < result.result.Count; i++)
                {
                    this.Result[i] = new ThreadResultSerializable(result.result[i]);
                }
            }
        }
    }
}
