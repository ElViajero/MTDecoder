using System.Data;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.DataReader;
using IDataReader = Tejas.JHU.MTDecoder.DataReader.IDataReader;

namespace Tejas.JHU.MTDecoder.DataReaderTests
{
    [TestFixture]
    class DataReaderTest
    {
        [Test]
        
        public void TestReadData()
        {
           
            IDataReader dataReader = new DataReader.DataReader();
            var res = dataReader.ReadData("c:\\01 My Projects\\MTDecoder\\Data\\input");
            Assert.That(res!=null);

        }
    }
}
