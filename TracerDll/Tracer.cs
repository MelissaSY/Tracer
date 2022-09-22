using System.Diagnostics;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics.Tracing;
using System.IO;

namespace TracerDll
{
    public class Tracer:ITracer
    {
        private TraceResult _traceResult;
        private Stopwatch _stopwatch;
        private ConcurrentDictionary<int, ConcurrentStack<MethodResult>> _runningMethods;
        private ConcurrentDictionary<int, ThreadResult> _threadResult;
        public Tracer()
        {
            _threadResult = new ConcurrentDictionary<int, ThreadResult>();
            _runningMethods = new ConcurrentDictionary<int, ConcurrentStack<MethodResult>>();
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
            MethodBase? methodBase = stackFrame.GetMethod();
            Type? declaringType = methodBase.DeclaringType;
            MethodResult methodResult;
            MethodResult method = new MethodResult(methodBase.Name, declaringType.Name);
            if (!_runningMethods.ContainsKey(threadId))
            {
                _runningMethods.TryAdd(threadId, new ConcurrentStack<MethodResult>());
                _threadResult.TryAdd(threadId, new ThreadResult(threadId));
                _threadResult[threadId].threadId = threadId;
               // _threadResult[threadId].childMethods = new List<MethodResult>();
                _threadResult[threadId].time = 0;
            }

            if (_runningMethods[threadId].IsEmpty)
            {
                _threadResult[threadId].AddChild(method);
            }
            else
            {
                _runningMethods[threadId].TryPeek(out methodResult);
                methodResult.AddChild(method);
            }
            _runningMethods[threadId].Push(method);
            method.time = _stopwatch.ElapsedMilliseconds;
        }
        
        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            MethodResult? method;
            if(_runningMethods.ContainsKey(threadId))
            {
                _runningMethods[threadId].TryPop(out method);
                if (method != null)
                {
                    method.time = _stopwatch.ElapsedMilliseconds - method.time;
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