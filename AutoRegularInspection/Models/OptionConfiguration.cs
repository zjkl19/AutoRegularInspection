using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoRegularInspection.Models
{
    [XmlRoot(ElementName = "configuration")]
    public class OptionConfiguration : UpdatePropertyAndOnPropertyChangedBase, INotifyPropertyChanged
    {
        private Picture _Picture;
        [XmlElement(ElementName = "Picture")]
        public Picture Picture
        {
            get { return _Picture; }
            set
            {
                UpdateProperty(ref _Picture, value);
            }
        }
        private Bookmark _Bookmark;
        [XmlElement(ElementName = "Bookmark")]
        public Bookmark Bookmark
        {
            get { return _Bookmark; }
            set
            {
                UpdateProperty(ref _Bookmark, value);
            }
        }
        private BridgeDeckSummaryTable _BridgeDeckSummaryTable;
        [XmlElement(ElementName = "BridgeDeckSummaryTable")]
        public BridgeDeckSummaryTable BridgeDeckSummaryTable
        {
            get { return _BridgeDeckSummaryTable; }
            set
            {
                UpdateProperty(ref _BridgeDeckSummaryTable, value);
            }
        }
        private SuperSpaceSummaryTable _SuperSpaceSummaryTable;
        [XmlElement(ElementName = "SuperSpaceSummaryTable")]
        public SuperSpaceSummaryTable SuperSpaceSummaryTable
        {
            get { return _SuperSpaceSummaryTable; }
            set
            {
                UpdateProperty(ref _SuperSpaceSummaryTable, value);
            }
        }

        private SubSpaceSummaryTable _SubSpaceSummaryTable;
        [XmlElement(ElementName = "SubSpaceSummaryTable")]
        public SubSpaceSummaryTable SubSpaceSummaryTable
        {
            get { return _SubSpaceSummaryTable; }
            set
            {
                UpdateProperty(ref _SubSpaceSummaryTable, value);
            }
        }
        private General _General;
        [XmlElement(ElementName = nameof(General))]
        public General General
        {
            get { return _General; }
            set { UpdateProperty(ref _General, value); }
        }

    }

    public class Picture : UpdatePropertyAndOnPropertyChangedBase, INotifyPropertyChanged
    {
        private double _Width;
        [XmlElement(ElementName = "Width")]
        public double Width
        {
            get { return _Width; }
            set
            {
                UpdateProperty(ref _Width, value);
            }
        }

        private double _Height;
        [XmlElement(ElementName = "Height")]
        public double Height
        {
            get { return _Height; }
            set
            {
                UpdateProperty(ref _Height, value);
            }
        }
        private int _MaxCompressSize;
        [XmlElement(ElementName = "MaxCompressSize")]
        public int MaxCompressSize
        {
            get { return _MaxCompressSize; }
            set
            {
                UpdateProperty(ref _MaxCompressSize, value);
            }
        }
        private int _CompressQuality;
        [XmlElement(ElementName = nameof(CompressQuality))]
        public int CompressQuality
        {
            get { return _CompressQuality; }
            set
            {
                UpdateProperty(ref _CompressQuality, value);
            }
        }
        private double _CompressWidth;
        [XmlElement(ElementName = nameof(CompressWidth))]
        public double CompressWidth
        {
            get { return _CompressWidth; }
            set
            { UpdateProperty(ref _CompressWidth, value); }
        }
        private double _CompressHeight;
        [XmlElement(ElementName = nameof(CompressHeight))]
        public double CompressHeight
        {
            get => _CompressHeight;
            set
            {
                UpdateProperty(ref _CompressHeight, value);
            }
        }
    }

    public class Bookmark : UpdatePropertyAndOnPropertyChangedBase, INotifyPropertyChanged
    {
        private int _BridgeDeckBookmarkStartNo;
        [XmlElement(ElementName = "BridgeDeckBookmarkStartNo")]
        public int BridgeDeckBookmarkStartNo
        {
            get => _BridgeDeckBookmarkStartNo;
            set
            {
                UpdateProperty(ref _BridgeDeckBookmarkStartNo, value);
            }
        }
        private int _SuperSpaceBookmarkStartNo;
        [XmlElement(ElementName = "SuperSpaceBookmarkStartNo")]
        public int SuperSpaceBookmarkStartNo
        {
            get => _SuperSpaceBookmarkStartNo;
            set
            {
                UpdateProperty(ref _SuperSpaceBookmarkStartNo, value);
            }
        }
        private int _SubSpaceBookmarkStartNo;
        [XmlElement(ElementName = "SubSpaceBookmarkStartNo")]
        public int SubSpaceBookmarkStartNo
        {
            get => _SubSpaceBookmarkStartNo;
            set
            {
                UpdateProperty(ref _SubSpaceBookmarkStartNo, value);
            }
        }
    }

    public class BridgeDeckSummaryTable : UpdatePropertyAndOnPropertyChangedBase, INotifyPropertyChanged
    {
        private double _No;
        [XmlAttribute(AttributeName = "No")]
        public double No
        {
            get => _No;
            set
            {
                UpdateProperty(ref _No, value);
            }
        }
        private double _Position;
        [XmlAttribute(AttributeName = "Position")]
        public double Position
        {
            get => _Position;
            set
            {
                UpdateProperty(ref _Position, value);
            }
        }

        private double _Component;
        [XmlAttribute(AttributeName = "Component")]
        public double Component
        {
            get => _Component;
            set
            {
                UpdateProperty(ref _Component, value);
            }
        }
        private double _Damage;
        [XmlAttribute(AttributeName = "Damage")]
        public double Damage
        {
            get => _Damage;
            set
            {
                UpdateProperty(ref _Damage, value);
            }
        }
        private double _DamagePosition;
        [XmlAttribute(AttributeName = "DamagePosition")]
        public double DamagePosition
        {
            get => _DamagePosition;
            set
            {
                UpdateProperty(ref _DamagePosition, value);
            }
        }
        private double _DamageDescription;
        [XmlAttribute(AttributeName = "DamageDescription")]
        public double DamageDescription
        {
            get => _DamageDescription;
            set
            { UpdateProperty(ref _DamageDescription, value); }
        }
        private double _PictureNo;
        [XmlAttribute(AttributeName = "PictureNo")]
        public double PictureNo
        {
            get => _PictureNo;
            set
            {
                UpdateProperty(ref _PictureNo, value);
            }
        }
        private double _Comment;
        [XmlAttribute(AttributeName = "Comment")]
        public double Comment
        {
            get => _Comment;
            set
            { UpdateProperty(ref _Comment, value); }
        }
    }

    public class SuperSpaceSummaryTable : BridgeDeckSummaryTable
    {
    }

    public class SubSpaceSummaryTable : BridgeDeckSummaryTable
    {
    }

    public class General : UpdatePropertyAndOnPropertyChangedBase, INotifyPropertyChanged
    {
        private bool _IntactStructNoInsertSummaryTable;
        [XmlElement(ElementName = "IntactStructNoInsertSummaryTable")]
        public bool IntactStructNoInsertSummaryTable
        {
            get => _IntactStructNoInsertSummaryTable;
            set { UpdateProperty(ref _IntactStructNoInsertSummaryTable, value); }
        }
    }
}
