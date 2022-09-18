using System.Diagnostics;
using System.Collections.Concurrent;

namespace TracerDll
{
    public class Tracer:ITracer
    {
        private TraceResult _traceResult;
        private Stopwatch _stopwatch;
        private ConcurrentDictionary<int, ConcurrentStack<MethodResult>> _startTime;
        private ConcurrentDictionary<int, ThreadResult> _threadResult;
        public Tracer()
        {
            _threadResult = new ConcurrentDictionary<int, ThreadResult>();
            _startTime = new ConcurrentDictionary<int, ConcurrentStack<MethodResult>>();
            _stopwatch = Stopwatch.StartNew();
        }

        public void StartTrace() 
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if(!_startTime.ContainsKey(threadId))
            {
                _startTime.TryAdd(threadId, new ConcurrentStack<MethodResult>());
                _threadResult.TryAdd(threadId, new ThreadResult(threadId));
                _threadResult[threadId].threadId = threadId;
                _threadResult[threadId].childMethods = new ConcurrentBag<MethodResult>();
            }

        }
        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            MethodResult? method;
            _startTime[threadId].TryPop(out method);
            if(method != null)
            {
                method.time = _stopwatch.ElapsedMilliseconds - method.time;
            }
        }
        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }
    }
}