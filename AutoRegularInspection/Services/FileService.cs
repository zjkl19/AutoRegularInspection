using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public static class FileService
    {
        /// 根据照片编号尾数获取照片完整的名称，如果有多张相同尾数的照片，按文件名排序取第一张
        /// </summary>
        /// <param name="folderName">文件夹名称</param>
        /// <param name="pictureNo">照片编号</param>
        /// <returns></returns>
        public static string GetFileName(string folderName,string pictureNo)
        {
            var lookup = ImageLocator.FindBest(pictureNo, folderName);
            if (lookup != null && !string.IsNullOrWhiteSpace(lookup.SelectedPath))
            {
                return lookup.SelectedPath;
            }

            throw new FileNotFoundException($"未找到编号为 {pictureNo} 的图片文件。");
        }

    }
}
