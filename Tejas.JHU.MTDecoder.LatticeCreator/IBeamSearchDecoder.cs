using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;

namespace Tejas.JHU.MTDecoder.LatticeCreator
{
    public interface IBeamSearchDecoder
    {
        BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties,EdgeProperties>> Decode(String inputSentence);
    }
}
