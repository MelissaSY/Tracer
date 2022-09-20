using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracerDll
{
    public interface ISerializer
    {
        public string Serialize(TraceResultSerializable traceResult);
    }
}
