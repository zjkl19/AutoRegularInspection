using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.IRepository
{
    public interface IFileWriter
    {
        TextWriter Create(string path);
    }
}
