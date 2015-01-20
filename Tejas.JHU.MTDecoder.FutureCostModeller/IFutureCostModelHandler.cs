using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.FutureCostModeller
{
    public interface IFutureCostModelHandler
    {
        double ComputeFutureCost(OrderedSet<int> coverageVector);
    }
}
