using NUnit.Framework;
using HelloWorld;


namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StringParser_ConvertToNumber_NormalText()
        {
            var sp = new StringParser("h");
            var result = sp.ConvertToNumber("Hi there");

            Assert.AreEqual(result, "7210532116104101114101");
        }
        [Test]
        public void StringParser_ConvertToNumber_WeirdText()
        {
            var sp = new StringParser("h");
            var result = sp.ConvertToNumber("1H i T h e r 3 ");

            Assert.AreEqual(result, "4972321053284321043210132114325132");
        }
        [Test]
        public void StringParser_ConvertToNumber3()
        {
            var sp = new StringParser("h");
            var result = sp.ConvertToNumber("-1");

            Assert.AreEqual(result, "4549");
        }
        [Test]
        public void StringParser_ConvertToNumber4()
        {
            var sp = new StringParser("h");
            var result = sp.ConvertToNumber("");

            Assert.AreEqual(result, "");
        }
        [Test]
        public void StringParser_ConvertToNumber5()
        {
            var sp = new StringParser("h");
            var result = sp.ConvertToNumber("@!#$%^&*()_+|/<>.,\\");

            Assert.AreEqual(result, "643335363794384240419543124476062464492");
        }
        [Test]
        public void StringParser_ConvertToNumber6()
        {
            var sp = new StringParser("h");
            var result = sp.ConvertToNumber(null);

            Assert.AreEqual(result, null);
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