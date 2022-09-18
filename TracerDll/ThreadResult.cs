using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TracerDll
{
    public class ThreadResult
    {
        public int threadId;
        public long time;
        public ConcurrentBag<MethodResult> childMethods;
        public ThreadResult(int threadId)
        {
            this.threadId = threadId;
            this.childMethods = new ConcurrentBag<MethodResult>();
        }
    }
}
