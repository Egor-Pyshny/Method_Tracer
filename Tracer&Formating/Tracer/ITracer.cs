using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer_Formating.Tracer
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TracerResult GetTraceResult();
    }
}
