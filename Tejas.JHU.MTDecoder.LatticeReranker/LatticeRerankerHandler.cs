using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;

namespace Tejas.JHU.MTDecoder.LatticeReranker
{
    public class LatticeRerankerHandler:ILatticeRerankerHandler
    {
        
        public void RerankLattice(BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph, int numInputWords)
        {
            BackwardCostComputer backwardCostComputer = new BackwardCostComputer();
            backwardCostComputer.ComputeBackwardCost(latticeGraph,numInputWords);
            LatticeEdgePosteriorCostComputer latticeEdgePosteriorCostComputer = new LatticeEdgePosteriorCostComputer();
            latticeEdgePosteriorCostComputer.ComputeEdgePosteriorCost(latticeGraph);
        }
    }
}
