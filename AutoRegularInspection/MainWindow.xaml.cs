using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Fields;
using Aspose.Words.Tables;
using AutoRegularInspection.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Linq;
using AutoRegularInspection.Services;
using OfficeOpenXml;
using System;
using System.Threading;

using Ninject;
using AutoRegularInspection.IRepository;

namespace AutoRegularInspection
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            //TODO:考虑放到App.xaml中
            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository=kernel.Get<IDataRepository>();

            //TODO：Grid数据和Excel绑定
            var ds = new DamageSummaryServices();
            
            List<DamageSummary> lst;

            lst= dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            ds.InitListDamageSummary(lst);
            BridgeDeckGrid.ItemsSource= lst;

            lst = dataRepository.ReadDamageData(BridgePart.SuperSpace);
            ds.InitListDamageSummary(lst,2_000_000);
            SuperSpaceGrid.ItemsSource = lst;

            lst = dataRepository.ReadDamageData(BridgePart.SubSpace);
            ds.InitListDamageSummary(lst, 3_000_000);
            SubSpaceGrid.ItemsSource = lst;

        }


        private void AutoReport_Click(object sender, RoutedEventArgs e)
        {
            string templateFile = "外观检查报告模板.docx";string outputFile = "自动生成的外观检查报告.docx";
            string BookmarkStartName = "BridgeDeckStart";
            int CompressImageFlag = 80;    //图片压缩质量（0-100,值越大质量越高）
            var listDamageSummary = BridgeDeckGrid.ItemsSource as List<DamageSummary>;

            double ImageWidth = 224.25; double ImageHeight = 168.75;

            new Thread(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var doc = new Document(templateFile);
                        InsertSummaryAndPictureTable(BookmarkStartName, CompressImageFlag, listDamageSummary, ImageWidth, ImageHeight, doc);

                        BookmarkStartName = "SuperSpaceStart";
                        listDamageSummary = SuperSpaceGrid.ItemsSource as List<DamageSummary>;
                        InsertSummaryAndPictureTable(BookmarkStartName, CompressImageFlag, listDamageSummary, ImageWidth, ImageHeight, doc);

                        BookmarkStartName = "SubSpaceStart";
                        listDamageSummary = SubSpaceGrid.ItemsSource as List<DamageSummary>;
                        InsertSummaryAndPictureTable(BookmarkStartName, CompressImageFlag, listDamageSummary, ImageWidth, ImageHeight, doc);

                        doc.UpdateFields();
                        doc.UpdateFields();

                        doc.Save(outputFile, SaveFormat.Docx);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }));
            }).Start();


        }

        private void InsertSummaryAndPictureTable(string BookmarkStartName,int CompressImageFlag, List<DamageSummary> listDamageSummary, double ImageWidth, double ImageHeight, Document doc)
        {
            var builder = new DocumentBuilder(doc);

            var fieldStyleRefBuilder = new FieldBuilder(FieldType.FieldStyleRef);
            fieldStyleRefBuilder.AddArgument(1);
            fieldStyleRefBuilder.AddSwitch(@"\s");

            var fieldSequenceBuilder = new FieldBuilder(FieldType.FieldSequence);
            fieldSequenceBuilder.AddArgument("图");
            fieldSequenceBuilder.AddSwitch(@"\*", "ARABIC");
            fieldSequenceBuilder.AddSwitch(@"\s", "1");

            //_Refxx的书签不会在word的“插入”=>“书签”中显示

            FieldRef pictureRefField;

            //模板在书签位置格式调整
            //1、单倍行距
            //2、首行不缩进
            var bookmark = doc.Range.Bookmarks[BookmarkStartName];

            builder.MoveTo(bookmark.BookmarkStart);

            //病害汇总表格
            var summaryTable = builder.StartTable();

            builder.InsertCell();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Font.Bold = true;

            builder.Write("序号");
            builder.InsertCell(); builder.Write("位置");
            builder.InsertCell(); builder.Write("构件类型");
            builder.InsertCell(); builder.Write("缺损类型");
            builder.InsertCell(); builder.Write("缺损描述");
            builder.InsertCell(); builder.Write("图示编号");

            builder.Font.Bold = false;
            builder.EndRow();

            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                builder.InsertCell(); builder.Write($"{i + 1}");
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].Position}");
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].Component}");
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].Damage}");
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].DamageDescription}");
                builder.InsertCell();
                if (listDamageSummary[i].PictureCounts == 0)
                {
                    builder.Write("/");
                }
                else if (listDamageSummary[i].PictureCounts == 1)
                {
                    pictureRefField = InsertFieldRef(builder, $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex}", "", "");
                    pictureRefField.InsertHyperlink = true;
                }
                else if (listDamageSummary[i].PictureCounts == 2)
                {
                    pictureRefField = InsertFieldRef(builder, $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex}", "", "");
                    pictureRefField.InsertHyperlink = true;

                    builder.Write("\r\n");

                    pictureRefField = InsertFieldRef(builder, $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex + 1}", "", "");
                    pictureRefField.InsertHyperlink = true;
                }
                else    //图片大于2张
                {
                    pictureRefField = InsertFieldRef(builder, $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex}", "", "");
                    pictureRefField.InsertHyperlink = true;

                    builder.Write("\r\n～\r\n");

                    pictureRefField = InsertFieldRef(builder, $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex + listDamageSummary[i].PictureCounts - 1}", "", "");
                    pictureRefField.InsertHyperlink = true;
                }
                builder.EndRow();
            }


            builder.EndTable();

            // Set a green border around the table but not inside. 
            summaryTable.SetBorder(BorderType.Left, LineStyle.Single, 1.5, Color.Black, true);
            summaryTable.SetBorder(BorderType.Right, LineStyle.Single, 1.5, Color.Black, true);
            summaryTable.SetBorder(BorderType.Top, LineStyle.Single, 1.5, Color.Black, true);
            summaryTable.SetBorder(BorderType.Bottom, LineStyle.Single, 1.5, Color.Black, true);

            builder.Writeln();

            //病害内容插入表格

            //Reference:
            //https://github.com/aspose-words/Aspose.Words-for-.NET/blob/f84af3bfbf2a1f818551064a0912b106e848b2ad/Examples/CSharp/Programming-Documents/Bookmarks/BookmarkTable.cs
            var pictureTable = builder.StartTable();    //病害详细图片

            //计算总的图片数量
            int totalPictureCounts = 0;

            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                totalPictureCounts += listDamageSummary[i].PictureCounts;
            }

            int tableTotalRows = 2 * ((totalPictureCounts + 1) / 2);    //表格总行数
            int tableTotalCols = 2;

            for (int i = 0; i < tableTotalRows; i++)
            {
                for (int j = 0; j < tableTotalCols; j++)
                {
                    builder.InsertCell();
                }
                builder.EndRow();
            }
            builder.EndTable();

            //builder.MoveTo(table.Rows[1 + 1].Cells[0].FirstParagraph);
            int curr = 0;    //当前已插入图片数
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                if (listDamageSummary[i].PictureCounts > 0)    //有图片则插入
                {
                    var p = listDamageSummary[i].PictureNo.Split(',');
                    for (int j = 0; j < p.Length; j++)
                    {
                        builder.MoveTo(pictureTable.Rows[2 * (int)(curr / 2)].Cells[(curr) % 2].FirstParagraph);

                        var dirs = Directory.GetFiles(@"Pictures/", $"*{p[j]}*");    //结果含有路径

                        //TODO：检测文件是否重复，若重复不需要再压缩（MD5校验）
                        //(暂时用文件名校验)
                        if(!File.Exists($"PicturesOut/{Path.GetFileName(dirs[0])}"))
                        {
                            CompressImage($"{dirs[0]}", $"PicturesOut/{Path.GetFileName(dirs[0])}", CompressImageFlag);    //只取查找到的第1个文件，TODO：UI提示       
                        }
                        builder.InsertImage($"PicturesOut/{Path.GetFileName(dirs[0])}", RelativeHorizontalPosition.Margin, 0, RelativeVerticalPosition.Margin, 0, ImageWidth, ImageHeight, WrapType.Inline);

                        builder.MoveTo(pictureTable.Rows[2 * (int)(curr / 2) + 1].Cells[(curr) % 2].FirstParagraph);
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.StartBookmark($"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex + j}");
                        builder.Write("图 ");
                        fieldStyleRefBuilder.BuildAndInsert(pictureTable.Rows[2 * (int)(curr / 2) + 1].Cells[(curr) % 2].Paragraphs[0]);
                        builder.Write("-");
                        fieldSequenceBuilder.BuildAndInsert(pictureTable.Rows[2 * (int)(curr / 2) + 1].Cells[(curr) % 2].Paragraphs[0]);
                        builder.EndBookmark($"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex + j}");
                        builder.Write($" {listDamageSummary[i].DamageDescriptionInPicture}-{j + 1}");


                        curr++;
                    }
                }
            }


            pictureTable.ClearBorders();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_Option_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }

        private void MenuItem_ViewSourceCode_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/zjkl19/AutoRegularInspection/");
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            //TODO：通过反射读取 AssemblyCopyright
            MessageBox.Show($"当前版本v{Application.ResourceAssembly.GetName().Version.ToString()}\r" +
            $"Copyright © 福建省建筑科学研究院 福建省建筑工程质量检测中心有限公司 2020\r" +
            $"系统框架设计、编程及维护：路桥检测研究所林迪南等"
            , "关于");
        }

        private void DisclaimerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }
        private void InstructionsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }

        /// <summary>
        /// Insert a sequence field with preceding text and a specified sequence identifier
        /// </summary>
        public FieldSeq InsertSeqField(DocumentBuilder builder, string textBefore, string textAfter, string sequenceIdentifier)
        {
            builder.Write(textBefore);
            FieldSeq fieldSeq = (FieldSeq)builder.InsertField(FieldType.FieldSequence, true);
            fieldSeq.SequenceIdentifier = sequenceIdentifier;
            builder.Write(textAfter);

            return fieldSeq;
        }

        /// <summary>
        /// Get the document builder to insert a REF field, reference a bookmark with it, and add text before and after
        /// </summary>
        private FieldRef InsertFieldRef(DocumentBuilder builder, string bookmarkName, string textBefore, string textAfter)
        {
            builder.Write(textBefore);
            FieldRef field = (FieldRef)builder.InsertField(FieldType.FieldRef, true);
            field.BookmarkName = bookmarkName;
            builder.Write(textAfter);
            return field;
        }
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片地址</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="size">压缩后图片的最大大小</param>
        /// <param name="sfsc">是否是第一次调用</param>
        /// <returns></returns>
        public static bool CompressImage(string sFile, string dFile, int flag = 90, int size = 300, bool sfsc = true)
        {
            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                firstFileInfo.CopyTo(dFile);
                return true;
            }
            Image iSource = Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int dHeight = iSource.Height / 2;
            int dWidth = iSource.Width / 2;
            int sW = 0, sH = 0;
            //按比例缩放
            var tem_size = new System.Drawing.Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                    FileInfo fi = new FileInfo(dFile);
                    if (fi.Length > 1024 * size)
                    {
                        flag = flag - 10;
                        CompressImage(sFile, dFile, flag, size, false);
                    }
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
    }
}
