using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class HelperClasses
    {
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
                _bar.InnerMethod();
                _bar.InnerMethod();
                _tracer.StopTrace();
            }
        }

        public class Bar
        {
            private ITracer _tracer;

            internal Bar(ITracer tracer)
            {
                _tracer = tracer;
            }

            public void InnerMethod()
            {
                _tracer.StartTrace();
                Thread.Sleep(10);
                _tracer.StopTrace();
            }
        }

        public class ThreadFoo
        {
            private ITracer tracer;
            public ThreadFoo(ITracer tracer)
            {
                this.tracer = tracer;
            }

            public void Print(string str)
            {
                Console.WriteLine(str);
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
                Thread thread1 = new(FirstThread);
                Thread thread2 = new(SecondThread);
                thread1.Start();
                thread2.Start();
                thread1.Join();
                thread2.Join();
            }
        }
    }
}
