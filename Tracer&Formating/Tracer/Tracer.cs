using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tracer_Formating.Tracer
{
    public class Tracer : ITracer
    {
        private ConcurrentDictionary<int, ConcurrentStack<MethodNode>> stacks;

        public Tracer() => stacks = [];

        public TracerResult GetTraceResult()
        {
            TracerResult res = new TracerResult();
            foreach (KeyValuePair<int, ConcurrentStack<MethodNode>> pair in stacks) {
                MethodNode parent = new MethodNode();
                MethodNode test = new MethodNode();
                bool change_parent = false;
                var reverseStack1 = new ConcurrentStack<MethodNode>(pair.Value);
                foreach (MethodNode node in reverseStack1)
                {
                    if (!node.isSystem)
                    {
                        parent.methods.Add(node);
                        if (change_parent)
                        {
                            node.parent = parent;
                            parent = node;
                        }
                    }
                    else { 
                        if (node.__sysName == "start")
                        {
                            change_parent = true;
                        }
                        else
                        {
                            parent = parent.parent;
                        }
                    }
                }
                ThreadNode th = new ThreadNode();
                th.tid = pair.Key;
                long duration = 0;
                th.methods = parent.methods;
                th.methods.ForEach(m => duration += m.duration);
                th.duration = duration;               
                res._threadNodes.Add(th);
            }

            return res;
        }

        public void StartTrace()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            if (!stacks.ContainsKey(tid))
            {
                stacks[tid] = new ConcurrentStack<MethodNode>();
            }
            StackTrace stackTrace = new StackTrace(1);
            StackFrame? stackFrame = stackTrace.GetFrame(0);
            if (stackFrame == null)
            {
                throw new NullReferenceException("Method stack trace is null");
            }
            MethodBase? mb = stackFrame.GetMethod();
            if (mb == null)
            {
                throw new NullReferenceException("Method stack trace is null");
            }
            string _class = (mb.ReflectedType == null || mb.ReflectedType.FullName == null) ? "Unknown" : mb.ReflectedType.FullName;
            MethodNode methodNode = new MethodNode(mb.Name, _class);          
            MethodNode marker = new MethodNode(null, null, true, "start");
            stacks[tid].Push(marker);
            stacks[tid].Push(methodNode);
            methodNode.stopwatch!.Start();
        }

        public void StopTrace()
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            if (!stacks.ContainsKey(tid))
            {
                stacks[tid] = new ConcurrentStack<MethodNode>();
            }
            StackTrace stackTrace = new StackTrace(1);
            StackFrame? stackFrame = stackTrace.GetFrame(0);
            if (stackFrame == null)
            {
                throw new NullReferenceException("Method stack trace is null");
            }
            MethodBase? mb = stackFrame.GetMethod();
            if (mb == null)
            {
                throw new NullReferenceException("Method stack trace is null");
            }
            MethodNode? methodNode = null;
            MethodNode marker = new MethodNode(null, null, true, "stop");
            foreach(MethodNode m in stacks[tid]) {
                if (!m.isSystem && m.closed == false) { 
                    m.closed = true;
                    methodNode = m;
                    break;
                }
            }
            if (methodNode == null) { 
                throw new NullReferenceException("MethodNode is null, you have at least one extra Tracer.Clsoe()");
            }
            methodNode!.stopwatch!.Stop();
            methodNode.duration = methodNode.stopwatch.ElapsedMilliseconds;
            stacks[tid].Push(marker);
        }
    }
}
