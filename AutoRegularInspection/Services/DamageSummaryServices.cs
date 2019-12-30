using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class DamageSummaryServices
    {

        public void InitListDamageSummary(List<DamageSummary> listDamageSummary)
        {
            SetListDamageSummaryPictureNums(listDamageSummary);
            int firstIndex = 10000;
            for(int i=0;i<listDamageSummary.Count;i++)
            {
                if(i==0)
                {
                    listDamageSummary[i].FirstPictureBookmark = $"_Ref{firstIndex+i}";
                }
                else
                {

                }
            }
        }

        void SetListDamageSummaryPictureNums(List<DamageSummary> listDamageSummary)
        {
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                listDamageSummary[i].PictureCounts = listDamageSummary[i].PictureNo.Split(',').Count();
            }
        }
    }
}
