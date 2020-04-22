using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Repository
{
    public class BCIRepository
    {
        List<DamageSummary> _listDamageSummary;
        BridgePart _bridgePart;
        public BCIRepository(List<DamageSummary> listDamageSummary,BridgePart bridgePart)
        {
            _listDamageSummary = listDamageSummary;
            _bridgePart = bridgePart;
        }

        public decimal GetBCIm()
        {
            return 0.0m;
        }

        public List<PointPenalty> GetPointPenalty()
        {
            var pointPenalty = _listDamageSummary.GroupBy(x => new {
                ComponentCategory=x.GetComponentCategoryName(_bridgePart), DamageCategory = x.GetDamageCategoryName()
            }).Sum(x=>x.FirstOrDefault().SeverityQuantity);

            var query = from p in _listDamageSummary
                        group p by new
                        {
                            ComponentCategory = p.GetComponentCategoryName(_bridgePart),
                            DamageCategory = p.GetDamageCategoryName()
                        }
                        into s
                        let cc= s.Select(r => r.GetComponentCategoryName(_bridgePart)).First()
                        select new PointPenalty
                        {
                            ComponentCategory = s.Select(r => r.GetComponentCategoryName(_bridgePart)).First()
                            ,DamageCategory = s.Select(r => r.GetDamageCategoryName()).First()
                            ,SeverityQuantity = s.Sum(r => r.SeverityQuantity)
                            ,SeverityQuality=s.Max(r=>r.SeverityQuality)
                            ,Penalty=0
                        };

            return query.ToList();

            //foreach (var v1 in pointPenalty)
            //{
            //    return pointPenalty.Select(x => new PointPenalty { }
            //    );
            //}


            //return new List<PointPenalty>();
        }
        /// <summary>
        /// 获取扣分总表（一般用来写计算书用）
        /// </summary>
        /// <returns></returns>
        public List<PointPenalty> GetSumPenalityTable()
        {
            var pointPenalty = _listDamageSummary.GroupBy(x => new {
                ComponentCategory = x.GetComponentCategoryName(_bridgePart),
                DamageCategory = x.GetDamageCategoryName()
            }).Sum(x => x.FirstOrDefault().SeverityQuantity);

            var query = from p in _listDamageSummary
                        group p by new
                        {
                            ComponentCategory = p.GetComponentCategoryName(_bridgePart),
                            DamageCategory = p.GetDamageCategoryName()
                        }
                        into s
                        let cc = s.Select(r => r.GetComponentCategoryName(_bridgePart)).First()
                        select new PointPenalty
                        {
                            ComponentCategory = s.Select(r => r.GetComponentCategoryName(_bridgePart)).First()
                            ,
                            DamageCategory = s.Select(r => r.GetDamageCategoryName()).First()
                            ,
                            SeverityQuantity = s.Sum(r => r.SeverityQuantity)
                            ,
                            SeverityQuality = s.Max(r => r.SeverityQuality)
                            ,
                            Penalty = 0
                        };

            return query.ToList();
        }
    }
}
