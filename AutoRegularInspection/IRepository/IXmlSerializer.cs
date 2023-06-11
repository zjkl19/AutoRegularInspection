using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.IRepository
{
    public interface IXmlSerializer<T>
    {
        void Serialize(TextWriter writer, T obj);
        T Deserialize(StreamReader reader);
        //void Serialize(IFileWriter fileWriter, OptionConfiguration configuration);
    }
}
