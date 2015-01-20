using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.TranslationModeller
{
   public class TranslationModelHandler:ITranslationModelHandler
    {
        public IDictionary<String, OrderedSet<TranslationObject>> TranslationModel;

        public TranslationModelHandler(String filePath)
        {
            ITranslationModelCreator creator = new TranslationModelCreator(filePath);
            TranslationModel = creator.CreateTranslationModel();
        }

        public OrderedSet<TranslationObject> GetTranslationPhraseList(string inputPhrase)
        {

            OrderedSet<TranslationObject> result = null;
            if (TranslationModel.ContainsKey(inputPhrase))
                TranslationModel.TryGetValue(inputPhrase, out result);
            return result;
        }

       public void PruneTranslationPhraseList(int listSize)
       {
           Parallel.ForEach(TranslationModel, currentEntry =>
           {
               if (currentEntry.Value.Count > listSize)
               {
                   while (currentEntry.Value.Count > listSize)
                   {
                       currentEntry.Value.RemoveLast();
                   }
                   
               }

           });
       

    }
    }
}
