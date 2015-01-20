using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.TranslationModeller
{
    public interface ITranslationModelHandler
    {
        OrderedSet<TranslationObject> GetTranslationPhraseList(String inputPhrase);
        
        void PruneTranslationPhraseList(int listSize);
    }
}
