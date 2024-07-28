
using System.Windows;
using System;
using System.IO;
using System.Diagnostics;
using System.Text; // 引入文本命名空间以访问编码类
using Aspose.Words;
using Aspose.Words.Tables;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
       
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            builder.StartTable();
            builder.RowFormat.HeadingFormat = true;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.Width = 100;
            builder.InsertCell();
            builder.Writeln("Heading row 1");
            builder.EndRow();
            builder.InsertCell();
            builder.Writeln("Heading row 2");
            builder.EndRow();

            builder.CellFormat.Width = 50;
            builder.ParagraphFormat.ClearFormatting();

            // Insert some content so the table is long enough to continue onto the next page
            for (int i = 0; i < 50; i++)
            {
                builder.InsertCell();
                builder.RowFormat.HeadingFormat = false;
                builder.Write("Column 1 Text");
                builder.InsertCell();
                builder.Write("Column 2 Text");
                builder.EndRow();
            }

            doc.Save("DocumentBuilder.InsertTableSetHeadingRow.docx");
        }


    }
}
