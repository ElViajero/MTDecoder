using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.LatticeCreator
{
    public class VertexProperties : IEquatable<VertexProperties>,IComparable<VertexProperties>
    {
        public double ForwardCost { get; set; }
        public double BackwardCost { get; set; }
        public double RestCostEstimate { get; set; }
        public String State { get; set; }
        public OrderedSet<int> CoverageVector { get; set; }


        public VertexProperties(double forwardCost,
            String state, OrderedSet<int> coverageVector)
        {
            ForwardCost = forwardCost;
            BackwardCost = 0.0;
            RestCostEstimate = 0.0;
            State = state;
            CoverageVector = coverageVector;
        }

        public bool Equals(VertexProperties other)
        {
            var res1 = CoverageVector.Except(other.CoverageVector).ToList();
            var res2 = other.CoverageVector.Except(CoverageVector).ToList();
            if (res1.Count > 0 || res2.Count>0)
                return false;
            if (State.Equals(other.State, StringComparison.InvariantCultureIgnoreCase))
                return true;
            return false;

        }

        public int CompareTo(VertexProperties other)
        {
            if (State.Equals(other.State, StringComparison.InvariantCultureIgnoreCase))
            {
                var res1 = CoverageVector.Except(other.CoverageVector).ToList();
                var res2 = other.CoverageVector.Except(CoverageVector).ToList();
                if (res1.Count == 0 && res2.Count == 0)
                    return 0;
            }

            if (ForwardCost + RestCostEstimate  < other.ForwardCost + other.RestCostEstimate)
                return -1;
            
            
            return 1;
        }
    }


}

