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
        public static BindingList<BridgeDamage> ComponentComboBox { get; } = new BindingList<BridgeDamage>() {
                    new BridgeDamage{ Idx=0, Id=1,Title="桥面铺装"
                        ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="网裂"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="龟裂"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="波浪"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="车辙"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="坑槽"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="碎裂"}
                            ,new BridgeDamage{ Idx=6, Id=7,Title="破碎"}
                            ,new BridgeDamage{ Idx=7, Id=8,Title="坑洞"}
                            ,new BridgeDamage{ Idx=8, Id=9,Title="桥面贯通横缝"}
                            ,new BridgeDamage{ Idx=9, Id=10,Title="桥面贯通纵缝"}
                            ,new BridgeDamage{ Idx=10, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{ Idx=1, Id=2,Title="桥头平顺"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="桥头沉降"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="台背下沉"}
                            ,new BridgeDamage{ Idx=2, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{  Idx=2,Id=3,Title="伸缩缝"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="螺帽松动"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="缝内沉积物阻塞"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="止水带破损"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="止水带老化"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="钢材料破损"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="接缝处铺装碎边"}
                            ,new BridgeDamage{ Idx=6, Id=7,Title="接缝处高差"}
                            ,new BridgeDamage{ Idx=7, Id=8,Title="钢材料翘曲变形"}
                            ,new BridgeDamage{ Idx=8, Id=9,Title="结构缝宽异常"}
                            ,new BridgeDamage{ Idx=9, Id=10,Title="伸缩缝处异常声响"}
                            ,new BridgeDamage{ Idx=10, Id=99,Title="其它"}
                        }}
                    ,new BridgeDamage{  Idx=3,Id=4,Title="排水系统"
                    ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="泄水管阻塞"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="残缺脱落"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="桥面积水"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="防水层"}
                            ,new BridgeDamage{ Idx=4, Id=99,Title="其它"}
                        }}
                    ,new BridgeDamage{  Idx=4,Id=5,Title="栏杆"
                    ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="露筋锈蚀"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="松动错位"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="丢失残缺"}
                            ,new BridgeDamage{ Idx=3, Id=99,Title="其它"}
                        }}
                    ,new BridgeDamage{  Idx=5,Id=6,Title="护栏"
                    ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="露筋锈蚀"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="松动错位"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="丢失残缺"}
                            ,new BridgeDamage{ Idx=3, Id=99,Title="其它"}
                        }}
                    ,new BridgeDamage{  Idx=6,Id=7,Title="人行道块件"
                    ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="网裂"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="松动或变形"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="残缺"}
                            ,new BridgeDamage{ Idx=3, Id=99,Title="其它"}
                        }}
                    ,new BridgeDamage{  Idx=7,Id=99,Title="其它"
                    ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=99,Title="其它"}
                        }}

        };

        public static BindingList<BridgeDamage> SuperSpaceComponentComboBox { get; } = new BindingList<BridgeDamage>() {
                    new BridgeDamage{ Idx=0, Id=1,Title="主梁"
                        ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="表面裂缝"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="混凝土剥离"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="露筋锈蚀"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="梁体下挠"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="结构裂缝"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="裂缝处渗水"}
                            ,new BridgeDamage{ Idx=6, Id=7,Title="桥面贯通横缝"}
                            ,new BridgeDamage{ Idx=7, Id=8,Title="梁体位移"}
                            ,new BridgeDamage{ Idx=8, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{ Idx=1, Id=2,Title="横向联系"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="桥面贯通纵缝"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="连接件脱焊松动"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="连接件断裂"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="横隔板网裂"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="横隔板剥落露筋"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="梁体异常振动"}
                            ,new BridgeDamage{ Idx=6, Id=99,Title="其它"}
                        }
                    }

                    ,new BridgeDamage{  Idx=2,Id=99,Title="其它"
                    ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=99,Title="其它"}
                        }}

        };

        public static BindingList<BridgeDamage> SubSpaceComponentComboBox { get; } = new BindingList<BridgeDamage>() {
                    new BridgeDamage{ Idx=0, Id=1,Title="台帽"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="表面裂缝"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="混凝土剥离"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="露筋锈蚀"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="结构裂缝"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="裂缝处渗水"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="墩台成块剥落"}
                            ,new BridgeDamage{ Idx=6, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{ Idx=1, Id=2,Title="盖梁"
                           ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="表面裂缝"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="混凝土剥离"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="露筋锈蚀"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="结构裂缝"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="裂缝处渗水"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="墩台成块剥落"}
                            ,new BridgeDamage{ Idx=6, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{ Idx=2, Id=3,Title="墩身"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="墩身水平裂缝"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="墩身纵向裂缝"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="框架式节点裂缝"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="露筋锈蚀"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="混凝土剥离"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="桥墩倾斜"}
                            ,new BridgeDamage{ Idx=6, Id=7,Title="桥面贯通横缝"}
                            ,new BridgeDamage{ Idx=7, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{ Idx=3, Id=4,Title="台身"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="墩身水平裂缝"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="墩身纵向裂缝"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="框架式节点裂缝"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="露筋锈蚀"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="混凝土剥离"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="桥墩倾斜"}
                            ,new BridgeDamage{ Idx=6, Id=7,Title="桥面贯通横缝"}
                            ,new BridgeDamage{ Idx=7, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{ Idx=4, Id=5,Title="支座"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="固定螺栓损坏"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="橡胶支座变形"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="钢支座损坏"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="支座底板混凝土破损"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="支座稳定性异常"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="钢垫板锈蚀"}
                            ,new BridgeDamage{ Idx=6, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{ Idx=5, Id=6,Title="耳背翼墙"
                            ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=1,Title="桥面贯通纵缝"}
                            ,new BridgeDamage{ Idx=1, Id=2,Title="连接件脱焊松动"}
                            ,new BridgeDamage{ Idx=2, Id=3,Title="连接件断裂"}
                            ,new BridgeDamage{ Idx=3, Id=4,Title="横隔板网裂"}
                            ,new BridgeDamage{ Idx=4, Id=5,Title="横隔板剥落露筋"}
                            ,new BridgeDamage{ Idx=5, Id=6,Title="梁体异常振动"}
                            ,new BridgeDamage{ Idx=6, Id=99,Title="其它"}
                        }
                    }
                    ,new BridgeDamage{  Idx=6,Id=99,Title="其它"
                    ,DamageComboBox=new BindingList<BridgeDamage>() {
                            new BridgeDamage{ Idx=0, Id=99,Title="其它"}
                        }}

        };

        //public static BindingList<BridgeDamage> ComponentComboBox { get; } = new BindingList<BridgeDamage>() {
        //            new BridgeDamage{  Id=1,Title="桥面铺装"}
        //            ,aa()
        //            ,new BridgeDamage{  Id=2,Title="其它"}

        //};

        /// <summary>
        /// 从文本文件中读取数据
        /// </summary>
        /// <returns></returns>
        private static BridgeDamage aa()
        {
            var strmopen = new System.IO.StreamReader("TestTxt.txt");
            string strOpen = strmopen.ReadToEnd();

            string r = string.Empty;
            for (int i = 0; i < strOpen.Length; i++)
            {
                r += strOpen[i];
            }
            strmopen.Close();
            return new BridgeDamage { Id = 3, Title = r };
        }

        public static BindingList<BridgeDamage> TestComboBox1 { get; } = new BindingList<BridgeDamage>() {
                    new BridgeDamage{  Id=1,Title="阻塞"}
                    ,new BridgeDamage{  Id=2,Title="碎边"}

        };

        public static BindingList<BridgeDamage> TestComboBox2 { get; } = new BindingList<BridgeDamage>() {
                    new BridgeDamage{  Id=1,Title="残缺"}
                    ,new BridgeDamage{  Id=2,Title="锈蚀"}

        };
    }
}
