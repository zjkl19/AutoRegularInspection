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
            var testList = new List<DamageSummary>
            {
                new DamageSummary {No=1,Position="第1跨",Component="伸缩缝",Damage="缝内沉积物阻塞",DamageDescription="左幅0#伸缩缝沉积物阻塞"
                ,DamageDescriptionInPicture="左幅0#伸缩缝沉积物阻塞",PictureNo="855"},
                new DamageSummary {No=2,Position="第1跨",Component="伸缩缝",Damage="接缝处铺装碎边",DamageDescription="左幅0#伸缩缝接缝处铺装碎边"
                ,DamageDescriptionInPicture="左幅0#伸缩缝接缝处铺装碎边",PictureNo="868,875"},
            };

            gridTotal.ItemsSource = testList;
            var ds = new DamageSummaryServices();
            ds.InitListDamageSummary(testList);
            ;
            //StartMain();

        }

        public void StartMain()
        {
            //var doc = new Document("default.docx");
            //var builder = new DocumentBuilder(doc);

            //获取ItemsSource的值参考代码
            //var m1 = gridTotal.ItemsSource as List<DamageSummary>;
            //MessageBox.Show(m1[0].Component);

            //int PictureNoColumn = 7;    //照片编号所在列

            double ImageWidth = 224.25; double ImageHeight = 168.75;

            var doc = new Document("RegularInspectionTemplate.docx");
            var builder = new DocumentBuilder(doc);

            var fieldStyleRefBuilder = new FieldBuilder(FieldType.FieldStyleRef);
            fieldStyleRefBuilder.AddArgument(1);
            fieldStyleRefBuilder.AddSwitch(@"\s");

            var fieldSequenceBuilder = new FieldBuilder(FieldType.FieldSequence);
            fieldSequenceBuilder.AddArgument("图");
            fieldSequenceBuilder.AddSwitch(@"\*", "ARABIC");
            fieldSequenceBuilder.AddSwitch(@"\s", "1");

            //_Refxx的书签不会在word的“插入”=>“书签”中显示

            //模板在书签位置格式调整
            //1、单倍行距
            //2、首行不缩进
            var bookmark = doc.Range.Bookmarks["BridgeDeckStart"];

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
            builder.InsertCell(); builder.Write("1");
            builder.InsertCell();
            builder.InsertCell();
            builder.InsertCell();
            builder.InsertCell();
            builder.InsertCell();


            var field2 = InsertFieldRef(builder, "_Ref11455", "", "");
            field2.InsertHyperlink = true;

            builder.EndRow();
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
            var table = builder.StartTable();

            //计算总的图片数量
            int totalPictureCounts = 0;
            var listDamageSummary = gridTotal.ItemsSource as List<DamageSummary>;
            for(int i=0;i<listDamageSummary.Count;i++)
            {
                totalPictureCounts += listDamageSummary[i].PictureCounts;
            }

            //假定每个病害都有照片
            //将所有图片依次插入word
            for(int i=0;i<= listDamageSummary.Count; i++)
            {
                
                var p=listDamageSummary[i].PictureNo.Split(',');
                for(int j=1;j<=p.Length;j++)
                {
                    builder.InsertCell();
                    CompressImage("Pictures/DSC00855.JPG", "PicturesOut/DSC00855.JPG", 80);
                    builder.InsertImage("PicturesOut/DSC00855.JPG", RelativeHorizontalPosition.Margin, 0, RelativeVerticalPosition.Margin, 0, ImageWidth, ImageHeight, WrapType.Inline);

                    //if (j % 2 == 0 || (j==p.Length && i== listDamageSummary.Count))
                    if (j % 2 == 0 || (j==p.Length && i== listDamageSummary.Count))
                    {
                        builder.EndRow();
                    }
                    builder.InsertCell();
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write("图 ");
                    fieldStyleRefBuilder.BuildAndInsert(table.Rows[1].Cells[0].Paragraphs[0]);
                    builder.Write("-");
                    fieldSequenceBuilder.BuildAndInsert(table.Rows[1].Cells[0].Paragraphs[0]);
                    builder.Write(" 病害描述1");

                    builder.InsertCell();

                    builder.StartBookmark("_Ref11455");
                    builder.Write("图 ");
                    fieldStyleRefBuilder.BuildAndInsert(table.Rows[1].Cells[1].Paragraphs[0]);
                    builder.Write("-");
                    fieldSequenceBuilder.BuildAndInsert(table.Rows[1].Cells[1].Paragraphs[0]);
                    builder.EndBookmark("_Ref11455");

                    builder.Write(" 病害描述2");



                    builder.EndRow();

                }
            }

            // 第1行
            builder.InsertCell();
            builder.InsertImage("Pictures/DSC00855.JPG", RelativeHorizontalPosition.Margin, 0, RelativeVerticalPosition.Margin, 0, ImageWidth, ImageHeight, WrapType.Inline);

            // Insert a cell
            builder.InsertCell();
            builder.InsertImage("Pictures/DSC00858.JPG", RelativeHorizontalPosition.Margin, 0, RelativeVerticalPosition.Margin, 0, ImageWidth, ImageHeight, WrapType.Inline);

            builder.EndRow();

            CompressImage("Pictures/DSC00855.JPG", "PicturesOut/DSC00855.JPG", 80);
            //第2行
            builder.InsertCell();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write("图 ");
            fieldStyleRefBuilder.BuildAndInsert(table.Rows[1].Cells[0].Paragraphs[0]);
            builder.Write("-");
            fieldSequenceBuilder.BuildAndInsert(table.Rows[1].Cells[0].Paragraphs[0]);
            builder.Write(" 病害描述1");

            builder.InsertCell();

            builder.StartBookmark("_Ref11455");
            builder.Write("图 ");
            fieldStyleRefBuilder.BuildAndInsert(table.Rows[1].Cells[1].Paragraphs[0]);
            builder.Write("-");
            fieldSequenceBuilder.BuildAndInsert(table.Rows[1].Cells[1].Paragraphs[0]);
            builder.EndBookmark("_Ref11455");

            builder.Write(" 病害描述2");



            builder.EndRow();

            builder.EndTable();
            table.ClearBorders();

            //builder.MoveTo(table.Rows[1].Cells[1].FirstChild);
            //builder.StartBookmark("_Ref11455");
            //builder.EndBookmark("_Ref11455");

            //builder.MoveTo(summaryTable.Rows[1].Cells[5].FirstChild);
            //var field2 = InsertFieldRef(builder, "_Ref11455", "图2-1", "");
            //field2.InsertHyperlink = true; 

            doc.UpdateFields();
            doc.UpdateFields();

            doc.Save("RegularInspectionTemplate-out.docx", SaveFormat.Docx);

            //var shape = new Shape(doc, ShapeType.Image);
            //shape.ImageData.SetImage("Pictures/DSC00855.JPG");

            //var s = doc.GetChildNodes(NodeType.Shape, true)[0] as Shape;

            //var table0 = doc.GetChildNodes(NodeType.Table, true)[0] as Table;
            //builder.MoveTo(table0.Rows[3].Cells[1].FirstChild);
            //builder.StartBookmark("_Ref11455");
            ////builder.InsertFootnote(FootnoteType.Footnote, "MyBookmark footnote #1");
            ////builder.Write("Text that will appear in REF field");
            ////builder.InsertFootnote(FootnoteType.Footnote, "MyBookmark footnote #2");
            //builder.EndBookmark("_Ref11455");
            //builder.MoveTo(table0.Rows[0].Cells[0].FirstChild);

            ////builder.InsertNode(shape);   //可测试查看效果
            ////https://blog.csdn.net/ly315ly/article/details/88076782
            //builder.InsertImage("Pictures/DSC00855.JPG", RelativeHorizontalPosition.Margin, 0, RelativeVerticalPosition.Margin, 0, 224.25, 168.75, WrapType.Inline);

            //builder.MoveTo(table0.Rows[1].Cells[0].FirstChild);
            //var field = InsertSeqField(builder,"图 ", @" 病害描述","图");
            //builder.MoveTo(table0.Rows[2].Cells[0].FirstChild);
            //var field1 = InsertFieldRef(builder, "_Ref11455", @"before", "after");
            //field1.InsertHyperlink = true; 
            ////word宏录制
            ////            Sub 宏1()
            ////'
            ////' 宏1 宏
            ////'
            ////'
            ////    Selection.InsertCrossReference ReferenceType:= "图", ReferenceKind:= _
            ////        wdEntireCaption, ReferenceItem:= "2", InsertAsHyperlink:= True, _
            ////          IncludePosition:= False, SeparateNumbers:= False, SeparatorString:= " "
            ////End Sub
            //doc.Save("default-out.doc", SaveFormat.Doc);

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
