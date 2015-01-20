using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.TranslationModeller;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.FutureCostModeller
{
    public class FutureCostModelCreator: IFutureCostModelCreator
    {
        public ITranslationModelHandler TranslationModelHandlerObject;
        public ILanguageModelHandler LanguageModelHandlerObject;

        public FutureCostModelCreator(TranslationModelHandler translationModelHandler,
            LanguageModelHandler languageModelHandler)
        {
            TranslationModelHandlerObject = translationModelHandler;
            LanguageModelHandlerObject = languageModelHandler;
        }


        public IDictionary<string, double> CreateFutureCostModel(string inputString)
        {
            IDictionary<String,double> futureCostModel = new Dictionary<string, double>();
            
            var inputStringWordList = inputString.Split(null);

            for (int spanLength = 1; spanLength <= inputStringWordList.Length; spanLength++)
            {
                

                for (int start = 0; start < inputStringWordList.Length - spanLength + 1; start++)
                {
                    String phraseString = "";
                    var end = start + spanLength;
                    var score = double.MaxValue;
                    for (int split = start; split < end; split++)
                    {
                        double tempScore = 0.0;
                        String phrase1 = "";
                        String phrase2 = "";
                        bool flag = true;
                        for (int i = start; i < split; i++)
                        {
                            phrase1 = phrase1 + inputStringWordList[i] + " ";
                        }
                        if (!phrase1.Equals(""))
                        {
                            phrase1 = phrase1.Substring(0, phrase1.Length - 1);
                            var result = TranslationModelHandlerObject.GetTranslationPhraseList(phrase1);
                            if (result != null)
                            {
                                flag = false;
                                var translationObject = result.GetFirst();
                                tempScore = tempScore + translationObject.Cost;
                               // tempScore = tempScore +
                                 //           LanguageModelHandlerObject.GetLanguageModelCost("<s> <s>", phrase1);                            
                            }
                            else if (spanLength > 1)
                            {
                                futureCostModel.TryGetValue(phrase1, out tempScore);
                                flag = false;
                            }
                        }
                        flag = true;
                        for (int i = split; i < end; i++)
                        {
                            phrase2 = phrase2 + inputStringWordList[i] + " ";
                        }
                        if (!phrase2.Equals(""))
                        {
                            phrase2 = phrase2.Substring(0, phrase2.Length - 1);
                            
                            if (phrase1.Equals(""))
                                phraseString = phrase2;

                            var result = TranslationModelHandlerObject.GetTranslationPhraseList(phrase2);
                            if (result != null)
                            {
                                flag = false;
                                var translationObject = result.GetFirst();
                                tempScore = tempScore + translationObject.Cost;
                                //tempScore = tempScore +
                                  //          LanguageModelHandlerObject.GetLanguageModelCost("<s> <s>", phrase2);
                            }
                            else if (spanLength > 1)
                            {
                                double x;
                                futureCostModel.TryGetValue(phrase2, out x);
                                tempScore = tempScore + x;
                                flag = false;
                            }
                        }
                        if (flag)
                            tempScore = double.MaxValue;

                        if (tempScore < score)
                            score = tempScore;
                    }

                    score = score + LanguageModelHandlerObject.GetLanguageModelCost("<s> <s>", phraseString);

                    if(!futureCostModel.ContainsKey(phraseString))
                        futureCostModel.Add(phraseString, score);                    
                    
                }
            }


            return futureCostModel;
        }
    }
}
