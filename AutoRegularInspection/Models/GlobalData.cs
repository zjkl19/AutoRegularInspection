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
        public static BindingList<BridgeDeck> TestComboBox { get; } = new BindingList<BridgeDeck>() {
                    new BridgeDeck{  Id=1,Title="桥面铺装"}
                    ,aa()
                    ,new BridgeDeck{  Id=2,Title="其它"}
                    
        };

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
    }
}
