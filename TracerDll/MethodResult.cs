using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TracerDll
{
    public class MethodResult
    {
        public string methodName;
        public string className;
        public long time;
        public ConcurrentQueue<MethodResult> childMethods;
        public MethodResult(string methodName, string className)
        {
            this.methodName = methodName;
            this.className = className;
            this.childMethods = new ConcurrentQueue<MethodResult>();
        }
        public void AddChild(MethodResult method)
        {
            childMethods.Enqueue(method);
        }
    }
}
