using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    public class GenerateReportSettings
    {
        public bool DeletePositionInBridgeDeckCheckBox { get; set; }

        public bool CustomTableCellWidth { get; set; }

        public TableCellWidth BridgeDeckTableCellWidth { get; set; }

        public TableCellWidth SuperSpaceTableCellWidth { get; set; }

        public TableCellWidth SubSpaceTableCellWidth { get; set; }

    }

    public class TableCellWidth
    {
        public double No { set; get; }

        public double Position { set; get; }

        public double Component { set; get; }

        public double Damage { set; get; }

        public double DamageDescription { get; set; }

        public double PictureNo { get; set; }

        public double Comment { get; set; }
    }
}
