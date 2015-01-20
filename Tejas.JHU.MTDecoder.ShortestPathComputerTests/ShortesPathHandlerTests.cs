using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.FutureCostModeller;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.LatticeCreator;
using Tejas.JHU.MTDecoder.LatticeReranker;
using Tejas.JHU.MTDecoder.ShortestPathComputer;
using Tejas.JHU.MTDecoder.TranslationModeller;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.ShortestPathComputerTests
{
    [TestFixture]
    class ShortesPathHandlerTests
    {
        [Test]
        public void ShortestPathHandlerTest()
        {
            TranslationModelHandler translationHandler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            LanguageModelHandler languageHandler = new LanguageModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\lm");
            translationHandler.PruneTranslationPhraseList(10);

            FutureCostModelHandler futureCostModelHandler = new FutureCostModelHandler(languageHandler, translationHandler,
                "je ai assisté hier à la première réunion de ce comité . ");

            BeamSearchDecoder decoder = new BeamSearchDecoder(languageHandler, translationHandler, futureCostModelHandler, 4, 2);

            var res = decoder.Decode("je ai assisté hier à la première réunion de ce comité . ");

            var wordList =
                "je ai assisté hier à la première réunion de ce comité . "
                    .Split(null);

            ILatticeRerankerHandler rerankerHandler = new LatticeRerankerHandler();
            rerankerHandler.RerankLattice(res,wordList.Length);

            ShortestPathComputerHandler shortestPathComputerHandler = new ShortestPathComputerHandler();
            var result = shortestPathComputerHandler.ComputeShortestPath(res);
            Assert.True(result!=null);


        }

        [Test]
        public void QueueTest()
        {
            OrderedSet<ShortestPathVertexObject> x =new OrderedSet<ShortestPathVertexObject>();
            x.Add(new ShortestPathVertexObject(new VertexProperties(0.0, "<s> <>", new OrderedSet<int>()), 2.0));

            x.Add(new ShortestPathVertexObject(new VertexProperties(0.0, "<s> <s>", new OrderedSet<int>()), 0.0));

            ShortestPathVertexObject a;
            a = x.GetLast();
            Assert.True(a.Distance==2.0);
        }

    }
}
