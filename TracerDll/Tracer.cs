using System.Diagnostics;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

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
            _stopwatch = new Stopwatch();
        }
        public void StartTrace() 
        {
            if(!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }
            int threadId = Thread.CurrentThread.ManagedThreadId;
            StackFrame stackFrame = new StackFrame(1);
            //add some checks here
            MethodResult methodResult;
            MethodResult method = new MethodResult(stackFrame.GetMethod().Name, stackFrame.GetMethod().DeclaringType.Name);
            if (!_startTime.ContainsKey(threadId))
            {
                _startTime.TryAdd(threadId, new ConcurrentStack<MethodResult>());
                _threadResult.TryAdd(threadId, new ThreadResult(threadId));
                _threadResult[threadId].threadId = threadId;
                _threadResult[threadId].childMethods = new ConcurrentBag<MethodResult>();
                _threadResult[threadId].time = 0;
            }
            //StackTrace stackTrace = new StackTrace();
            if (_startTime[threadId].IsEmpty)
            {
                _threadResult[threadId].childMethods.Add(method);
            }
            else
            {
                _startTime[threadId].TryPeek(out methodResult);
                methodResult.AddChild(method);
            }
            _startTime[threadId].Push(method);
            method.time = _stopwatch.ElapsedMilliseconds;
        }
        
        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            MethodResult? method;
            if(_startTime.ContainsKey(threadId))
            {
                _startTime[threadId].TryPop(out method);
                if (method != null)
                {
                    method.time = _stopwatch.ElapsedMilliseconds - method.time;
                    _threadResult[threadId].time += method.time;
                }
            }
        }
        public TraceResult GetTraceResult()
        {
            _traceResult = new TraceResult(_threadResult);
            return _traceResult;
        }
    }
}