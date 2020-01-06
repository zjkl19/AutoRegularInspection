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
        private List<DamageSummary> _bridgeDeckListDamageSummary;
        private List<DamageSummary> _superSpaceListDamageSummary;
        private List<DamageSummary> _subSpaceListDamageSummary;
        readonly string BridgeDeckBookmarkStartName = "BridgeDeckStart";
        readonly string SuperSpaceBookmarkStartName = "SuperSpaceStart";
        readonly string SubSpaceBookmarkStartName = "SubSpaceStart";

        public AsposeWordsServices(ref Document doc
            , ref List<DamageSummary> bridgeDeckListDamageSummary
            , ref List<DamageSummary> superSpaceListDamageSummary
            , ref List<DamageSummary> subSpaceListDamageSummary)
        {
            _doc = doc;
            _bridgeDeckListDamageSummary = bridgeDeckListDamageSummary;
            _superSpaceListDamageSummary = superSpaceListDamageSummary;
            _subSpaceListDamageSummary = subSpaceListDamageSummary;
        }

        public void GenerateSummaryTableAndPictureTable(double ImageWidth = 224.25, double ImageHeight = 168.75, int CompressImageFlag = 70)
        {


            InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, CompressImageFlag, _bridgeDeckListDamageSummary, ImageWidth, ImageHeight);
            InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, CompressImageFlag, _superSpaceListDamageSummary, ImageWidth, ImageHeight);
            InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, CompressImageFlag, _subSpaceListDamageSummary, ImageWidth, ImageHeight);
        }

        private void InsertSummaryAndPictureTable(string BookmarkStartName, int CompressImageFlag, List<DamageSummary> listDamageSummary, double ImageWidth, double ImageHeight)
        {
            var builder = new DocumentBuilder(_doc);

            var fieldStyleRefBuilder = new FieldBuilder(FieldType.FieldStyleRef);
            fieldStyleRefBuilder.AddArgument(1);
            fieldStyleRefBuilder.AddSwitch(@"\s");

            var pictureFieldSequenceBuilder = new FieldBuilder(FieldType.FieldSequence);
            pictureFieldSequenceBuilder.AddArgument("图");
            pictureFieldSequenceBuilder.AddSwitch(@"\*", "ARABIC");
            pictureFieldSequenceBuilder.AddSwitch(@"\s", "1");

            var tableFieldSequenceBuilder = new FieldBuilder(FieldType.FieldSequence);
            tableFieldSequenceBuilder.AddArgument("表");
            tableFieldSequenceBuilder.AddSwitch(@"\*", "ARABIC");
            tableFieldSequenceBuilder.AddSwitch(@"\s", "1");

            //_Refxx的书签不会在word的“插入”=>“书签”中显示

            FieldRef pictureRefField;

            //模板在书签位置格式调整
            //1、单倍行距
            //2、首行不缩进
            var bookmark = _doc.Range.Bookmarks[BookmarkStartName];

            builder.MoveTo(bookmark.BookmarkStart);

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            builder.Write("表 ");
            var r1 = new Run(_doc, "");
            builder.InsertNode(r1);
            fieldStyleRefBuilder.BuildAndInsert(r1);
            builder.Write("-");
            var r2 = new Run(_doc, "");
            builder.InsertNode(r2);
            tableFieldSequenceBuilder.BuildAndInsert(r2);
            builder.Write(" ");

            //写入表头
            if (BookmarkStartName == BridgeDeckBookmarkStartName)
            {
                builder.Write("桥面系检查结果汇总表");

            }
            else if (BookmarkStartName == SuperSpaceBookmarkStartName)
            {
                builder.Write("上部结构检查结果汇总表");
            }
            else
            {
                builder.Write("下部结构检查结果汇总表");
            }

            builder.Writeln();
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
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

            //先合并“缺损类型”列
            //合并算法：
            //1、先找出合并起始行和最后一行
            #region t1
            int col = 2;    //第3列
            int refCol;    //参考列，默认为前一列（位置列）
            refCol = col - 1;
            int totalRows; totalRows = listDamageSummary.Count+1;
            int mergeLength;    //合并长度
            mergeLength = 0;
            int currRow;    //当前行
            string currContent;    //当前单元格的内容
            string currRefContent;    //当前参考列的内容

            int startRow = 1;    //第2行


    //逻辑较复杂，需作图理解算法
    currRow = startRow;
            for (int i1=startRow;i1<totalRows;i1++)
            {
                mergeLength = 0;
                currContent = summaryTable.Rows[currRow].Cells[col].Range.Text; currRefContent = summaryTable.Cell(currRow, refCol).Range.Text
            }
    For i = startRow To totalRows -1
        mergeLength = 0
        currContent = tbl.Cell(currRow, col).Range.Text: currRefContent = tbl.Cell(currRow, refCol).Range.Text


        '计算合并的长度
        For j = currRow + 1 To totalRows
            If currContent = tbl.Cell(j, col).Range.Text And currRefContent = tbl.Cell(j, refCol).Range.Text Then
                mergeLength = mergeLength + 1
            Else
                Exit For
            End If
        Next j

        If mergeLength <> 0 Then
            tbl.Cell(currRow, col).Merge tbl.Cell(currRow + mergeLength, col)
            tbl.Cell(currRow, col).Range.Text = Mid(currContent, 1, Len(currContent) - 2)   '最后一个回车换行符去掉（CrLf占两个位置）
            currRow = currRow + mergeLength
        End If


        currRow = currRow + 1    '无论是否合并，都+1


        If currRow >= totalRows Then
            Exit For
        End If
    Next i
            #endregion
            var cellStartRange = summaryTable.Rows[1].Cells[1];
            var cellEndRange = summaryTable.Rows[2].Cells[1];

            // Merge all the cells between the two specified cells into one
            MergeCells(cellStartRange, cellEndRange);

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
                        pictureFieldSequenceBuilder.BuildAndInsert(pictureTable.Rows[2 * (int)(curr / 2) + 1].Cells[(curr) % 2].Paragraphs[0]);
                        builder.EndBookmark($"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex + j}");
                        builder.Write($" {listDamageSummary[i].DamageDescriptionInPicture}-{j + 1}");


                        curr++;
                    }
                }
            }


            pictureTable.ClearBorders();
        }

        /// <summary>
        /// Merges the range of cells found between the two specified cells both horizontally and vertically. Can span over multiple rows.
        /// </summary>
        ///参考：https://github.com/aspose-words/Aspose.Words-for-.NET/blob/22d6889ef1ee0d3f1f69a129aa46fef6644048b0/ApiExamples/CSharp/ApiExamples/ExTable.cs
        internal static void MergeCells(Cell startCell, Cell endCell)
        {
            Table parentTable = startCell.ParentRow.ParentTable;

            // Find the row and cell indices for the start and end cell.
            Point startCellPos = new Point(startCell.ParentRow.IndexOf(startCell), parentTable.IndexOf(startCell.ParentRow));
            Point endCellPos = new Point(endCell.ParentRow.IndexOf(endCell), parentTable.IndexOf(endCell.ParentRow));
            // Create the range of cells to be merged based off these indices. Inverse each index if the end cell if before the start cell. 
            Rectangle mergeRange = new Rectangle(Math.Min(startCellPos.X, endCellPos.X), Math.Min(startCellPos.Y, endCellPos.Y),
                Math.Abs(endCellPos.X - startCellPos.X) + 1, Math.Abs(endCellPos.Y - startCellPos.Y) + 1);

            foreach (Row row in parentTable.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    Point currentPos = new Point(row.IndexOf(cell), parentTable.IndexOf(row));

                    // Check if the current cell is inside our merge range then merge it.
                    if (mergeRange.Contains(currentPos))
                    {
                        if (currentPos.X == mergeRange.X)
                            cell.CellFormat.HorizontalMerge = CellMerge.First;
                        else
                            cell.CellFormat.HorizontalMerge = CellMerge.Previous;

                        if (currentPos.Y == mergeRange.Y)
                            cell.CellFormat.VerticalMerge = CellMerge.First;
                        else
                            cell.CellFormat.VerticalMerge = CellMerge.Previous;
                    }
                }
            }
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
