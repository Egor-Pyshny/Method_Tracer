using Tracer_Formating.Tracer;
using Tracer_Formating.Utils.Serialization;
using ConsoleApp_Writers.Utils.Writers;

Tracer tracer = new Tracer();
ThreadFoo f = new ThreadFoo(tracer);
f.Start();
TracerResult traceResult = tracer.GetTraceResult();

ISerialize xmlSerializer = new XML();
ISerialize jsonSerializer = new JSON();

IWriter writer = new ConsoleWriter();
IWriter writer1 = new FileWriter("1.json");
IWriter writer2 = new FileWriter("2.xml");

writer2.Write(xmlSerializer.Serialize(traceResult));
writer1.Write(jsonSerializer.Serialize(traceResult));

public class ThreadFoo
{
    private ITracer tracer;
    public ThreadFoo(ITracer tracer)
    {
        this.tracer = tracer;
    }

    public void FirstThread()
    {
        tracer.StartTrace();
        Thread.Sleep(200);
        tracer.StopTrace();
    }

    public void SecondThread()
    {
        tracer.StartTrace();
        FirstThread();
        tracer.StopTrace();
    }

    public void Start()
    {
        Thread thread1 = new Thread(FirstThread);
        Thread thread2 = new Thread(SecondThread);
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
    }
}
