using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.DataReader;
using Tejas.JHU.MTDecoder.LanguageModeller;

namespace Tejas.JHU.MTDecoder.LanguageModellerTests
{
    [TestFixture]
    class LanguageModelCreatorTests
    {

        [Test]
        public void StringSplitTest()
        {
            String x = "-2.964804	\"	-0.3276199";
            var spl = x.Split('\t');
            Assert.True(spl.Length == 3);
            Assert.True(spl[0] == "-2.964804");
            Assert.True(spl[1] == "\"");
            Assert.True(spl[2] == "-0.3276199");

            x="\\data\\";
            spl = x.Split('\t');
            Assert.True(spl.Length==1);

        }


        [Test]
        public void CreateLanguageModelTest()
        {
            LanguageModelCreator modeller = new LanguageModelCreator("c:\\01 My Projects\\MTDecoder\\Data\\lm");
            IDictionary<String, NGramProbabilityObject> languageModel = modeller.CreateLanguageModel();

            Assert.True(languageModel.ContainsKey("Act"));
            Assert.True(languageModel.ContainsKey("are concerns we"));
            NGramProbabilityObject x;
            languageModel.TryGetValue("are concerns we", out x);
            Assert.True(x.NgramCost == 1.365182);
            Assert.True(x.BackOffCost == 0.0);


        }
    }
}
