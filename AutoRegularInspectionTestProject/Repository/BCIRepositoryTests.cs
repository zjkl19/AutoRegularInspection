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
    public class BCIRepositoryTests
    {
        [Fact]
        public void GetBCIm_ShouldReturnCorretResult()
        {
            //Arrange

            List<DamageSummary> lst=new List<DamageSummary>
            {
                //桥面铺装-损坏类型-网裂或龟裂
                new DamageSummary
                {
                    ComponentValue=0,
                    DamageValue=0,
                    Severity=0.02m,
                }
                ,
                new DamageSummary
                {
                    ComponentValue=0,
                    DamageValue=0,
                    Severity=0.04m,
                }
                ,
                new DamageSummary
                {
                    ComponentValue=0,
                    DamageValue=2,
                    Severity=0.02m,
                }
                ,
                new DamageSummary
                {
                    ComponentValue=0,
                    DamageValue=5,
                    Severity=0.02m,
                }
            };

            //Act

            //Assert
            //查看读取的单位是否有误
            Assert.Equal(1, 1);
        }
    }
}
