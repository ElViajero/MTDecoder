using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.LanguageModeller
{
    public class NGramProbabilityObject
    {
        public double NgramCost { get; set; }
        public double BackOffCost { get; set; }

        public NGramProbabilityObject(double nGramCost, double backOffCost)
        {
            NgramCost = nGramCost;
            BackOffCost = backOffCost;
        }

    }
}
