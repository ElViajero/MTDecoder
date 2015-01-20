using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Tejas.JHU.MTDecoder.DataReader;

namespace Tejas.JHU.MTDecoder.LanguageModeller
{
    public class LanguageModelCreator : ILanguageModelCreator
    {
        String LanugeModelFilePath { get; set; }

        public LanguageModelCreator(String filePath)
        {
            LanugeModelFilePath = filePath;
        }

        public ConcurrentDictionary<string, NGramProbabilityObject> CreateLanguageModel()
        {
            IDataReader dataReader = new DataReader.DataReader();
            var data = dataReader.ReadData(LanugeModelFilePath);

            ConcurrentDictionary<String, NGramProbabilityObject> languageModel = new ConcurrentDictionary<string, NGramProbabilityObject>();

            Parallel.ForEach(data, currentDataObject =>
            {
                var currentDataEnryList = currentDataObject.Split('\t');

                if (currentDataEnryList.Length > 1 && !currentDataEnryList[0].Equals("ngram",StringComparison.CurrentCultureIgnoreCase))
                {
                    double ngramCost = Math.Abs(Convert.ToDouble(currentDataEnryList[0]));
                    double backOffCost = 0.0;
                    if(currentDataEnryList.Length==3)
                        backOffCost = Math.Abs(Convert.ToDouble(currentDataEnryList[2]));
                    lock (languageModel)
                    {
                        languageModel.TryAdd(currentDataEnryList[1],new NGramProbabilityObject(ngramCost,backOffCost));
                        
                    }

                }

            });

            return languageModel;

        }
    }
}
