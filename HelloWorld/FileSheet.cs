using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace HelloWorld
{
    [Serializable]
    public class FileSheet : Sheet
    {
        public FileSheetSettings Settings { get; set; }
        public string TemplatePath{get;set;}

        public FileSheet() : base()
        {
            this.AttachSheet(ThisAddIn.MyApp.Worksheets["File"]);
            if (File.Exists(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld"))
                this.Settings = FileSheetSettings.DeserializeSettings();       //if the xml exists, use it
            else
                this.Settings = new FileSheetSettings();
            this.TemplatePath = Settings.TemplatePath;
        }
        public override void Format()
        {
            base.Format();
            Excel.Workbook templateBook = Utilities.OpenWorkbook(TemplatePath);
            Excel.Worksheet templateSheet = templateBook.Worksheets["File_Template"];
            this.ThisSheet.Cells.Clear();
            templateSheet.Cells.Copy();
            ThisSheet.Cells.PasteSpecial(Excel.XlPasteType.xlPasteFormats);
        }


    }
}
