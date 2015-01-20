using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tejas.JHU.MTDecoder.DataReader;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.TranslationModeller
{
    public class TranslationModelCreator: ITranslationModelCreator
    {
        private String TranslationFilePath { get; set; }        

        public TranslationModelCreator(String filePath)
        {
            TranslationFilePath = filePath;
        }

        public IDictionary<string, OrderedSet<TranslationObject>> CreateTranslationModel()
        {
            IDataReader dataReader = new DataReader.DataReader();
            var data = dataReader.ReadData(TranslationFilePath);

            IDictionary<String, OrderedSet<TranslationObject>> translationModel =
                new Dictionary<string, OrderedSet<TranslationObject>>();

            Parallel.ForEach(data, currentDataObject =>
            {

                var currentDataEnryList = currentDataObject.Split(new string[] { " ||| " }, StringSplitOptions.None);
                lock (translationModel)
                {
                    if (!translationModel.ContainsKey(currentDataEnryList[0]))
                        translationModel.Add(currentDataEnryList[0], new OrderedSet<TranslationObject>());

                    OrderedSet<TranslationObject> currentSet;
                    translationModel.TryGetValue(currentDataEnryList[0], out currentSet);
                    if (currentSet != null)
                        currentSet.Add(new TranslationObject(Math.Abs(Convert.ToDouble(currentDataEnryList[2])),
                            currentDataEnryList[1]));
                }
            });



            return translationModel;
        }
    }
}
