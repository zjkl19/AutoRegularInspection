using AutoRegularInspection.Services;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.IRepository
{
    public interface IFileRepository
    {
        string[] GetFiles(string path, string searchPattern);
        bool Exists(string path);
        SixLabors.ImageSharp.Image<Rgba32> LoadImage(string path);
        void ResizeImage(SixLabors.ImageSharp.Image<Rgba32> image, int width, int height);
        void SaveImage(SixLabors.ImageSharp.Image<Rgba32> image, string path);
        string GetFileName(string path, string searchPattern);
    }

}
