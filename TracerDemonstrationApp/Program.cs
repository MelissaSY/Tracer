using TracerDll;
using System.Threading;
using System.IO;
namespace TracerDemonstrationApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tracer tracer = new Tracer();
            Foo foo = new Foo(tracer);
            Bar bar = new Bar(tracer);
            Thread thread1 = new Thread(() => foo.MyMethod());
            thread1.Start();
            foo.MyMethod();
            Thread thread2 = new Thread(() => bar.RecourseMethod());
            thread2.Start();
            bar.RecourseMethod();
            thread1.Join();
            thread2.Join();
            TraceResult traceResult = tracer.GetTraceResult();

        //    TraceResultSerializable result = new TraceResultSerializable(traceResult);
            XmlTraceResult xmlSerializer = new XmlTraceResult();
            JsonTraceResult jsonSerializer = new JsonTraceResult();
            string xmlResult = xmlSerializer.Serialize(traceResult);
            string jsonResult = jsonSerializer.Serialize(traceResult);

            ResultWriter resultWriter = new ResultWriter();
            resultWriter.Write(xmlResult, Console.Out);
            resultWriter.Write(jsonResult, Console.Out);


            resultWriter.Write(jsonResult, "fff.json");
            resultWriter.Write(xmlResult, "fff.xml");

            Console.ReadLine();
        }
    }
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void MyMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _bar.InnerMethod();
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private static int _iterations;
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _iterations = 0;

            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }
        public void RecourseMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(10 - _iterations);
            if(_iterations < 2)
            {
                _iterations++;
                this.RecourseMethod();
            }
            _tracer.StopTrace();
        }
    }
}