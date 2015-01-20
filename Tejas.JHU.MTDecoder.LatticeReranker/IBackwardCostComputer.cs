using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;

namespace Tejas.JHU.MTDecoder.LatticeReranker
{
    public interface IBackwardCostComputer
    {
        void ComputeBackwardCost(
            BidirectionalGraph<VertexProperties,TaggedEdge<VertexProperties,EdgeProperties>> latticeGraph,int numInputWords);
    }
}
