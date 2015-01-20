using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;

namespace Tejas.JHU.MTDecoder.LatticeReranker
{
    public class LatticeEdgePosteriorCostComputer : ILatticeEdgePosteriorCostComputer
    {

        private IDictionary<String, NGramPosteriorCostObject> NgramPosteriorCostDict;

        public LatticeEdgePosteriorCostComputer()
        {
            NgramPosteriorCostDict = new Dictionary<string, NGramPosteriorCostObject>();
        }

        public void ComputeEdgePosteriorCost(BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph)
        {
            Parallel.ForEach(latticeGraph.Edges, currentEdge =>
            {
                currentEdge.Tag.Cost = currentEdge.Source.ForwardCost + currentEdge.Tag.Cost +
                                       currentEdge.Target.BackwardCost;
                String phraseString = currentEdge.Source.State + " " + currentEdge.Tag.Phrase;
                var phraseWordList = phraseString.Split(null);

                String x = phraseWordList[0];
                String y = phraseWordList[1];
                for (int i = 2; i < phraseWordList.Length; i++)
                {
                    String z = phraseWordList[i];
                    String bigram = y + " " + z;
                    String trigram = x+" " + bigram;
                    currentEdge.Tag.NgramSet.Add(bigram);
                    currentEdge.Tag.NgramSet.Add(trigram);
                    currentEdge.Tag.NgramSet.Add(z);

                    x = y;
                    y = z;
                }

                lock (NgramPosteriorCostDict)
                {
                    foreach (var ngram in currentEdge.Tag.NgramSet)
                    {
                        if (!NgramPosteriorCostDict.ContainsKey(ngram))
                            NgramPosteriorCostDict.Add(ngram,
                                new NGramPosteriorCostObject(currentEdge.Tag.Cost, currentEdge.Tag.Cost));
                        else
                        {
                            NGramPosteriorCostObject nGramPosteriorCostObject=null;
                            NgramPosteriorCostDict.TryGetValue(ngram, out nGramPosteriorCostObject);
                            if (nGramPosteriorCostObject.CurrentBestCost > currentEdge.Tag.Cost)
                            {
                                nGramPosteriorCostObject.CurrentBestCost = currentEdge.Tag.Cost;
                                nGramPosteriorCostObject.PosteriorCost = nGramPosteriorCostObject.PosteriorCost + currentEdge.Tag.Cost;
                            }
                            
                        }
                    }

                }


            });

            Parallel.ForEach(latticeGraph.Edges, currentEdge =>
            {
                currentEdge.Tag.Cost = 0;
                foreach (var ngram in currentEdge.Tag.NgramSet)
                {
                    NGramPosteriorCostObject nGramPosteriorCostObject;
                    NgramPosteriorCostDict.TryGetValue(ngram, out nGramPosteriorCostObject);
                    currentEdge.Tag.Cost = currentEdge.Tag.Cost + nGramPosteriorCostObject.PosteriorCost;
                }
            });
        


        }
    }
}
