using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
	public partial class AsposeWordsServices
	{
		public void GenerateDefaultTemplateReport(ProgressBarModel progressModel)
		{
			//progressModel.ProgressValue = 0;
			//progressModel.Content = $"正在处理{Properties.Resources.BridgeDeck}……";
			progressModel.ReportProgress($"正在处理{Properties.Resources.BridgeDeck}……", 0);
			InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, _bridgeDeckListDamageSummary);
			System.Threading.Thread.Sleep(1000);

			//progressModel.Content = $"正在处理{Properties.Resources.SuperSpace}……";
			//progressModel.ProgressValue = 33;
			progressModel.ReportProgress($"正在处理{Properties.Resources.SuperSpace}……", 33);
			InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, _superSpaceListDamageSummary);
			System.Threading.Thread.Sleep(1000);


			//progressModel.Content = $"正在处理{Properties.Resources.SubSpace}……";
			//progressModel.ProgressValue = 66;
			progressModel.ReportProgress($"正在处理{Properties.Resources.SubSpace}……", 66);

			InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, _subSpaceListDamageSummary);
			System.Threading.Thread.Sleep(1000);

			//progressModel.Content = "正在生成统计汇总表……";
			//progressModel.ProgressValue = 90;
			progressModel.ReportProgress("正在生成统计汇总表…", 90);
			CreateStatisticsTable();
			System.Threading.Thread.Sleep(1000);


			//progressModel.Content = "正在替换文档变量……";
			//progressModel.ProgressValue = 99;
			progressModel.ReportProgress("正在替换文档变量…", 99);
			ReplaceDocVariable();
			//其它不怎么耗时的操作
			InsertSummaryWords();

			//两次更新域，1次更新序号，1次更新序号对应的交叉引用
			_doc.UpdateFields();
			_doc.UpdateFields();

			progressModel.ProgressValue = 100;
			progressModel.Content = "正在完成……";
		}
	}
}
