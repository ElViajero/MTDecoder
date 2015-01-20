using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.FutureCostModeller
{
    public interface IFutureCostModelCreator
    {
        IDictionary<String, double> CreateFutureCostModel(String inputString);
    }
}
