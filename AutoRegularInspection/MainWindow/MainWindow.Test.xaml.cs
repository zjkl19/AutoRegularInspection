
using System.Windows;
using System;
using System.IO;
using System.Diagnostics;
using System.Text; // 引入文本命名空间以访问编码类
using Aspose.Words;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
       
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            // 指定子目录路径
            string targetDirectory = Path.Combine(currentDirectory, "更新域");
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.doc?");

            if (fileEntries.Length == 0)
            {
                MessageBox.Show("“更新域”目录下没有找到 .doc 或 .docx 文件。");
            }
            else
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                foreach (string fileName in fileEntries)
                {
                    try
                    {
                        Document doc = new Document(fileName);
                        doc.UpdateFields();
                        doc.UpdateFields();
                        doc.Save(fileName);
                        Console.WriteLine($"已更新文件中的域: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"更新文件 {fileName} 时出错: {ex.Message}");
                    }
                }

                stopwatch.Stop();
                MessageBox.Show($"所有文件的域更新过程已完成。程序运行耗时：{stopwatch.Elapsed.TotalMinutes} 分钟。");
            }

        }


    }
}
