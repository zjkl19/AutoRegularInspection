using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    public class PointPenalty
    {
        /// <summary>
        /// 构件分类
        /// </summary>
        public string ComponentCategory { get; set;  }
        /// <summary>
        /// 病害分类
        /// </summary>
        public string DamageCategory { get; set; }
        /// <summary>
        /// 严重程度
        /// </summary>
        //public decimal Severity { get; set; }

        public decimal SeverityQuantity { get; set; }

        public int SeverityQuality { get; set; }


        /// <summary>
        /// 单项扣分
        /// </summary>
        public decimal Penalty { get; set; }
    }
}
