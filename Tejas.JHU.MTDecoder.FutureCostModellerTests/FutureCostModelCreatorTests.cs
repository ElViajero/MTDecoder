using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.FutureCostModeller;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.TranslationModeller;

namespace Tejas.JHU.MTDecoder.FutureCostModellerTests
{
    [TestFixture]
    class FutureCostModelCreatorTests
    {
        [Test]
        public void SubstringTest()
        {
            String x = "my name is tejas ";
            String y = x.Substring(0, x.Length - 1);
            Assert.True(y.Equals("my name is tejas"));
        }

        [Test]
        public void CreateFutureCostModelTest()
        {
            TranslationModelHandler translationHandler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            LanguageModelHandler languageHandler = new LanguageModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\lm");

            IFutureCostModelCreator futureCostModelCreator = new FutureCostModelCreator(translationHandler,languageHandler);

            var res =
                futureCostModelCreator.CreateFutureCostModel(
                    "honorables sénateurs , que se est - il passé ici , mardi dernier ?");
            Assert.True(res!=null);
            Assert.True(res.ContainsKey("honorables sénateurs ,"));
            double x=10.01;
            res.TryGetValue("honorables sénateurs ,", out x);
            res.TryGetValue("honorables", out x);
            Assert.True(x<double.MaxValue);



        }

    }
}
