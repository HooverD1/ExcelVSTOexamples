using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;

namespace HelloWorld
{
    public partial class ThisAddIn
    {
        public static Excel.Application MyApp { get; set; }     //handle on the Excel application
        public static Excel.AddIn HelperVBA { get; set; }
        public static List<Excel.Application> TempAppList { get; set; }
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            MyApp = Globals.ThisAddIn.Application;      //Grab Excel at startup.
            TempAppList = new List<Excel.Application>();
            Utilities.LoadKeybinds();
            //this.test();
            HelperVBA = MyApp.AddIns["Experimental"];

        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            foreach (Excel.Application app in TempAppList)      //Kill any helper Applications
                app.Quit();
        }

        private COM_Visibles utilities;
        protected override object RequestComAddInAutomationService()        //What is this doing?
        {
            if (utilities == null)
                utilities = new COM_Visibles();
            return utilities;
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new MyRibbon();
        }

        

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
