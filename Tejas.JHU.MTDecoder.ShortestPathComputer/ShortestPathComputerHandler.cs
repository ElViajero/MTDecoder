using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.ShortestPathComputer
{
    public class ShortestPathComputerHandler: IShortestPathComputerHandler
    {
        public OrderedBag<ShortestPathVertexObject> Queue;

        public ShortestPathComputerHandler()
        {
            Queue = new OrderedBag<ShortestPathVertexObject>();
        }

        public IList<TaggedEdge<VertexProperties,EdgeProperties>> ComputeShortestPath(BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph)
        {
          var endVertex = CreateEndVertex(latticeGraph);
          var sourceVertex = InitializeSource(latticeGraph);
          InitiliazeQueue(latticeGraph,sourceVertex);
            //Queue.Add(new ShortestPathVertexObject(endVertex,double.MaxValue));
            if (Queue.Contains(new ShortestPathVertexObject(endVertex, 0.0)) || Queue.Contains(new ShortestPathVertexObject(endVertex, double.MaxValue)))
            {
                var r = 1;
            }
            
            var processedVertexList = new List<ShortestPathVertexObject>();  
          while (Queue.Count > 0)
          {
              var shortesPathVertex = Queue.GetFirst();
              Queue.RemoveFirst();
                                         
              foreach (var currentEdge in latticeGraph.OutEdges(shortesPathVertex.Vertex))    
              {
                  var targetShortesPathVertex = new ShortestPathVertexObject(currentEdge.Target,
                      shortesPathVertex.Distance + currentEdge.Tag.Cost);
                  var targetShortesPathVertex1 = new ShortestPathVertexObject(currentEdge.Target,
                     0.0);
                  var targetShortesPathVertex2 = new ShortestPathVertexObject(currentEdge.Target,
                      double.MaxValue);

                  var result = (from v in Queue
                      where v.Vertex.Equals(targetShortesPathVertex.Vertex)
                            && v.Distance > targetShortesPathVertex.Distance
                      select v).ToList();
                  Parallel.ForEach(result, v =>
                  {
                      v.Distance = targetShortesPathVertex.Distance;
                      v.Edge = currentEdge;
                      v.PreviousVertex = shortesPathVertex;
                  });

                  /*
                  if (currentEdge.Target.Equals(endVertex) || shortesPathVertex.Vertex.Equals(endVertex))
                  {
                      int x = 1;
                  }

                  if (Queue.Contains(targetShortesPathVertex1))
                  {
                      //ShortestPathVertexObject v;
                      var vList = Queue.GetEqualItems(targetShortesPathVertex1);//TryGetItem(targetShortesPathVertex, out v);
                      if (vList != null)
                      {
                          foreach (var v in vList)
                          {


                              if (targetShortesPathVertex.Distance <= v.Distance)
                              {
                                  //lock (v)
                                  {
                                      v.Distance = targetShortesPathVertex.Distance;
                                      v.Edge = currentEdge;
                                      v.PreviousVertex = shortesPathVertex;
                                  }
                              }
                          }
                      }
                      

                  }
                  else if (Queue.Contains(targetShortesPathVertex2))
                  {
                      //ShortestPathVertexObject v;
                      var vList = Queue.GetEqualItems(targetShortesPathVertex2);//TryGetItem(targetShortesPathVertex, out v);
                      if (vList != null)
                      {
                          foreach (var v in vList)
                          {


                              if (targetShortesPathVertex.Distance <= v.Distance)
                              {
                                  //lock (v)
                                  {
                                      v.Distance = targetShortesPathVertex.Distance;
                                      v.Edge = currentEdge;
                                      v.PreviousVertex = shortesPathVertex;
                                  }
                              }
                          }
                      }


                  }
                  */

              }

              processedVertexList.Add(shortesPathVertex);
              //if (processedVertexList.Count == latticeGraph.VertexCount)
                //  break;
          }

            var e = (from vertex in processedVertexList
                     where vertex.Vertex.CoverageVector.Count==0 &&
                     vertex.PreviousVertex!=null &&
                     vertex.Edge!=null &&
                     vertex.Distance < double.MaxValue
                     select vertex).FirstOrDefault();

            var edgeList = new List<TaggedEdge<VertexProperties, EdgeProperties>>();
            while (true)
            {
                edgeList.Add(e.Edge);
                e = e.PreviousVertex;
                if (e.Vertex.Equals(sourceVertex))
                    break;
            }

            return edgeList;

        }

        private void InitiliazeQueue(BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph,
            VertexProperties source)
        {
            foreach (var v in latticeGraph.Vertices)
            {
                if(v.Equals(source))
                    continue;                
                Queue.Add(new ShortestPathVertexObject(v, double.MaxValue));
            }
        }

        private VertexProperties InitializeSource(BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph)
        {
            var vertex = latticeGraph.Vertices.FirstOrDefault();
            OrderedSet<int> coverageVector = new OrderedSet<int>();
            for (int i = 0; i < vertex.CoverageVector.Count; i++)
            {
                coverageVector.Add(i);
            }
            var sourceVertex = (from v in latticeGraph.Vertices.AsParallel()
                where v.Equals(new VertexProperties(0.0, "<s> <s>", coverageVector))
                select v).FirstOrDefault();
            Queue.Add(new ShortestPathVertexObject(sourceVertex, 0.0));
            return sourceVertex;
        }

        private VertexProperties CreateEndVertex(BidirectionalGraph<VertexProperties, TaggedEdge<VertexProperties, EdgeProperties>> latticeGraph)
        {
            VertexProperties endVertex = new VertexProperties(0.0,"</s> </s>",new OrderedSet<int>());
            if (latticeGraph.ContainsVertex(endVertex))
            {
                var r = 1;
            }
            latticeGraph.AddVertex(endVertex);
            var completeVertexList = (from vertex in latticeGraph.Vertices.AsParallel()
                where vertex.CoverageVector.Count == 0
                select vertex).ToList();

            foreach (var vertex in completeVertexList)
            {
                bool addEdge = latticeGraph.AddEdge(new TaggedEdge<VertexProperties, EdgeProperties>(vertex, endVertex,
                    new EdgeProperties("", 0.0)));
            }
            return endVertex;
        }
    }
}
