using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNA_Test
{
    public partial class DateTimePicker_Custom : DateTimePicker
    {
        public DateTimePicker_Custom()
        {
            InitializeComponent();
            this.CalendarFont = new Font("Arial", 10);
            //Can't change colors because they are derived from the underlying windows style...
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            int style = (int)SendMessage(this.Handle, DTM_GETMCSTYLE, IntPtr.Zero, IntPtr.Zero);
            style |= MCS_NOTODAY | MCS_NOTODAYCIRCLE;
            SendMessage(this.Handle, DTM_SETMCSTYLE, IntPtr.Zero, (IntPtr)style);
            base.OnHandleCreated(e);
        }
        //pinvoke:
        private const int DTM_FIRST = 0x1000;
        private const int DTM_SETMCSTYLE = DTM_FIRST + 11;
        private const int DTM_GETMCSTYLE = DTM_FIRST + 12;
        private const int MCS_NOTODAYCIRCLE = 0x0008;
        private const int MCS_NOTODAY = 0x0010;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        //const int WM_ERASEBKGND = 0x14;
        //protected override void WndProc(ref System.Windows.Forms.Message m)
        //{
        //    if (m.Msg == WM_ERASEBKGND)
        //    {
        //        using (var g = Graphics.FromHdc(m.WParam))
        //        {
        //            using (var b = new SolidBrush(_backColor))
        //            {
        //                g.FillRectangle(b, ClientRectangle);
        //            }
        //        }
        //        return;
        //    }

        //    base.WndProc(ref m);
        //}
    }
}
