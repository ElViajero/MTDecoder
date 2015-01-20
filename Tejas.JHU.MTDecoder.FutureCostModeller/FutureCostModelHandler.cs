using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.TranslationModeller;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.FutureCostModeller
{
    public class FutureCostModelHandler:IFutureCostModelHandler
    {

        public IDictionary<string, double> FutureCostDictionary;
        public String InputString;
        public OrderedSet<int> CoverageVector;
        public string[] InputStringWordList;  

        public FutureCostModelHandler(LanguageModelHandler languageModelHandler,
            TranslationModelHandler translationModelHandler,String inputString )
        {
            InputString = inputString;
            FutureCostModelCreator futureCostModelCreator = new FutureCostModelCreator(translationModelHandler,languageModelHandler);
            FutureCostDictionary = futureCostModelCreator.CreateFutureCostModel(InputString);
            InputStringWordList = InputString.Split(null);
            CoverageVector = new OrderedSet<int>();
            for (int i = 0; i < InputStringWordList.Length; i++)
            {
                CoverageVector.Add(i);
            }

        }

        public double ComputeFutureCost(OrderedSet<int> coverageVector)
        {
            var difference = CoverageVector.Except(coverageVector).ToList();
            OrderedSet<int> differenceSet = new OrderedSet<int>();
            differenceSet.AddMany(difference);
            int i = 0;
            double futureCost = 0.0;
            while (differenceSet.Count > 0)
            {
                int endIndex = differenceSet.GetFirst();
                differenceSet.RemoveFirst();
                String phraseString = "";
                for (int j = i; j < endIndex; j++)
                {
                    phraseString = phraseString +InputStringWordList[j] + " ";
                }
                i = endIndex + 1;
                if(phraseString.Equals("",StringComparison.InvariantCultureIgnoreCase))
                    continue;               
                phraseString = phraseString.Substring(0, phraseString.Length - 1);
                double tempCost = 0.0;
                FutureCostDictionary.TryGetValue(phraseString, out tempCost);
                futureCost = futureCost + tempCost;

            }

            if (i < InputStringWordList.Length)
            {
                string phraseString = "";
                for (int k = i; k < InputStringWordList.Length; k++)
                {
                    phraseString = phraseString + InputStringWordList[k] + " ";
                }
                phraseString = phraseString.Substring(0, phraseString.Length - 1);
                double tempCost = 0.0;
                FutureCostDictionary.TryGetValue(phraseString, out tempCost);
                futureCost = futureCost + tempCost;
            }

            return futureCost;

        }
    }
}
