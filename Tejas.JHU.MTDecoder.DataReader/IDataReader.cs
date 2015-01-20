using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tejas.JHU.MTDecoder.DataReader
{
    public interface IDataReader
    {

        IList<String> ReadData(String filePath);

    }
}
