using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;

namespace Tejas.JHU.MTDecoder.ShortestPathComputer
{
    public interface IShortestPathComputerHandler
    {
        IList<TaggedEdge<VertexProperties,EdgeProperties>> ComputeShortestPath(
            BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph);

    }
}
