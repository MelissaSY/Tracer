using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TracerDll
{
    public class TraceResult
    {
        public IReadOnlyList<ReadOnlyThreadResult> result;
        public TraceResult(ConcurrentDictionary<int, ThreadResult> threadResult)
        {
            List<ReadOnlyThreadResult> threads = new List<ReadOnlyThreadResult>();
            foreach (var thread in threadResult)
            {
                thread.Value.CountTime();
                threads.Add(new ReadOnlyThreadResult(thread.Value));
            }
            result = threads;
        }
    }
}
