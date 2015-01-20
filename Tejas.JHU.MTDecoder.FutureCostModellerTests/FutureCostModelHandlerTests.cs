using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.FutureCostModeller;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.TranslationModeller;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.FutureCostModellerTests
{
    [TestFixture]
    class FutureCostModelHandlerTests
    {
        [Test]
        public void FutureCostForPartialStringTest()
        {
            TranslationModelHandler translationHandler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            LanguageModelHandler languageHandler = new LanguageModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\lm");

            String sentence = "je ai assisté hier à la première réunion de ce comité . ";
            IFutureCostModelHandler handler = new FutureCostModelHandler(languageHandler, translationHandler, sentence );
            var l = sentence.Split(null);
           OrderedSet<int> c = new OrderedSet<int>();
            for (int i = 0; i < l.Length-2; i++)
                c.Add(i);

            var res1 = handler.ComputeFutureCost(c);
            Assert.True(res1>0);
            c.Remove(1);
            var res2 = handler.ComputeFutureCost(c);
            Assert.True(res1!=res2);

        }
    }
}
