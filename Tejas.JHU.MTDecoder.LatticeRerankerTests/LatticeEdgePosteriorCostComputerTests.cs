using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QuickGraph;
using Tejas.JHU.MTDecoder.FutureCostModeller;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.LatticeCreator;
using Tejas.JHU.MTDecoder.LatticeReranker;
using Tejas.JHU.MTDecoder.TranslationModeller;

namespace Tejas.JHU.MTDecoder.LatticeRerankerTests
{
    [TestFixture]
    class LatticeEdgePosteriorCostComputerTests 
    {
        [Test]
        public void LatticeEdgePosteriorCostComputerTest()
        {
            TranslationModelHandler translationHandler = new TranslationModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\tm");
            LanguageModelHandler languageHandler = new LanguageModelHandler("c:\\01 My Projects\\MTDecoder\\Data\\lm");
            translationHandler.PruneTranslationPhraseList(10);
            FutureCostModelHandler futureCostModelHandler = new FutureCostModelHandler(languageHandler,translationHandler,
                "et je me attendais à ce que le comité avoue que il ne savait trop quoi faire de les sénateurs indépendants . ");
            BeamSearchDecoder decoder = new BeamSearchDecoder(languageHandler, translationHandler,futureCostModelHandler, 4, 20);
            var res = decoder.Decode("et je me attendais à ce que le comité avoue que il ne savait trop quoi faire de les sénateurs indépendants . ");
            var words =
                "et je me attendais à ce que le comité avoue que il ne savait trop quoi faire de les sénateurs indépendants . "
                    .Split(null);

            var wr = (from vertex in res.Vertices
                      where vertex.BackwardCost > 0
                      select vertex).ToList();
            Assert.True(wr.Count == 0);

            IBackwardCostComputer backwardCostComputer = new BackwardCostComputer();

            backwardCostComputer.ComputeBackwardCost(res, words.Length);

            var r = (from vertex in res.Vertices
                     where vertex.BackwardCost > 0
                     select vertex).ToList();

            Assert.True(r.Count > 0);

            ILatticeEdgePosteriorCostComputer posteriorCostComputer = new LatticeEdgePosteriorCostComputer();
            posteriorCostComputer.ComputeEdgePosteriorCost(res);

            Assert.That(res!=null);

        }

    }
}
