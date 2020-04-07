using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AutoRegularInspectionTestProject
{
    public class MainWindowTests
    {
        [Fact]
        public void AutoReport_Test()
        {
            //Arrange
            var fileName = @"..\..\..\TestFiles\FindUnitError.doc";

            var log = _fixture.log;
            string xml = @"<?xml version=""1.0"" encoding=""utf - 8"" ?>
                            <configuration>
                              <FindStrainOrDispError row1=""0"" col1=""0"" row2 =""1"" col2 =""1""  charactorString =""测点号"" >
                                <Strain charactorString = ""总应变"" />
                                <Disp charactorString = ""总变形"" />
                             </FindStrainOrDispError >
                           </configuration >";
            var config = XDocument.Parse(xml);

            var ai = new AsposeAIReportCheck(fileName, log.Object, config);
            //Act
            ai.FindUnitError();

            using (MemoryStream dstStream = new MemoryStream())
            {
                ai._doc.Save(dstStream, SaveFormat.Doc);
            }

            Comment docComment = (Comment)ai._doc.GetChild(NodeType.Comment, 0, true);
            Comment docComment1 = (Comment)ai._doc.GetChild(NodeType.Comment, 1, true);

            NodeCollection allComments = ai._doc.GetChildNodes(NodeType.Comment, true);

            //Assert
            Assert.Equal(2, allComments.Count);
            Assert.Equal(1, docComment.Count);
            Assert.True(docComment.GetText().IndexOf("应为km/h") >= 0);
            Assert.True(docComment1.GetText().IndexOf("应为km/h") >= 0);
            //Assert.Equal("\u0005My comment.\r", docComment.GetText());
        }
    }
}
