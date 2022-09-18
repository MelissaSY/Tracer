using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracerDll
{
    public class ReadOnlyMethodResult
    {
        public readonly string methodName;
        public readonly string className;
        public readonly long time;
        public IReadOnlyList<ReadOnlyMethodResult> childMethods;
        public ReadOnlyMethodResult(MethodResult methodResult)
        {
            List<ReadOnlyMethodResult> methods = new List<ReadOnlyMethodResult>();
            foreach (var method in methodResult.childMethods)
            {
                methods.Add(new ReadOnlyMethodResult(method));
            }
            this.childMethods = methods;
            this.methodName = methodResult.methodName;
            this.time = methodResult.time;
            this.className = methodResult.className;
         }
    }
}
