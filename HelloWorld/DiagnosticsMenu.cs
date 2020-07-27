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
                if (unit == TimeUnit.seconds)
                {
                    time = (double)stopwatch.ElapsedMilliseconds / 1000;
                    if(msgbox)
                        MessageBox.Show($"{time} sec; {message}");
                }
                else if (unit == TimeUnit.milliseconds)
                {
                    time = (double)stopwatch.ElapsedMilliseconds;
                    if(msgbox)
                        MessageBox.Show($"{(double)stopwatch.ElapsedMilliseconds} ms; {message}");
                }
                else
                {
                    time = (double)stopwatch.ElapsedTicks;
                    if(msgbox)
                        MessageBox.Show($"{(double)stopwatch.ElapsedTicks} ticks; {message}");
                }
            
            return time;
        }

    }
}
