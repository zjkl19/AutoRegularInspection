using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Fields;
using Aspose.Words.Tables;
using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoRegularInspection.Services
{
    public class AsposeWordsServices
    {
        private Document _doc;
        public AsposeWordsServices(ref Document doc)
        {
            _doc = doc;
        }
        public void InsertSummaryAndPictureTable(string BookmarkStartName, int CompressImageFlag, List<DamageSummary> listDamageSummary, double ImageWidth, double ImageHeight)
        {
            var builder = new DocumentBuilder(_doc);

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
            var bookmark = _doc.Range.Bookmarks[BookmarkStartName];

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
                        if (!File.Exists($"PicturesOut/{Path.GetFileName(dirs[0])}"))
                        {
                            ImageServices.CompressImage($"{dirs[0]}", $"PicturesOut/{Path.GetFileName(dirs[0])}", CompressImageFlag);    //只取查找到的第1个文件，TODO：UI提示       
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
        
    }
}
