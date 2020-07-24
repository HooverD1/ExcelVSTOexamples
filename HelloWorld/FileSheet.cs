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
            this.Write();
        }
        public override void Format()
        {
            Excel.Workbook templateBook = ThisAddIn.Model.GetTemplateBook();
            Excel.Worksheet templateSheet = templateBook.Worksheets["File_Template"];
            this.ThisSheet.Cells.Clear();
            templateSheet.Cells.Copy();
            ThisSheet.Cells.PasteSpecial(Excel.XlPasteType.xlPasteAll);
            ThisAddIn.MyApp.DisplayAlerts = false;
            templateBook.Saved = true;
            templateBook.Close();
            ThisAddIn.MyApp.DisplayAlerts = true;
            this.Write();            
        }        
        public void Write()
        {
            ThisSheet.Range["E4"].Value = Settings.TemplatePath;
            ThisSheet.Range["E5"].Value = Settings.InflationDirectory;
        }
        public void Read()
        {
            Settings.TemplatePath = ThisSheet.Range["E4"].Value;
            Settings.InflationDirectory = ThisSheet.Range["E5"].Value;
        }
        public void ClearSheetSettings()
        {
            ThisSheet.Range["B4:B13"].Clear();
            ThisSheet.Range["E4:E13"].Clear();
        }
    }
}
