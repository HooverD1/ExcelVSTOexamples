using NUnit.Framework;
using HelloWorld;
using System.IO;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OpenBookTest()
        {
            InflationTable table = new InflationTable(4);
            //table.DoNothing();
            table.UpdateTable();

            Assert.AreEqual(1,1);
        }

        //[Test]
        //public void ObjModel_SetSelection()
        //{
        //    string saveToCell = "c";
        //    ObjModel.SetSelection(saveToCell);

        //    Assert.AreEqual(ObjModel.GetSelectionValue(), saveToCell);
        //}
    }
}