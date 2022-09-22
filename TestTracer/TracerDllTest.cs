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


                for(int i = 0; i < _iterations; i++)
                {
                    methods = methods[0].childMethods;
                    methods.Count.Should().Be(1);
                    methods[0].methodName.Should().Be("RecourseMethod");
                    methods[0].className.Should().Be("TracerDllTest");
                    methods[0].time.Should().BeGreaterThanOrEqualTo(20 *( _iterations - i));
                }
            }
        }
        public void InnerMethod(Tracer tracer)
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
        }
        public void OuterMethod(Tracer tracer)
        {
            tracer.StartTrace();
            InnerMethod(tracer);
            Thread.Sleep(50);
            tracer.StopTrace();
        }
        [TestMethod]
        public void SimpleMethod_OuterMethod_TwoThreads()
        {
            //arrange
            Tracer tracer = new Tracer();
            //act
            Thread thread1 = new Thread(() => { SimpleMethod(tracer); });
            Thread thread2 = new Thread(() => { OuterMethod(tracer); });
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            TraceResult traceResult = tracer.GetTraceResult();
            //assert
            using(new AssertionScope())
            {
                traceResult.result.Count.Should().Be(2);
                foreach(var method in traceResult.result)
                {
                    method.childMethods.Count.Should().Be(1);
                    method.childMethods[0].className.Should().Be("TracerDllTest");
                }
                traceResult.result[0].childMethods[0].methodName.Should().Be("SimpleMethod");

                ReadOnlyMethodResult secondThreadMethod = traceResult.result[1].childMethods[0];
                secondThreadMethod.methodName.Should().Be("OuterMethod");

                secondThreadMethod.childMethods.Count.Should().Be(1);
                secondThreadMethod.childMethods[0].methodName.Should().Be("InnerMethod");
                secondThreadMethod.childMethods[0].className.Should().Be("TracerDllTest");
            }
        }
    }
}