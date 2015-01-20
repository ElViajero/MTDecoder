using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.LanguageModeller
{
    public interface ILanguageModelHandler
    {
        double GetLanguageModelCost(String state, String phrase);
    }
}
