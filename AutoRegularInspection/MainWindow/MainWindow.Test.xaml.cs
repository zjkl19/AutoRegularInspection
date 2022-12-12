using AutoRegularInspection.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //var im = new ImageClass(FileService.GetFileName($"{App.PicturesFolder}", "DSC07952"));

            try
            {
                using (Image image = Image.Load<Rgba32>($"{App.PicturesFolder}\\DSC07952.jpg"))
                {
                    int width = image.Width / 8;
                    int height = image.Height / 8;
                    image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));
                    image.Save($"{App.PicturesOutFolder}\\DSC07952-out.jpg");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(CultureInfo.InvariantCulture));
            }



        }


    }
}
