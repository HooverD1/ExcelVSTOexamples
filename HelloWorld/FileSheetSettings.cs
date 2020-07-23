using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HelloWorld
{
    [Serializable]
    public class FileSheetSettings
    {
        public string TemplatePath { get; set; }
        public string InflationDirectory { get; set; }
        
        public void SerializeSettings()
        {
            Serializer.SerializeObject<FileSheetSettings>(this, $@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\FileSheetSettings_xml.xml");
        }
        public static FileSheetSettings DeserializeSettings()
        {
            return Serializer.ReadXML<FileSheetSettings>($@"C:\Users\grins\source\repos\HelloWorld\HelloWorld\FileSheetSettings_xml.xml");
        }
    }
}
