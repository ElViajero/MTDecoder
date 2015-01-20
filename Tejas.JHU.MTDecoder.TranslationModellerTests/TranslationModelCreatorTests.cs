using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.TranslationModeller;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.TranslationModellerTests
{
    [TestFixture]
    class TranslationModelCreatorTests
    {
        [Test]
        public void StringSplitTest()
        {
            String line = "crap ||| crapa ||| -10001.00";
            var x = line.Split(new string[] {" ||| "}, StringSplitOptions.None);
            Assert.True(x[0].Equals("crap", StringComparison.InvariantCultureIgnoreCase));
            Assert.True(x[1].Equals("crapa", StringComparison.InvariantCultureIgnoreCase));
            Assert.True(x[2].Equals("-10001.00", StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void TryGetValueTest()
        {
            
            IDictionary<int,int> z = new Dictionary<int, int>();
            z.Add(1,1);
            z.Add(2,3);
            int x;
            z.TryGetValue(2, out x );
            Assert.True(x==3);


        }


        [Test]
        public void AbsoluteValueTest()
        {
            String x = "-1001";
            Assert.True(Math.Abs(Convert.ToDouble(x))==1001.00);
        }


        [Test]
        public void ReferenceAddTester()
        {

            IDictionary<int,OrderedSet<TranslationObject>> x = new Dictionary<int, OrderedSet<TranslationObject>>();
            x.Add(1,new OrderedSet<TranslationObject>());

            OrderedSet<TranslationObject> y;
            x.TryGetValue(1, out y);

            y.Add(new TranslationObject(1, "J"));

            OrderedSet<TranslationObject> z;
            x.TryGetValue(1, out z);

            Assert.True(z.GetFirst().Cost == 1);
            Assert.True(z.GetFirst().Phrase == "J");

        }

        [Test]
        public void CreateTranslationModelTest()
        {
            TranslationModelCreator creator = new TranslationModelCreator("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            var res = creator.CreateTranslationModel();
            OrderedSet<TranslationObject> x;
            res.TryGetValue("honorables", out x);
            Assert.True(res!=null);
            Assert.True(res.ContainsKey("honorables"));            
            Assert.True(x.Count==1);

            res.TryGetValue("sénateurs", out x);
            Assert.True(x.Count == 3);
            Assert.True(x.GetFirst().Cost == 0.124938733876);
            Assert.True(x.GetLast().Cost == 1.57978355885);
            
            
        }

    }
}
