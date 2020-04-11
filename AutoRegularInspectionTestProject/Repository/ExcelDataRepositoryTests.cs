using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Ninject;
using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;

namespace AutoRegularInspectionTestProject.Repository
{
    public class ExcelDataRepositoryTests
    {
        [Fact]
        public void ReadDamageData_ShouldGetCorretVar()
        {
            //Arrange
            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            List<DamageSummary> lst;

            //Act

            lst = dataRepository.ReadDamageData(BridgePart.BridgeDeck);

            //Assert
            //查看读取的单位是否有误
            Assert.Equal("条", lst[0].Unit1);
            Assert.Equal(1, lst[0].Unit1Counts);
            Assert.Equal("米", lst[0].Unit2);
            Assert.Equal(11.2m, lst[0].Unit2Counts);
            Assert.Equal(string.Empty, lst[4].Unit1);
            Assert.Equal(0, lst[4].Unit1Counts);
            Assert.Equal(string.Empty, lst[4].Unit2);
            Assert.Equal(0, lst[4].Unit2Counts);
        }
    }
}
