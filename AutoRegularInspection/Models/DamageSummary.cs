using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    /// <summary>
    /// 病害汇总
    /// </summary>
    public class DamageSummary
    {
        /// <summary>
        /// 位置
        /// </summary>
        public string Position { set; get; }

        /// <summary>
        /// 构件类型
        /// </summary>
        public string Component { set; get; }

        /// <summary>
        /// 损坏类型
        /// </summary>
        public string Damage { set; get; }

        /// <summary>
        /// 病害描述
        /// </summary>
        public string DamageDescription { set; get; }

        /// <summary>
        /// 病害对应图片描述
        /// </summary>
        public string DamageDescriptionInPicture { set; get; }

        /// <summary>
        /// 病害对应图片编号
        /// </summary>
        public string PictureNo { set; get; }
    }
}
