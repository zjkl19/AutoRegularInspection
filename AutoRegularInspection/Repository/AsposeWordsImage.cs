using Aspose.Words;
using Aspose.Words.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Repository
{
    public class AsposeWordsImage
    {
        /// <summary>
        /// 提取word中的图片
        /// </summary>
        /// <param name="filePath">word文件路径</param>
        /// <param name="savePath">保存文件路径</param>
        /// <returns></returns>
        public static IEnumerable<string> ExportImageFromWordFile(string filePath, string savePath = "")
        {
            if (!File.Exists(filePath)) yield return string.Empty;
            if (string.IsNullOrEmpty(savePath)) savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Temp\\";

            //加载word
            Document doc = new Document(filePath);
            NodeCollection shapes = doc.GetChildNodes(NodeType.Shape, true);
            int imageIndex = 0;

            Shape shape;
            for (int i = 0; i < shapes.Count ; i++)
            {
                shape = shapes[i] as Shape;
                if (shape.HasImage)
                {
                    //扩展名
                    string ex = FileFormatUtil.ImageTypeToExtension(shape.ImageData.ImageType);
                    //文件名
                    string fileName = $"{imageIndex + 1}{ex}";
                    shape.ImageData.Save(savePath + fileName);

                    yield return fileName;
                    imageIndex++;
                }
            }
        }

        /// <summary>
        /// 提取word中的图片
        /// </summary>
        /// <param name="skipBefore">跳过之前的图片数量（要测试，因为有的Shape的HasImage属性为false）</param>
        /// <param name="skipAfter">跳过之后的图片数量（要测试，因为有的Shape的HasImage属性为false）</param>
        /// <param name="filePath">word文件路径</param>
        /// <param name="savePath">保存文件路径</param>
        /// <returns></returns>
        public static IEnumerable<string> ExportImageFromWordFile(int skipBefore,int skipAfter,string filePath, string savePath = "")
        {
            if (!File.Exists(filePath))
            {
                yield return string.Empty;
            }

            if (string.IsNullOrEmpty(savePath)) savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Temp\\";

            //加载word
            Document doc = new Document(filePath);
            var shapes = doc.GetChildNodes(NodeType.Shape, true);
            int imageIndex = 0;

            Shape shape;
            for (int i = skipBefore; i < shapes.Count-skipAfter; i++)
            {
                shape = shapes[i] as Shape;
                if (shape.HasImage)
                {
                    //扩展名
                    string ex = FileFormatUtil.ImageTypeToExtension(shape.ImageData.ImageType);
                    //文件名
                    string fileName = $"{imageIndex+1}{ex}";
                    shape.ImageData.Save(savePath + fileName);

                    yield return fileName;
                    imageIndex++;
                }
            }
            //return list;
        }
    }
}
