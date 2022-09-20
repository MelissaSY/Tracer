using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace TracerDll
{
    public class XmlTraceResult: ISerializer
    {
        public string Serialize(TraceResult traceReadonlyResult)
        {
            string result;
            TraceResultSerializable traceResult = new TraceResultSerializable(traceReadonlyResult); 
            XmlSerializer serializer = new XmlSerializer(traceResult.GetType());
            using(StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, traceResult);
                result = XElement.Parse(sw.ToString()).ToString();
            }
            return result;
        }
    }
}
