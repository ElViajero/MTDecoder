using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.LatticeReranker
{
   public  class NGramPosteriorCostObject
    {
       public double PosteriorCost { get; set; }
       public double CurrentBestCost { get; set; }

       public NGramPosteriorCostObject(double posteriorCost, double currentBestCost)
       {
           PosteriorCost = posteriorCost;
           CurrentBestCost = currentBestCost;
       }
    }
}
