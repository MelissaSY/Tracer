using TracerDll;
namespace TestTracer
{
    [TestClass]
    public class TracerDllTest
    {
        private int _iterations = 3;
        public void Simple_Method(Tracer tracer)
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
        }
        public void Recourse_Method(Tracer tracer)
        {
            tracer.StartTrace();
            if(_iterations > 0)
            {
                _iterations--;
                Recourse_Method(tracer);
            }
            Thread.Sleep(20);
            tracer.StopTrace();
        }

        [TestMethod]
        public void One_Method_One_Thread()
        {
            //arrange
            Tracer tracer = new Tracer();
            //act
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
            TraceResult traceResult = tracer.GetTraceResult();
            //assert
            Assert.AreEqual(traceResult.result.Count, 1);
        }
    }
}