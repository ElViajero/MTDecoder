using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.TranslationModeller
{
    public class TranslationObject : IComparable
    {
        public double Cost { get; set; }
        public String Phrase { get; set; }

        public TranslationObject(double cost, String phrase)
        {
            Cost = cost;
            Phrase = phrase;
        }

        int IComparable.CompareTo(object obj)
        {
            var c = (TranslationObject)obj;
            if (this.Cost <= c.Cost)
                return -1;                        
            return 1;
            
            

        }

    }
}
