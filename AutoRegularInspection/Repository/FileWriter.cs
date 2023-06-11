using AutoRegularInspection.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Repository
{
    public class FileWriter : IFileWriter
    {
        public TextWriter Create(string path)
        {
            return new StreamWriter(path);
        }
    }
}
