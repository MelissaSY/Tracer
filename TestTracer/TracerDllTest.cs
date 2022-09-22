using TracerDll;
using FluentAssertions;
using FluentAssertions.Execution;

namespace TestTracer
{
    [TestClass]
    public class TracerDllTest
    {
        private int _iterations = 3;
        public void SimpleMethod(Tracer tracer)
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
        }
        public void RecourseMethod(Tracer tracer, int iterations)
        {
            tracer.StartTrace();
            if(iterations > 0)
            {
                RecourseMethod(tracer, iterations - 1);
            }
            Thread.Sleep(20);
            tracer.StopTrace();
        }
        

        [TestMethod]
        public void OneMethod_OneThread()
        {
            //arrange
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Tracer tracer = new Tracer();
            //act
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
            TraceResult traceResult = tracer.GetTraceResult();
            //assert
            traceResult.result.Count.Should().Be(1);
            using(new AssertionScope())
            {
                ReadOnlyThreadResult threadResult = traceResult.result[0];
                threadResult.threadId.Should().Be(threadId);
                threadResult.childMethods.Count.Should().Be(1);
                threadResult.childMethods[0].methodName.Should().Be("OneMethod_OneThread");
                threadResult.childMethods[0].className.Should().Be("TracerDllTest");
                threadResult.time.Should().Be(threadResult.childMethods[0].time);
                threadResult.time.Should().BeGreaterThanOrEqualTo(100);
            }
        }
        [TestMethod]
        public void RecourseMethod_OneThread()
        {
            //arrange
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Tracer tracer = new Tracer();
            //act
            tracer.StartTrace();
            RecourseMethod(tracer, _iterations);
            tracer.StopTrace();
            TraceResult traceResult = tracer.GetTraceResult();
            //assert
            traceResult.result.Count.Should().Be(1);
            using (new AssertionScope())
            {
                ReadOnlyThreadResult threadResult = traceResult.result[0];
                IReadOnlyList<ReadOnlyMethodResult> methods = threadResult.childMethods;
                threadResult.threadId.Should().Be(threadId);
                methods = threadResult.childMethods;

                methods.Count.Should().Be(1);
                methods[0].methodName.Should().Be("RecourseMethod_OneThread");
                methods[0].className.Should().Be("TracerDllTest");
                methods[0].time.Should().BeGreaterThanOrEqualTo(20*_iterations);
                threadResult.time.Should().Be(methods[0].time);
                methods = methods[0].childMethods;
            }
        }
    }
}