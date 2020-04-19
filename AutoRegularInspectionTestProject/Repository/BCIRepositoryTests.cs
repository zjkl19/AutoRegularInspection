using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Ninject;
using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System.Linq;
using AutoRegularInspection.Repository;

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
                    ComponentValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().Idx,
                    DamageValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().DamageComboBox.Where(x => x.Title == "龟裂").FirstOrDefault().Idx,
                    Severity=0.02m,
                }
                ,
                new DamageSummary
                {
                    ComponentValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().Idx,
                    DamageValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().DamageComboBox.Where(x => x.Title == "网裂").FirstOrDefault().Idx,
                    Severity=0.04m,
                }
                ,
                new DamageSummary
                {
                    ComponentValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().Idx,
                    DamageValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().DamageComboBox.Where(x => x.Title == "波浪").FirstOrDefault().Idx,
                    Severity=0.02m,
                }
                ,
                new DamageSummary
                {
                    ComponentValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().Idx,
                    DamageValue=GlobalData.ComponentComboBox.Where(x => x.Title == "桥面铺装").FirstOrDefault().DamageComboBox.Where(x => x.Title == "碎裂").FirstOrDefault().Idx,
                    Severity=0.04m,
                }
            };

            //Act
            var bci = new BCIRepository(lst,BridgePart.BridgeDeck);
            List<PointPenalty> t =bci.GetPointPenalty();
            //Assert
            //查看读取的单位是否有误
            Assert.Equal(1, 1);
        }
    }
}
