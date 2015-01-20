using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;

namespace Tejas.JHU.MTDecoder.LatticeReranker
{
    public interface ILatticeEdgePosteriorCostComputer
    {
        void ComputeEdgePosteriorCost(
            BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph);
    }
}
