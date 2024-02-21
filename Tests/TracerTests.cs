using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.HelperClasses;

namespace Tests
{
    [TestClass]
    public class TracerTests
    {
        private TracerResult TestInitializeNoThread()
        {
            List<ThreadNode> threadNodes = new List<ThreadNode>();
            ThreadNode threadNode = new ThreadNode();
            threadNode.tid = 0;
            threadNode.duration = 0;
            MethodNode methodNode = new MethodNode(), methodNode1 = new MethodNode(), methodNode2 = new MethodNode();
            methodNode._name = "MyMethod";
            methodNode._class = "Tests.HelperClasses+Foo";
            methodNode.duration = 0;

            methodNode1._name = "InnerMethod";
            methodNode1._class = "Tests.HelperClasses+Bar";
            methodNode1.duration = 0;
            methodNode1.methods = [];

            methodNode2._name = "InnerMethod";
            methodNode2._class = "Tests.HelperClasses+Bar";
            methodNode2.duration = 0;
            methodNode2.methods = [];

            methodNode.methods = [];
            methodNode.methods.Add(methodNode1);
            methodNode.methods.Add(methodNode2);

            threadNode.methods = [];
            threadNode.methods.Add(methodNode);
            threadNodes.Add(threadNode);
            return  new TracerResult(threadNodes);
        }

        private TracerResult TestInitializeThread()
        {
            List<ThreadNode> threadNodes = new List<ThreadNode>();
            ThreadNode threadNode = new ThreadNode(), threadNode1 = new ThreadNode();
            threadNode.tid = 0;
            threadNode.duration = 0;
            MethodNode methodNode = new MethodNode(), methodNode1 = new MethodNode(), methodNode2 = new MethodNode();
            methodNode._name = "FirstThread";
            methodNode._class = "Tests.HelperClasses+ThreadFoo";
            methodNode.duration = 0;

            methodNode1._name = "SecondThread";
            methodNode1._class = "Tests.HelperClasses+ThreadFoo";
            methodNode1.duration = 0;
            methodNode1.methods = [];
            methodNode1.methods.Add(methodNode);

            threadNode.methods = [];
            threadNode.methods.Add(methodNode);

            threadNode1.methods = [];
            threadNode1.methods.Add(methodNode1);

            threadNodes.Add(threadNode);
            threadNodes.Add(threadNode1);
            return new TracerResult(threadNodes);
        }

        private void CompareMethodNode(MethodNode actual, MethodNode expected)
        {
            actual._name.Should().Be(expected._name);
            actual._class.Should().Be(expected._class);
            actual.duration.Should().BeGreaterThan(0);
            for (int i = 0; i < actual.methods.Count; i++)
            {
                CompareMethodNode(actual.methods[i], expected.methods[i]);
            }
        }

        private void CompareThreadNode(ThreadNode actual, ThreadNode expected) {
            actual.tid.Should().BeGreaterThan(0);
            actual.duration.Should().BeGreaterThan(0);
            for (int i = 0; i < actual.methods.Count; i++)
            {
                CompareMethodNode(actual.methods[i], expected.methods[i]);
            }
        }

        [TestMethod]
        public void TestWithoutThreading()
        {
            Tracer tracer = new Tracer();
            Foo f = new Foo(tracer);
            f.MyMethod();
            var t = tracer.GetTraceResult();
            TracerResult tracerResult = TestInitializeNoThread();
            for(int i=0;i<t.threadNodes.Count;i++) {
                CompareThreadNode(t.threadNodes[i], tracerResult.threadNodes[i]);
            }
        }

        [TestMethod]
        public void TestWithThreading()
        {
            Tracer tracer = new Tracer();
            ThreadFoo f = new ThreadFoo(tracer);
            f.Start();
            var t = tracer.GetTraceResult();
            TracerResult tracerResult = TestInitializeThread();
            for (int i = 0; i < t.threadNodes.Count; i++)
            {
                CompareThreadNode(t.threadNodes[i], tracerResult.threadNodes[i]);
            }
        }
    }
}
