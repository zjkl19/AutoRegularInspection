using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    public class GlobalData
    {
        public static BindingList<BridgeDeck> ComponentComboBox { get; } = new BindingList<BridgeDeck>() {
                    new BridgeDeck{ Idx=0, Id=1,Title="桥面铺装"
                        ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=1,Title="网裂"}
                            ,new BridgeDeck{ Idx=1, Id=2,Title="龟裂"}
                            ,new BridgeDeck{ Idx=2, Id=3,Title="波浪"}
                            ,new BridgeDeck{ Idx=3, Id=4,Title="车辙"}
                            ,new BridgeDeck{ Idx=4, Id=5,Title="坑槽"}
                            ,new BridgeDeck{ Idx=5, Id=6,Title="碎裂"}
                            ,new BridgeDeck{ Idx=6, Id=7,Title="破碎"}
                            ,new BridgeDeck{ Idx=7, Id=8,Title="坑洞"}
                            ,new BridgeDeck{ Idx=8, Id=9,Title="桥面贯通横缝"}
                            ,new BridgeDeck{ Idx=9, Id=10,Title="桥面贯通纵缝"}
                            ,new BridgeDeck{ Idx=10, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDeck{ Idx=1, Id=2,Title="桥头平顺"
                            ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=1,Title="桥头沉降"}
                            ,new BridgeDeck{ Idx=1, Id=2,Title="台背下沉"}
                            ,new BridgeDeck{ Idx=2, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDeck{  Idx=2,Id=3,Title="伸缩缝"
                            ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=1,Title="螺帽松动"}
                            ,new BridgeDeck{ Idx=1, Id=2,Title="缝内沉积物阻塞"}
                            ,new BridgeDeck{ Idx=2, Id=3,Title="止水带破损"}
                            ,new BridgeDeck{ Idx=3, Id=4,Title="止水带老化"}
                            ,new BridgeDeck{ Idx=4, Id=5,Title="钢材料破损"}
                            ,new BridgeDeck{ Idx=5, Id=6,Title="接缝处铺装碎边"}
                            ,new BridgeDeck{ Idx=6, Id=7,Title="接缝处高差"}
                            ,new BridgeDeck{ Idx=7, Id=8,Title="钢材料翘曲变形"}
                            ,new BridgeDeck{ Idx=8, Id=9,Title="结构缝宽异常"}
                            ,new BridgeDeck{ Idx=9, Id=10,Title="伸缩缝处异常声响"}
                            ,new BridgeDeck{ Idx=10, Id=99,Title="其它"}
                        }}
                    ,new BridgeDeck{  Idx=3,Id=4,Title="排水系统"
                    ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=1,Title="泄水管阻塞"}
                            ,new BridgeDeck{ Idx=1, Id=2,Title="残缺脱落"}
                            ,new BridgeDeck{ Idx=2, Id=3,Title="桥面积水"}
                            ,new BridgeDeck{ Idx=3, Id=4,Title="防水层"}
                            ,new BridgeDeck{ Idx=4, Id=99,Title="其它"}
                        }}
                    ,new BridgeDeck{  Idx=4,Id=5,Title="栏杆"
                    ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=1,Title="露筋锈蚀"}
                            ,new BridgeDeck{ Idx=1, Id=2,Title="松动错位"}
                            ,new BridgeDeck{ Idx=2, Id=3,Title="丢失残缺"}
                            ,new BridgeDeck{ Idx=3, Id=99,Title="其它"}
                        }}
                    ,new BridgeDeck{  Idx=5,Id=6,Title="护栏"
                    ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=1,Title="露筋锈蚀"}
                            ,new BridgeDeck{ Idx=1, Id=2,Title="松动错位"}
                            ,new BridgeDeck{ Idx=2, Id=3,Title="丢失残缺"}
                            ,new BridgeDeck{ Idx=3, Id=99,Title="其它"}
                        }}
                    ,new BridgeDeck{  Idx=6,Id=7,Title="人行道块件"
                    ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=1,Title="网裂"}
                            ,new BridgeDeck{ Idx=1, Id=2,Title="松动或变形"}
                            ,new BridgeDeck{ Idx=2, Id=3,Title="残缺"}
                            ,new BridgeDeck{ Idx=3, Id=99,Title="其它"}
                        }}
                    ,new BridgeDeck{  Idx=7,Id=99,Title="其它"
                    ,SubComponentComboBox=new BindingList<BridgeDeck>() {
                            new BridgeDeck{ Idx=0, Id=99,Title="其它"}
                        }}

        };

        //public static BindingList<BridgeDeck> ComponentComboBox { get; } = new BindingList<BridgeDeck>() {
        //            new BridgeDeck{  Id=1,Title="桥面铺装"}
        //            ,aa()
        //            ,new BridgeDeck{  Id=2,Title="其它"}

        //};

        /// <summary>
        /// 从文本文件中读取数据
        /// </summary>
        /// <returns></returns>
        private static BridgeDeck aa()
        {
            var strmopen = new System.IO.StreamReader("TestTxt.txt");
            string strOpen = strmopen.ReadToEnd();

            string r = string.Empty;
            for (int i = 0; i < strOpen.Length; i++)
            {
                r += strOpen[i];
            }
            strmopen.Close();
            return new BridgeDeck { Id = 3, Title = r };
        }

        public static BindingList<BridgeDeck> TestComboBox1 { get; } = new BindingList<BridgeDeck>() {
                    new BridgeDeck{  Id=1,Title="阻塞"}
                    ,new BridgeDeck{  Id=2,Title="碎边"}

        };

        public static BindingList<BridgeDeck> TestComboBox2 { get; } = new BindingList<BridgeDeck>() {
                    new BridgeDeck{  Id=1,Title="残缺"}
                    ,new BridgeDeck{  Id=2,Title="锈蚀"}

        };
    }
}
