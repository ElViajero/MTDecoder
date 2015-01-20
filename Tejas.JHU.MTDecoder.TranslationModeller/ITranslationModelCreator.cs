using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Wintellect.PowerCollections;
namespace Tejas.JHU.MTDecoder.TranslationModeller
{
    public interface ITranslationModelCreator
    {
        IDictionary<String, OrderedSet<TranslationObject>> CreateTranslationModel();
    }
}
