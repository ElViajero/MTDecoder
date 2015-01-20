using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using Tejas.JHU.MTDecoder.FutureCostModeller;
using Tejas.JHU.MTDecoder.LanguageModeller;
using Tejas.JHU.MTDecoder.TranslationModeller;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.LatticeCreator
{
    public class BeamSearchDecoder : IBeamSearchDecoder
    {
        public ILanguageModelHandler LanguageModelHandlerObject;
        public ITranslationModelHandler TranslationModelHandlerObject;
        public IFutureCostModelHandler FutureCostModelHandlerObject;
        public System.Object LockObject;
        private int NumSkipWords;
        private int StackSize;

        public BeamSearchDecoder(LanguageModelHandler languageModelHandler,
            TranslationModelHandler translationModelHandler, FutureCostModelHandler futureCostModelHandler,
            int numSkipWords,int stackSize)
        {
            LanguageModelHandlerObject = languageModelHandler;
            TranslationModelHandlerObject = translationModelHandler;
            FutureCostModelHandlerObject = futureCostModelHandler;
            NumSkipWords = numSkipWords;
            StackSize = stackSize;
            LockObject = new object();
        }


        public BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> Decode(String inputSentence)
        {
              var latticeGraph =
                  new BidirectionalGraph<VertexProperties,TaggedEdge<VertexProperties,EdgeProperties>>();

            var inputSentenceWordList = inputSentence.Split(null);
            IList<OrderedSet<VertexProperties>> stackList = new List<OrderedSet<VertexProperties>>();
            var initialCoverageVector = new OrderedSet<int>();
            for (int i = 0; i <= inputSentenceWordList.Length; i++)
            {
                stackList.Add(new OrderedSet<VertexProperties>());
                if (i < inputSentenceWordList.Length)
                    initialCoverageVector.Add(i);
            }

            var sourceVertex = new VertexProperties(0.0,"<s> <s>",initialCoverageVector);
            stackList[0].Add(sourceVertex);
            latticeGraph.AddVertex(sourceVertex);

            

            for (int currentStack = 0; currentStack < stackList.Count-1; currentStack++)
            {
                int currentEntryNumber = 0;
                var processedVertexList = new OrderedSet<VertexProperties>();
                while (currentEntryNumber < Math.Min(StackSize,stackList[currentStack].Count))
                {
                    currentEntryNumber++;
                    var currentVertex = stackList[currentStack].GetFirst();
                    stackList[currentStack].RemoveFirst();

                    int startIndex = currentVertex.CoverageVector.GetFirst();
                    int endIndex = startIndex + NumSkipWords;

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        for (int j = i + 1; j < endIndex; j++)
                        {
                            String phrase = "";
                            bool flag = false;
                            OrderedSet<int> coveredIndices = new OrderedSet<int>();
                            for (int k = i; k < j; k++)
                            {
                                if (!currentVertex.CoverageVector.Contains(k))
                                {
                                    flag = true;
                                    break;
                                }
                                
                                phrase = phrase + inputSentenceWordList[k] + " ";
                                coveredIndices.Add(k);
                            }
                            
                            if (flag)
                                break;

                            var newCoverageVectorList = currentVertex.CoverageVector.Except(coveredIndices).ToList();
                            OrderedSet<int> newCoverageVector = new OrderedSet<int>();
                            newCoverageVector.AddMany(newCoverageVectorList);
                            double futureCost=FutureCostModelHandlerObject.ComputeFutureCost(newCoverageVector);
                            phrase = phrase.Substring(0, phrase.Length - 1);
                            var translationList = TranslationModelHandlerObject.GetTranslationPhraseList(phrase);
                            if(translationList==null)
                                continue;
                            Parallel.ForEach(translationList, currentTranslationObject =>
                            {
                                var currentEdge = new EdgeProperties(currentTranslationObject.Phrase,
                                    currentTranslationObject.Cost);
                                var languageModelCost =
                                    LanguageModelHandlerObject.GetLanguageModelCost(currentVertex.State,
                                        currentTranslationObject.Phrase);
                                currentEdge.Cost = currentEdge.Cost + languageModelCost;

                                var translationPhraseWordList = currentTranslationObject.Phrase.Split(null);

                                var extendedPhrase = currentVertex.State + " " + currentTranslationObject.Phrase;
                                var extendedPhraseWordList = extendedPhrase.Split(null);

                                String nextState = extendedPhraseWordList[extendedPhraseWordList.Length - 2] + " " +
                                                   extendedPhraseWordList[extendedPhraseWordList.Length - 1];
                                
                                var nextVertex = new VertexProperties(0.0,nextState, newCoverageVector);
                                nextVertex.RestCostEstimate = futureCost;

                                lock (LockObject)
                                {
                                    if (stackList[inputSentenceWordList.Length-newCoverageVector.Count].Contains(nextVertex))
                                    {
                                        VertexProperties vertex;
                                        stackList[inputSentenceWordList.Length-newCoverageVector.Count].TryGetItem(nextVertex, out vertex);
                                        vertex.ForwardCost = vertex.ForwardCost + currentEdge.Cost +
                                                             currentVertex.ForwardCost;
                                        latticeGraph.AddEdge(new TaggedEdge<VertexProperties, EdgeProperties>(
                                            currentVertex, vertex, currentEdge));
                                    }
                                    else
                                    {
                                        nextVertex.ForwardCost = currentVertex.ForwardCost + currentEdge.Cost;
                                        latticeGraph.AddVertex(nextVertex);
                                        stackList[inputSentenceWordList.Length-newCoverageVector.Count].Add(nextVertex);
                                        latticeGraph.AddEdge(new TaggedEdge<VertexProperties, EdgeProperties>(
                                            currentVertex, nextVertex, currentEdge));
                                        
                                    }
                                }


                            });

                        }
                    }

                    processedVertexList.Add(currentVertex);
                }
                if (stackList[currentStack + 1].Count == 0)
                {
                    Parallel.ForEach(processedVertexList, currentVertex =>
                    {
                        var firstUntranslatedWordIndex = currentVertex.CoverageVector.GetFirst();
                        OrderedSet<int> coveredWordSet = new OrderedSet<int>();
                        coveredWordSet.Add(firstUntranslatedWordIndex);
                        var newCoverageVectorList = currentVertex.CoverageVector.Except(coveredWordSet).ToList();
                        EdgeProperties newEdge = new EdgeProperties(inputSentenceWordList[firstUntranslatedWordIndex],0.0);
                        var currentStateWordList = currentVertex.State.Split(null);
                        var newState = currentStateWordList[1] + " " + inputSentenceWordList[firstUntranslatedWordIndex];
                        var newVertex = new VertexProperties(currentVertex.ForwardCost, newState ,new OrderedSet<int>(newCoverageVectorList));
                        lock (LockObject)
                        {
                            if(!latticeGraph.ContainsVertex(newVertex))
                                latticeGraph.AddVertex(newVertex);
                            latticeGraph.AddEdge(new TaggedEdge<VertexProperties, EdgeProperties>(currentVertex,newVertex,newEdge));
                        }
                    });
                }
            }

            
            return latticeGraph;
        }
    }
}
