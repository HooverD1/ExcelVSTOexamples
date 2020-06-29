using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using System.Reflection;


namespace HelloWorld
{
    public class RightClick    //Object to define the right-click menu additions
    {
        private ThisAddIn MyAddIn { get; set; }
        private CommandBar RightClickMenu { get; set; }         //There is supposed to be a newer "Fluent Ribbon" way to do this
        private List<CommandBarControl> MenuControls { get; set; }

        public RightClick(ThisAddIn MyAddIn)
        {
            this.MyAddIn = MyAddIn;
            this.RightClickMenu = MyAddIn.Application.CommandBars["Cell"];
            //SetupControls();
        }
        public void AddControls(List<CommandBarControl> MenuControls)
        {
            this.MenuControls = MenuControls;
            SetupControls();
        }
        public void SetupControls()
        {
            foreach(CommandBarControl control in MenuControls)
            {
                RightClickMenu.Controls.Add(control);       //Add all the controls to the bar
            }
        }
    }
}
