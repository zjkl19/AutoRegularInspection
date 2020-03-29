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
                    new BridgeDeck{  Id=1,Title="桥面铺装"}
                    ,new BridgeDeck{  Id=2,Title="桥头平顺"}
                    ,new BridgeDeck{  Id=3,Title="伸缩缝"}
                    ,new BridgeDeck{  Id=4,Title="排水系统"}
                    ,new BridgeDeck{  Id=5,Title="栏杆"}
                    ,new BridgeDeck{  Id=6,Title="护栏"}
                    ,new BridgeDeck{  Id=7,Title="人行道块件"}
                    ,new BridgeDeck{  Id=8,Title="其它"}

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
                    ,aa()
                    ,new BridgeDeck{  Id=2,Title="碎边"}

        };

        public static BindingList<BridgeDeck> TestComboBox2 { get; } = new BindingList<BridgeDeck>() {
                    new BridgeDeck{  Id=1,Title="残缺"}
                    ,aa()
                    ,new BridgeDeck{  Id=2,Title="锈蚀"}

        };
    }
}
