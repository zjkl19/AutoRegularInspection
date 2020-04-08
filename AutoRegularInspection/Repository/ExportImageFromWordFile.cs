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
        public static List<string> ExportImageFromWordFile(string filePath, string savePath = "")
        {
            if (!File.Exists(filePath)) return new List<string>();
            if (string.IsNullOrEmpty(savePath)) savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Temp\\";

            //文件名集合
            List<string> list = new List<string>();
            //加载word
            Document doc = new Document(filePath);
            NodeCollection shapes = doc.GetChildNodes(NodeType.Shape, true);
            int imageIndex = 0;
            var k = shapes.Count;
            foreach (Shape shape in shapes)
            {
                if (shape.HasImage)
                {
                    string time = DateTime.Now.ToString("HHmmssfff");
                    //扩展名
                    string ex = FileFormatUtil.ImageTypeToExtension(shape.ImageData.ImageType);
                    //文件名
                    string fileName = string.Format("{0}_{1}{2}", time, imageIndex, ex);
                    shape.ImageData.Save(savePath + fileName);
                    //添加文件到集合
                    list.Add(fileName);
                    imageIndex++;
                }
            }
            return list;
        }
    }
}
