//using Aspose.Words;
//using Aspose.Words.Drawing;
//using Aspose.Words.Fields;
//using Aspose.Words.Tables;
//using System.Windows;

//namespace AutoRegularInspection
//{
//    /// <summary>
//    /// MainWindow.xaml 的交互逻辑
//    /// </summary>
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            //InitializeComponent();
//            StartMain();
//        }

//        public void StartMain()
//        {
//            var doc = new Document("default.docx");
//            var builder = new DocumentBuilder(doc);
//            var shape = new Shape(doc, ShapeType.Image);
//            shape.ImageData.SetImage("Pictures/DSC00855.JPG");

//            var s = doc.GetChildNodes(NodeType.Shape, true)[0] as Shape;

//            var table0 = doc.GetChildNodes(NodeType.Table, true)[0] as Table;
//            builder.MoveTo(table0.Rows[3].Cells[1].FirstChild);
//            builder.StartBookmark("_Ref11455");
//            //builder.InsertFootnote(FootnoteType.Footnote, "MyBookmark footnote #1");
//            //builder.Write("Text that will appear in REF field");
//            //builder.InsertFootnote(FootnoteType.Footnote, "MyBookmark footnote #2");
//            builder.EndBookmark("_Ref11455");
//            builder.MoveTo(table0.Rows[0].Cells[0].FirstChild);

//            //builder.InsertNode(shape);   //可测试查看效果
//            //https://blog.csdn.net/ly315ly/article/details/88076782
//            builder.InsertImage("Pictures/DSC00855.JPG", RelativeHorizontalPosition.Margin, 0, RelativeVerticalPosition.Margin, 0, 224.25, 168.75, WrapType.Inline);

//            builder.MoveTo(table0.Rows[1].Cells[0].FirstChild);
//            var field = InsertSeqField(builder, "图 ", @" 病害描述", "图");
//            builder.MoveTo(table0.Rows[2].Cells[0].FirstChild);
//            var field1 = InsertFieldRef(builder, "_Ref11455", @"before", "after");
//            field1.InsertHyperlink = true;
//            //word宏录制
//            //            Sub 宏1()
//            //'
//            //' 宏1 宏
//            //'
//            //'
//            //    Selection.InsertCrossReference ReferenceType:= "图", ReferenceKind:= _
//            //        wdEntireCaption, ReferenceItem:= "2", InsertAsHyperlink:= True, _
//            //          IncludePosition:= False, SeparateNumbers:= False, SeparatorString:= " "
//            //End Sub
//            doc.Save("default-out.doc", SaveFormat.Doc);

//        }

//        /// <summary>
//        /// Insert a sequence field with preceding text and a specified sequence identifier
//        /// </summary>
//        public FieldSeq InsertSeqField(DocumentBuilder builder, string textBefore, string textAfter, string sequenceIdentifier)
//        {
//            builder.Write(textBefore);
//            FieldSeq fieldSeq = (FieldSeq)builder.InsertField(FieldType.FieldSequence, true);
//            fieldSeq.SequenceIdentifier = sequenceIdentifier;
//            builder.Write(textAfter);

//            return fieldSeq;
//        }

//        /// <summary>
//        /// Get the document builder to insert a REF field, reference a bookmark with it, and add text before and after
//        /// </summary>
//        private FieldRef InsertFieldRef(DocumentBuilder builder, string bookmarkName, string textBefore, string textAfter)
//        {
//            builder.Write(textBefore);
//            FieldRef field = (FieldRef)builder.InsertField(FieldType.FieldRef, true);
//            field.BookmarkName = bookmarkName;
//            builder.Write(textAfter);
//            return field;
//        }
//    }
//}
