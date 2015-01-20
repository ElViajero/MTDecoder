using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tejas.JHU.MTDecoder.LanguageModeller
{
    public class LanguageModelHandler : ILanguageModelHandler
    {
        public ConcurrentDictionary<String, NGramProbabilityObject> LanguageModel;
        private readonly System.Object LockObject;

        public LanguageModelHandler(String filePath)
        {
            LanguageModelCreator creator = new LanguageModelCreator(filePath);
            LanguageModel = creator.CreateLanguageModel();
            LockObject = new System.Object();
        }


        public double GetLanguageModelCost(String state, String phrase)
        {

            double cost = 0.0;
            IList<String> nGramList = TrigramCreator(state, phrase);

            Parallel.ForEach(nGramList, currentNGram =>
            {

                double tempScore = 0.0;
                NGramProbabilityObject nGramProbabilityObject;
                if (LanguageModel.ContainsKey(currentNGram))
                {
                    LanguageModel.TryGetValue(currentNGram, out nGramProbabilityObject);
                    if (nGramProbabilityObject != null) tempScore = nGramProbabilityObject.NgramCost;
                }
                else
                {
                    var ngramList = currentNGram.Split(null);
                    if (LanguageModel.ContainsKey(ngramList[2]))
                    {
                        LanguageModel.TryGetValue(ngramList[2], out nGramProbabilityObject);
                        if (nGramProbabilityObject != null) tempScore = tempScore + nGramProbabilityObject.BackOffCost;
                    }
                    if (LanguageModel.ContainsKey(ngramList[1] + " " + ngramList[2]))
                    {
                        LanguageModel.TryGetValue(ngramList[2], out nGramProbabilityObject);
                        if (nGramProbabilityObject != null) tempScore = tempScore + nGramProbabilityObject.BackOffCost;
                    }
                    else if (LanguageModel.ContainsKey(ngramList[2]))
                    {
                        LanguageModel.TryGetValue(ngramList[2], out nGramProbabilityObject);
                        if (nGramProbabilityObject != null)
                        {
                            tempScore = tempScore + nGramProbabilityObject.BackOffCost;
                            tempScore = tempScore + nGramProbabilityObject.NgramCost;
                        }
                    }


                }

                lock(LockObject)
                {
                    cost = cost + tempScore;
                }
                                          
            });



            return cost;
        }


        public IList<String> TrigramCreator(String state, String phrase)
        {

            var stateList = state.Split(null);
            var phraseList = phrase.Split(null);
            IList<String> nGramList = new List<string>();

            String x = stateList[0];
            String y = stateList[1];

            

            for (int i = 0; i < phraseList.Length; i++)
            {
                String z = phraseList[i];
                nGramList.Add(x + " " + y + " " + z);
                x = y;
                y = z;
            }
            return nGramList;
        }

    }
}
