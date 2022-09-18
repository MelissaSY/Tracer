using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracerDll
{
    public class ReadOnlyThreadResult
    {
        readonly public int threadId;
        readonly public long time;
        public IReadOnlyList<ReadOnlyMethodResult> childMethods;
        public ReadOnlyThreadResult(ThreadResult threadResult)
        {
            List<ReadOnlyMethodResult> methods = new List<ReadOnlyMethodResult>();
            foreach (var method in threadResult.childMethods)
            {
                methods.Add(new ReadOnlyMethodResult(method));
            }
            this.childMethods = methods;
            this.threadId = threadResult.threadId;
            this.time = threadResult.time;
        }
    }
}
