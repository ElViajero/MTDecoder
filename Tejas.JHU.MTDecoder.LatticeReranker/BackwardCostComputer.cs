using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.LatticeReranker
{
    public class BackwardCostComputer:IBackwardCostComputer
    {
        public void ComputeBackwardCost(BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph,int numInputWords)
        {

            var completeVertexList = (from vertex in latticeGraph.Vertices.AsParallel()
                where vertex.CoverageVector.Count == 0
                select vertex).ToList();
            
            Deque<VertexProperties> queue = new Deque<VertexProperties>();
            

            Parallel.ForEach(completeVertexList, currentVertex =>
            {
                currentVertex.BackwardCost = 0.0;                
            });

            for (int i = 1; i <= numInputWords; i++)
            {
                var vertexList = (from vertex in latticeGraph.Vertices.AsParallel()
                    where vertex.CoverageVector.Count == i
                    select vertex).ToList();

                queue.AddManyToBack(vertexList);
            }

            while (queue.Count > 0)
            {
                var currentVertex = queue.RemoveFromFront();
                foreach (var edge in latticeGraph.OutEdges(currentVertex))
                {
                    currentVertex.BackwardCost = currentVertex.BackwardCost + edge.Tag.Cost + edge.Target.BackwardCost;
                }

            }










        }
    }
}
