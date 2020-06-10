using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Fields;
using Aspose.Words.Tables;
using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
            , List<DamageSummary> bridgeDeckListDamageSummary
            , List<DamageSummary> superSpaceListDamageSummary
            , List<DamageSummary> subSpaceListDamageSummary)
        {
            _doc = doc;
            _bridgeDeckListDamageSummary = bridgeDeckListDamageSummary;
            _superSpaceListDamageSummary = superSpaceListDamageSummary;
            _subSpaceListDamageSummary = subSpaceListDamageSummary;
        }
        /// <summary>
        /// 生成汇总表以及插入图片
        /// </summary>
        /// <param name="progressModel">进度条数据绑定模型</param>
        /// <param name="ImageWidth"></param>
        /// <param name="ImageHeight"></param>
        /// <param name="CompressImageFlag"></param>
        public void GenerateSummaryTableAndPictureTable(ref ProgressBarModel progressModel, bool CommentColumnInsertTable, double ImageWidth = 224.25, double ImageHeight = 168.75, int CompressImageFlag = 70)
        {
            progressModel.ProgressValue = 0;

            InsertSummaryWords();
            progressModel.Content = "正在处理桥面系……";
            InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, CompressImageFlag, _bridgeDeckListDamageSummary, ImageWidth, ImageHeight, CommentColumnInsertTable);
            progressModel.Content = "正在处理上部结构……";
            progressModel.ProgressValue = 33;
            System.Threading.Thread.Sleep(500);

            InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, CompressImageFlag, _superSpaceListDamageSummary, ImageWidth, ImageHeight, CommentColumnInsertTable);
            progressModel.Content = "正在处理下部结构……";
            progressModel.ProgressValue = 66;
            System.Threading.Thread.Sleep(500);

            InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, CompressImageFlag, _subSpaceListDamageSummary, ImageWidth, ImageHeight, CommentColumnInsertTable);
            progressModel.ProgressValue = 100;
            progressModel.Content = "正在完成……";
        }

        public void GenerateSummaryTableAndPictureTable(bool CommentColumnInsertTable,double ImageWidth = 224.25, double ImageHeight = 168.75, int CompressImageFlag = 70)
        {
            InsertSummaryWords();
            InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, CompressImageFlag, _bridgeDeckListDamageSummary, ImageWidth, ImageHeight, CommentColumnInsertTable);
            InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, CompressImageFlag, _superSpaceListDamageSummary, ImageWidth, ImageHeight, CommentColumnInsertTable);
            InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, CompressImageFlag, _subSpaceListDamageSummary, ImageWidth, ImageHeight, CommentColumnInsertTable);
        }
        /// <summary>
        /// 插入总结描述，一般插入点在文档开头，统计病害数量等
        /// </summary>
        private void InsertSummaryWords()
        {
            //IEnumerable<IGrouping<ComponentDamageGroup,DamageSummary>> bridgeDeckDamageStatistics = _bridgeDeckListDamageSummary.Where(x => x.GetUnit1() != "无").GroupBy(x =>new ComponentDamageGroup { ComponentName = x.GetComponentName(), DamageName = x.GetDamageName() });
            //IEnumerable<IGrouping<ComponentDamageGroup, DamageSummary>> superSpaceDamageStatistics = _superSpaceListDamageSummary.Where(x => x.GetUnit1() != "无").GroupBy(x => new ComponentDamageGroup { ComponentName = x.GetComponentName(BridgePart.SuperSpace), DamageName = x.GetDamageName(BridgePart.SuperSpace) });
            //IEnumerable<IGrouping<ComponentDamageGroup, DamageSummary>> subSpaceDamageStatistics = _subSpaceListDamageSummary.Where(x => x.GetUnit1() != "无").GroupBy(x => new ComponentDamageGroup  { ComponentName = x.GetComponentName(BridgePart.SubSpace), DamageName = x.GetDamageName(BridgePart.SubSpace) });

            var bridgeDeckDamageStatistics = _bridgeDeckListDamageSummary.Where(x => x.GetUnit1() != "无").GroupBy(x => new  { ComponentName = x.GetComponentName(), DamageName = x.GetDamageName() });
            var superSpaceDamageStatistics = _superSpaceListDamageSummary.Where(x => x.GetUnit1() != "无").GroupBy(x => new  { ComponentName = x.GetComponentName(BridgePart.SuperSpace), DamageName = x.GetDamageName(BridgePart.SuperSpace) });
            var subSpaceDamageStatistics = _subSpaceListDamageSummary.Where(x => x.GetUnit1() != "无").GroupBy(x => new { ComponentName = x.GetComponentName(BridgePart.SubSpace), DamageName = x.GetDamageName(BridgePart.SubSpace) });


            var builder = new DocumentBuilder(_doc);
            var bookmark = _doc.Range.Bookmarks["BridgeDeckSummaryStart"];

            builder.MoveTo(bookmark.BookmarkStart);
            builder.Write(GenerateInsertText(bridgeDeckDamageStatistics,BridgePart.BridgeDeck));
            builder.Writeln();

            bookmark = _doc.Range.Bookmarks["SuperSpaceSummaryStart"];
            builder.MoveTo(bookmark.BookmarkStart);
            builder.Write(GenerateInsertText(superSpaceDamageStatistics,BridgePart.SuperSpace));
            builder.Writeln();

            bookmark = _doc.Range.Bookmarks["SubSpaceSummaryStart"];
            builder.MoveTo(bookmark.BookmarkStart);
            builder.Write(GenerateInsertText(subSpaceDamageStatistics,BridgePart.SubSpace));
            builder.Writeln();

            string GenerateInsertText(IEnumerable<IGrouping<dynamic, DamageSummary>> damageStatistics, BridgePart bridgePart)
            {
                int i = 0;
                string insertText = string.Empty;
                string previousComponent = string.Empty; string currComponent = string.Empty;
                if (damageStatistics.Any())
                {

                    foreach (var v1 in damageStatistics)
                    {
                        currComponent = v1.Key.ComponentName.ToString(CultureInfo.InvariantCulture);
                        if (currComponent != previousComponent)
                        {
                            if (i != 0)
                            {
                                insertText = $"{insertText}。";
                            }
                            if (v1.FirstOrDefault().GetUnit2() != "无")
                            {
                                insertText = $"{insertText}\n{v1.Key.ComponentName.ToString(CultureInfo.InvariantCulture)}：共{v1.Sum(x => x.Unit1Counts)}{v1.FirstOrDefault().GetDisplayUnit1()}{v1.FirstOrDefault().GetDamageName(bridgePart)}，{v1.FirstOrDefault().GetPhysicalItem()}{v1.Sum(x => x.Unit2Counts)}{v1.FirstOrDefault().GetUnit2()}";
                            }
                            else
                            {
                                insertText = $"{insertText}\n{v1.Key.ComponentName.ToString(CultureInfo.InvariantCulture)}：共{v1.Sum(x => x.Unit1Counts)}{v1.FirstOrDefault().GetDisplayUnit1()}{v1.FirstOrDefault().GetDamageName(bridgePart)}";
                            }

                        }
                        else
                        {
                            insertText = $"{insertText}；";
                            if (v1.FirstOrDefault().GetUnit2() != "无")
                            {
                                insertText = $"{insertText}共{v1.Sum(x => x.Unit1Counts)}{v1.FirstOrDefault().GetDisplayUnit1()}{v1.FirstOrDefault().GetDamageName(bridgePart)}，{v1.FirstOrDefault().GetPhysicalItem()}{v1.Sum(x => x.Unit2Counts)}{v1.FirstOrDefault().GetUnit2()}";
                            }
                            else
                            {
                                insertText = $"{insertText}共{v1.Sum(x => x.Unit1Counts)}{v1.FirstOrDefault().GetDisplayUnit1()}{v1.FirstOrDefault().GetDamageName(bridgePart)}";
                            }
                        }

                        if (i == damageStatistics.Count() - 1)
                        {
                            insertText = $"{insertText}。";
                        }

                        previousComponent = currComponent;
                        i++;
                    }
                }
                return insertText;
            }

        }

        private void InsertSummaryAndPictureTable(string BookmarkStartName, int CompressImageFlag, List<DamageSummary> listDamageSummary, double ImageWidth, double ImageHeight,bool CommentColumnInsertTable)
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

            //TODO：考虑一下具体的缩进值
            //builder.ParagraphFormat.FirstLineIndent = 8;
            
            //要求：至少要有2张照片
            if (BookmarkStartName == BridgeDeckBookmarkStartName)
            {
                builder.Write("桥面系检查结果详见");
            }
            else if (BookmarkStartName == SuperSpaceBookmarkStartName)
            {
                builder.Write("上部结构检查结果详见");
            }
            else
            {
                builder.Write("下部结构检查结果详见");
            }
            int firstIndex = 0;int lastIndex = listDamageSummary.Count-1;
            //查找第一张
            while(listDamageSummary[firstIndex].PictureCounts==0 && firstIndex< listDamageSummary.Count - 1)
            {
                firstIndex++;
            }
            //查找最后一张
            while (listDamageSummary[lastIndex].PictureCounts == 0 && lastIndex >0)
            {
                lastIndex--;
            }

            //TODO：考虑表格第1行和最后1行可能没有照片
            pictureRefField = InsertFieldRef(builder, $"_Ref{listDamageSummary[0].FirstPictureBookmarkIndex}", "", "");
            pictureRefField.InsertHyperlink = true;
            builder.Write("～");
            pictureRefField = InsertFieldRef(builder, $"_Ref{listDamageSummary.Last().FirstPictureBookmarkIndex + listDamageSummary.Last().PictureCounts - 1}", "", "");
            pictureRefField.InsertHyperlink = true;
            builder.Write("。");
            builder.Writeln();

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
            builder.InsertCell();

            BridgePart bridgePart;

            if (BookmarkStartName == BridgeDeckBookmarkStartName)
            {
                builder.Write("要素");
                bridgePart = BridgePart.BridgeDeck;

            }
            else if (BookmarkStartName == SuperSpaceBookmarkStartName)
            {
                builder.Write("构件类型");
                bridgePart = BridgePart.SuperSpace;
            }
            else
            {
                builder.Write("构件类型");
                bridgePart = BridgePart.SubSpace;
            }

            builder.InsertCell(); builder.Write("缺损类型");
            builder.InsertCell(); builder.Write("缺损描述");
            builder.InsertCell(); builder.Write("图示编号");

            if(CommentColumnInsertTable)
            {
                builder.InsertCell(); builder.Write("备注");
            }

            builder.Font.Bold = false;
            builder.EndRow();

            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                builder.InsertCell(); builder.Write($"{i + 1}");
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].Position}");
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].GetComponentName(bridgePart)}");
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].GetDamageName(bridgePart).Replace("m2", "m\u00B2").Replace("m3", "m\u00B3")}");    //\u00B2是2的上标,\u00B3是3的上标
                builder.InsertCell(); builder.Write($"{listDamageSummary[i].DamageDescription.Replace("m2", "m\u00B2").Replace("m3", "m\u00B3")}");
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
                if (CommentColumnInsertTable)
                {
                    builder.InsertCell(); builder.Write($"{listDamageSummary[i].Comment}");
                }
                builder.EndRow();
            }


            builder.EndTable();

            //TODO:用建造者模式重构
            MergeDamageColumn(listDamageSummary,summaryTable);
            MergeComponentColumn(listDamageSummary, summaryTable);
            MergeTheSameColumn(listDamageSummary, summaryTable,1);

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

                        if(listDamageSummary[i].PictureCounts>1)
                        {
                            builder.Write($" {listDamageSummary[i].DamageDescriptionInPicture}-{j + 1}");
                        }
                        else
                        {
                            builder.Write($" {listDamageSummary[i].DamageDescriptionInPicture}");
                        }

                        curr++;
                    }
                }
            }


            pictureTable.ClearBorders();
        }

        /// <summary>
        /// 合并缺损类型1列
        /// </summary>
        /// <param name="listDamageSummary">病害列表</param>
        /// <param name="summaryTable">word中的汇总表（Aspose.words格式）</param>
        private void MergeDamageColumn(List<DamageSummary> listDamageSummary, Table summaryTable)
        {
            int damageColumn = 3;    //缺损类型所在列(Aspose.words)
            //先合并“缺损类型”列
            //合并算法：
            //1、先找出合并起始行和最后一行
            int mergeLength = 0;     //合并长度

            //i==0时，对应表格第i+1行（Aspose.Words中的行，人为认识的第2行）
            for (int i = 0; i < listDamageSummary.Count-1; i++)
            {
                for(int j=i+1;j<listDamageSummary.Count;j++)
                {
                    //缺损类型列相同并且构件类型相同
                    if(listDamageSummary[i].Damage == listDamageSummary[j].Damage
                        && listDamageSummary[i].Component == listDamageSummary[j].Component
                        && listDamageSummary[i].Position == listDamageSummary[j].Position)
                    {
                        mergeLength++;
                    }
                    else
                    {
                        break;
                    }
                }
                if(mergeLength>0)
                {
                    var cellStartRange = summaryTable.Rows[i+1].Cells[damageColumn];
                    var cellEndRange = summaryTable.Rows[i+1+mergeLength].Cells[damageColumn];
                    MergeCells(cellStartRange, cellEndRange);
                    i += mergeLength;    //i要跳过
                    mergeLength = 0;    //合并单元格后归0
                   
                }
            }

        }

        /// <summary>
        /// 合并构件类型1列
        /// </summary>
        /// <param name="listDamageSummary">病害列表</param>
        /// <param name="summaryTable">word中的汇总表（Aspose.words格式）</param>
        private void MergeComponentColumn(List<DamageSummary> listDamageSummary, Table summaryTable)
        {
            int componentColumn = 2;    //构件类型所在列(Aspose.words)

            //合并算法：
            //1、先找出合并起始行和最后一行
            int mergeLength = 0;     //合并长度

            //i==0时，对应表格第i+1行（Aspose.Words中的行，人为认识的第2行）
            for (int i = 0; i < listDamageSummary.Count - 1; i++)
            {
                for (int j = i + 1; j < listDamageSummary.Count; j++)
                {
                    //缺损类型列相同并且构件类型相同
                    if (listDamageSummary[i].Component == listDamageSummary[j].Component
                        && listDamageSummary[i].Position == listDamageSummary[j].Position)
                    {
                        mergeLength++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (mergeLength > 0)
                {
                    var cellStartRange = summaryTable.Rows[i + 1].Cells[componentColumn];
                    var cellEndRange = summaryTable.Rows[i + 1 + mergeLength].Cells[componentColumn];
                    MergeCells(cellStartRange, cellEndRange);
                    i += mergeLength;    //i要跳过
                    mergeLength = 0;    //合并单元格后归0

                }
            }

        }

        /// <summary>
        /// 合并竖向内容相同的列
        /// </summary>
        /// <param name="listDamageSummary">病害列表</param>
        /// <param name="summaryTable">word中的汇总表（Aspose.words格式）</param>
        /// <param name="mergedColumn">word中的汇总表需要合并的列（Aspose.words格式）</param>
        private void MergeTheSameColumn(List<DamageSummary> listDamageSummary, Table summaryTable ,int mergedColumn=1)
        {
            //合并算法：
            //1、先找出合并起始行和最后一行
            int mergeLength = 0;     //合并长度

            //i==0时，对应表格第i+1行（Aspose.Words中的行，人为认识的第2行）
            for (int i = 0; i < listDamageSummary.Count - 1; i++)
            {
                for (int j = i + 1; j < listDamageSummary.Count; j++)
                {
                    if (listDamageSummary[i].Position == listDamageSummary[j].Position)
                    {
                        mergeLength++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (mergeLength > 0)
                {
                    var cellStartRange = summaryTable.Rows[i + 1].Cells[mergedColumn];
                    var cellEndRange = summaryTable.Rows[i + 1 + mergeLength].Cells[mergedColumn];
                    MergeCells(cellStartRange, cellEndRange);
                    i += mergeLength;    //i要跳过
                    mergeLength = 0;    //合并单元格后归0

                }
            }

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
