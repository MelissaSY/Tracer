using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TracerDll
{
    public class JsonTraceResult: ISerializer
    {
        public string Serialize(TraceResultSerializable traceResult)
        {
            string result;
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
            result = JsonSerializer.Serialize(traceResult, options);
            return result;
        }
    }
}
