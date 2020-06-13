using System;
using System.Collections.Generic;
using AutoRegularInspection.Services;
using Xunit;

namespace AutoRegularInspectionTestProject.Services
{
    public class FileServiceTests
    {
        [Fact]
        public void GetFileName_ShouldReturnCorrectFileName()
        {
 
            //Arrange

            string folderName = @"Pictures";
            string pictureNo = @"2732";
            string expectedFullName = @"Pictures/DSC02732.JPG";

            //Act
            string fullName=FileService.GetFileName(folderName, pictureNo);

            //Assert

            Assert.Equal(expectedFullName, fullName);

        }
    }
}
