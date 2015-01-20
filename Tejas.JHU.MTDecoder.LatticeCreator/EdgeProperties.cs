using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.LatticeCreator
{
    public class EdgeProperties : IEdge<VertexProperties>
    {
        public String Phrase;
        public double Cost;
        public Set<String> NgramSet;

        public EdgeProperties(String phrase, double cost)
        {
            Phrase = phrase;
            Cost = cost;
            NgramSet = new Set<string>();
        }

        public VertexProperties Source { get; private set; }
        public VertexProperties Target { get; private set; }
    }
}
