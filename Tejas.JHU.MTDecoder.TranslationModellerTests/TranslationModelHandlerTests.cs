using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.TranslationModeller;
namespace Tejas.JHU.MTDecoder.TranslationModellerTests
{
    [TestFixture]
    class TranslationModelHandlerTests
    {
        [Test]
        public void NullListTest()
        {
            ITranslationModelHandler handler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            var res = handler.GetTranslationPhraseList("sénateurs");
            Assert.True(res != null);
            Assert.True(res.Count==3);
            //Assert.True("sénateurs");

        }

        [Test]
        public void MulitWordPhraseTest()
        {
            ITranslationModelHandler handler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            var res = handler.GetTranslationPhraseList(", mardi dernier");
            Assert.True(res != null);
        }

        [Test]
        public void PrunerTest()
        {
            ITranslationModelHandler handler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            handler.PruneTranslationPhraseList(10);
            var res = handler.GetTranslationPhraseList(",");
            Assert.True(res.Count==10);
            Assert.That(res.GetFirst().Cost <= 2.31671905518);

        }

    }
}
