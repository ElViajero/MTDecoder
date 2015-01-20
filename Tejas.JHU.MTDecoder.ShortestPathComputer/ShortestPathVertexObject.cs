using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;

namespace Tejas.JHU.MTDecoder.ShortestPathComputer
{
    public class ShortestPathVertexObject : IComparable<ShortestPathVertexObject>,IEquatable<ShortestPathVertexObject>
    {
        public VertexProperties Vertex;
        public ShortestPathVertexObject PreviousVertex;
        public TaggedEdge<VertexProperties,EdgeProperties> Edge;        
        public double Distance;

        public ShortestPathVertexObject(VertexProperties vertex, double disance)
        {
            Vertex = vertex;
            Distance = disance;
        }

        public int CompareTo(ShortestPathVertexObject other)
        {
            if (Vertex.State.Equals(other.Vertex.State, StringComparison.InvariantCultureIgnoreCase))
            {
                var res1 = Vertex.CoverageVector.Except(other.Vertex.CoverageVector).ToList();
                var res2 = other.Vertex.CoverageVector.Except(Vertex.CoverageVector).ToList();
                if (res1.Count == 0 && res2.Count == 0)
                    return 0;
            }
            if (Distance < other.Distance)
                return -1;

            return 1;
        }

        public bool Equals(ShortestPathVertexObject other)
        {
            return Vertex.Equals(other.Vertex);
        }
    }
}
