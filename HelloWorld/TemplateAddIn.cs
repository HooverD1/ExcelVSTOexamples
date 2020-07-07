using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace HelloWorld
{
    public class TemplateAddIn: Excel.AddIn
    {
        public Excel.Worksheet sheet { get; set; }
        public Excel.Application Application => this.Application;

        public Excel.XlCreator Creator => this.Creator;

        public dynamic Parent => this.Parent;

        public string Author => this.Author;

        public string Comments => this.Comments;

        public string FullName => this.FullName;

        public bool Installed { get => this.Installed; set => this.Installed = value; }

        public string Keywords => this.Keywords;

        public string Name => this.Name;

        public string Path => this.Path;

        public string Subject => this.Subject;

        public string Title => this.Title;

        public string progID => this.progID;

        public string CLSID => this.CLSID;

        public bool IsOpen => this.IsOpen;
    }
}
