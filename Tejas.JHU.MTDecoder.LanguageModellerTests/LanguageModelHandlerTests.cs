using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.LanguageModeller;

namespace Tejas.JHU.MTDecoder.LanguageModellerTests
{
    [TestFixture]
    class LanguageModelHandlerTests
    {

        [Test]
        public void StringSplitTest()
        {
            String x = "My name is tejas";
            var y = x.Split(null);
            Assert.True(y.Length == 4);
            Assert.True(y[0].Equals("My", StringComparison.CurrentCultureIgnoreCase));
            Assert.True(y[1].Equals("name", StringComparison.CurrentCultureIgnoreCase));
            Assert.True(y[2].Equals("is", StringComparison.CurrentCultureIgnoreCase));
            Assert.True(y[3].Equals("tejas", StringComparison.CurrentCultureIgnoreCase));            
        }

        [Test]
        public void TrigramCreatorTest()
        {
            String state = "<s> <s>";
            String phrase = "said is that";
            LanguageModelHandler handler = new LanguageModelHandler("C:\\01 My Projects\\MTDecoder\\Data\\lm");

            var nGramList = handler.TrigramCreator(state, phrase);

            Assert.True(nGramList.Count == 3);
            Assert.True(nGramList.Contains("<s> <s> said"));
            Assert.True(nGramList.Contains("<s> said is"));
            Assert.True(nGramList.Contains("said is that"));

        }

        [Test]
        public void GetLanguageModelCostTest()
        {
            String state = "<s> <s>";
            String phrase = "said is that";
            LanguageModelHandler handler = new LanguageModelHandler("C:\\01 My Projects\\MTDecoder\\Data\\lm");

            var cost = handler.GetLanguageModelCost(state, phrase);

            Assert.True(cost>0.0);
        }


    }
}
