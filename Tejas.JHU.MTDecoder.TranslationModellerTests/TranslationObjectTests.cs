using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NUnit.Framework;
using Wintellect.PowerCollections;
using Tejas.JHU.MTDecoder.TranslationModeller;
namespace Tejas.JHU.MTDecoder.TranslationModellerTests
{
    [TestFixture]
    class TranslationObjectTests
    {

        [Test]
        public void TranslationObjectCompareTest()
        {
            var obj1 = new TranslationObject(1.2, "HEY");
            var obj2 = new TranslationObject(1.1, "HEY");
            var set = new OrderedSet<TranslationObject>();

            set.Add(obj1);
            set.Add(obj2);
            var x = set.GetFirst();
            
            Assert.True(x.Cost==1.1);                        
        }

        [Test]
        public void ReferenceRemoveTest()
        {
            var obj1 = new TranslationObject(1.2, "HEY");
            var obj2 = new TranslationObject(1.1, "HEY");
            var set = new OrderedSet<TranslationObject>();

            set.Add(obj1);
            set.Add(obj2);

            IDictionary<String,OrderedSet<TranslationObject>> d = new Dictionary<string, OrderedSet<TranslationObject>>();

            d.Add("1",set);

            OrderedSet<TranslationObject> w;
            d.TryGetValue("1", out w);
            Assert.True(w.Count==2);
            var item = w.GetFirst();
            w.RemoveFirst();

            Assert.True(w.Count==1);

            OrderedSet<TranslationObject> e;
            d.TryGetValue("1", out e);
            e.Add(item);
            Assert.True(e.Count==2);

            

        }

    }
}
