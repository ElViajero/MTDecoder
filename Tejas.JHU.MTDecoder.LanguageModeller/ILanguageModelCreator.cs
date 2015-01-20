using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.LanguageModeller
{
    public interface ILanguageModelCreator
    {
        ConcurrentDictionary<String, NGramProbabilityObject> CreateLanguageModel();
    }
}
