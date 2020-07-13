using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace HelloWorld
{
    public static class DiagnosticsMenu
    {
        public static Stopwatch stopwatch;
        public static void PrimeDiagnostics()
        {
            stopwatch = new Stopwatch();
        }
        public static void StartStopwatch()
        {
            stopwatch.Reset();
            stopwatch.Start();
        }
        public static double StopStopwatch()
        {
            stopwatch.Stop();
            //MessageBox.Show(stopwatch.ElapsedTicks.ToString());
            return (double)stopwatch.ElapsedTicks;
        }

    }
}
