using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.FutureCostModeller;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.LatticeCreator;
using Tejas.JHU.MTDecoder.TranslationModeller;

namespace Tejas.JHU.MTDecoder.LatticeCreatorTests
{
    [TestFixture]
    class BeamSearchDecoderTest
    {
        [Test]
        public void BeamSearchTest()
        {
            TranslationModelHandler translationHandler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            LanguageModelHandler languageHandler = new LanguageModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\lm");
            translationHandler.PruneTranslationPhraseList(10);
           
            FutureCostModelHandler futureCostModelHandler = new FutureCostModelHandler(languageHandler, translationHandler,
                "et je me attendais à ce que le comité avoue que il ne savait trop quoi faire de les sénateurs indépendants . "); 
            
            BeamSearchDecoder decoder = new BeamSearchDecoder(languageHandler,translationHandler,futureCostModelHandler,4,20);
            var res = decoder.Decode("et je me attendais à ce que le comité avoue que il ne savait trop quoi faire de les sénateurs indépendants . ");
            Assert.True(res!=null);
            var s = (from vertex in res.Vertices
                where vertex.CoverageVector.Count == 0
                select vertex);
            Assert.True(s!=null);

        }
    }
}
