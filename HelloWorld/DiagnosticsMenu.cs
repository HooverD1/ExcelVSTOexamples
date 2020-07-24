using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace HelloWorld
{
    public enum TimeUnit
    {
        ticks,
        milliseconds,
        seconds
    }

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

        public static double StopStopwatch(TimeUnit unit=TimeUnit.seconds, bool msgbox = false, string message="")
        {
            stopwatch.Stop();
            double time=-1;
            if (msgbox == true)
            {
                if (unit == TimeUnit.seconds)
                {
                    time = (double)stopwatch.ElapsedMilliseconds / 1000;
                    MessageBox.Show($"{time} {message}");
                }
                else if (unit == TimeUnit.milliseconds)
                {
                    time = (double)stopwatch.ElapsedMilliseconds;
                    MessageBox.Show($"{(double)stopwatch.ElapsedMilliseconds} {message}");
                }
                else
                {
                    time = (double)stopwatch.ElapsedTicks;
                    MessageBox.Show($"{(double)stopwatch.ElapsedTicks} {message}");
                }
            }
            return time;
        }

    }
}
