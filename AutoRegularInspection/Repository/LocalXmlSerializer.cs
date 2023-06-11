using AutoRegularInspection.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Repository
{
    public class LocalXmlSerializer<T> : IXmlSerializer<T>
    {
        private readonly System.Xml.Serialization.XmlSerializer _serializer;

        public LocalXmlSerializer()
        {
            _serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        }

        public void Serialize(TextWriter writer, T obj)
        {
            _serializer.Serialize(writer, obj);
        }

        public T Deserialize(StreamReader reader)
        {
            return (T)_serializer.Deserialize(reader);
        }
    }
}
