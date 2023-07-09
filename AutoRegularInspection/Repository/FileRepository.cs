using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoRegularInspection.IRepository;
using SixLabors.ImageSharp.Processing;

namespace AutoRegularInspection.Repository
{
    public class FileRepository:IFileRepository
    {
        public string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public SixLabors.ImageSharp.Image<Rgba32> LoadImage(string path)
        {
            return SixLabors.ImageSharp.Image.Load<Rgba32>(path);
        }
        public void ResizeImage(SixLabors.ImageSharp.Image<Rgba32> image, int width, int height)
        {
            image.Mutate(x => x.Resize(width, height, KnownResamplers.Bicubic));
        }
        public void SaveImage(SixLabors.ImageSharp.Image<Rgba32> image, string path)
        {
            image.Save(path);
        }

        public string GetFileName(string folderName, string pictureNo)
        {
            string[] dirs = Directory.GetFiles($@"{folderName}/", $"*{pictureNo}.*");    //结果含有路径

            return dirs[0];
        }
    }
}
