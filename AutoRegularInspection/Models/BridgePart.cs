using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    /// <summary>
    /// 桥梁部位
    /// </summary>
    public enum BridgePart
    {
        [Display(Name = "桥面系")]
        BridgeDeck = 1,
        [Display(Name = "上部结构")]
        SuperSpace = 2,
        [Display(Name = "下部结构")]
        SubSpace = 3,
    }
}
