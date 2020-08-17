using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace HelloWorld
{
    public class FileSheet : Sheet
    {
        public FileSheetSettings Settings { get; set; }

        public FileSheet() : base()
        {
            this.AttachSheet(ThisAddIn.MyApp.Worksheets["File"]);
            if (File.Exists(@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\FileSheetSettings_xml.xml"))
            {
                this.Settings = FileSheetSettings.DeserializeSettings();       //if the xml exists, use it
            }
            else
                this.Settings = new FileSheetSettings();
            this.WriteToSheet();
        }
        public override void Format()
        {
            Excel.Workbook templateBook = ThisAddIn.Model.GetTemplateBook();
            Excel.Worksheet templateSheet = templateBook.Worksheets["File_Template"];
            this.ThisWorkSheet.Cells.Clear();
            templateSheet.Cells.Copy();
            ThisWorkSheet.Cells.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            ThisAddIn.MyApp.DisplayAlerts = false;
            templateBook.Saved = true;
            templateBook.Close();
            ThisAddIn.MyApp.DisplayAlerts = true;
            this.WriteToSheet();            
        }        
        public override void WriteToSheet()
        {
            ThisWorkSheet.Range["E4"].Value = Settings.TemplatePath;
            ThisWorkSheet.Range["E5"].Value = Settings.InflationDirectory;
        }
        public override void ReadFromSheet()
        {
            Settings.TemplatePath = ThisWorkSheet.Range["E4"].Value;
            Settings.InflationDirectory = ThisWorkSheet.Range["E5"].Value;
        }
        public void ClearSheetSettings()
        {
            ThisWorkSheet.Range["B4:B13"].Clear();
            ThisWorkSheet.Range["E4:E13"].Clear();
        }
        
    }
}
