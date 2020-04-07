using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using AutoRegularInspection.Services;
using AutoRegularInspection.Models;
using AutoRegularInspection.ViewModels;
using System.Linq;

namespace AutoRegularInspectionTestProject
{
    public class MainWindowTests
    {
        [Fact]
        public void GenerateReport_Test()
        {
            //Arrange
            //string fileName = @"..\..\..\TestFiles\FindUnitError.doc";
            string templateFile = @"..\..\..\TestFiles\外观检查报告模板.docx";
            string outputFile = @"..\..\..\外观检查报告.docx";
            double ImageWidth = 224.25; double ImageHeight = 168.75; int CompressImageFlag = 80;
            var l1 = new GridViewModel().GridSource.GridData;
            var l2 = new GridViewModel(BridgePart.SuperSpace).GridSource.GridData;
            var l3 = new GridViewModel(BridgePart.SubSpace).GridSource.GridData;

            //Act
            var doc = new Document(templateFile);
            var asposeService = new AsposeWordsServices(ref doc, l1.ToList(), l2.ToList(), l3.ToList());
            asposeService.GenerateSummaryTableAndPictureTable(ImageWidth, ImageHeight, CompressImageFlag);

            doc.UpdateFields();
            doc.UpdateFields();

            doc.Save(outputFile, SaveFormat.Docx);

            //Assert
            Assert.Equal(2, 3);
        }
    }
}
