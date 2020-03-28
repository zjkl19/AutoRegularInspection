using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AutoRegularInspection.Models
{
    /// <summary>
    /// 桥面系-要素
    /// </summary>
    public enum BridgeDeckEnum
    {
        [Display(Name = "桥面铺装")]
        DeckPavement = 1,
        [Display(Name = "桥头平顺")]
        SuperSpace = 2,
        [Display(Name = "伸缩缝")]
        ExpansionJoint = 3,
        [Display(Name = "排水系统")]
        SubSpace1 = 4,
        [Display(Name = "栏杆")]
        Handrail = 5,
        [Display(Name = "其它")]
        Others = 99,
    }
}
