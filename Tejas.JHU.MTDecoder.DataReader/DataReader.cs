using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.DataReader
{
    public class DataReader : IDataReader
    {
        public IList<string> ReadData(string filePath)
        {
            IList<String> fileData = new List<string>();
            var file =
                new System.IO.StreamReader(filePath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                fileData.Add(line);
            }
            return fileData;
        }
    }
}
