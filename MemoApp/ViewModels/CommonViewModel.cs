using MemoApp.Helpers;
using MemoApp.Models;
using MemoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static MemoApp.Models.ConfigModel;
using static MemoApp.ViewModels.MemoSearchPageViewModel;

namespace MemoApp.ViewModels
{
	public class CommonViewModel
	{
		public static string ReviewTaskId = "REVIEW_AND_DIARY_ID";

		public string MainPageButtonLabel { get; } = GetLangMessage.GetMessage("MainPageButtonLabel");
		public string RegisterPageButtonLabel { get; } = GetLangMessage.GetMessage("RegisterPageButtonLabel");
		public string LogPageButtonLabel { get; } = GetLangMessage.GetMessage("LogPageButtonLabel");
		public string ConfigPageButtonLabel { get; } = GetLangMessage.GetMessage("ConfigPageButtonLabel");
		public string MemoSearchPageButtonLabel { get; } = GetLangMessage.GetMessage("MemoSearchPageButtonLabel");
		public string HowToPageButtonLabel { get; } = GetLangMessage.GetMessage("HowToPageButtonLabel");



		public static DateTimeOffset RecentInfoSetDate { get; set; }

		private static string _recentSelectedEachTaskId = string.Empty;
		public static string RecentSelectedEachTaskId
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_recentSelectedEachTaskId))
				{
					_recentSelectedEachTaskId = ConfigModel.GetSpecificConfigValue(ConfigType.RecentEachTaskId);
					if (!EachTaskModel.IsSpecificDateTask(_recentSelectedEachTaskId, DateTimeOffset.Now.LocalDateTime))
					{
						_recentSelectedEachTaskId = string.Empty;
					}
					else
					{
						RecentInfoSetDate = DateTimeOffset.Now.LocalDateTime;
					}
				}
				else
				{
					if(RecentInfoSetDate.Date != DateTimeOffset.Now.LocalDateTime.Date)
					{
						_recentSelectedEachTaskId = string.Empty;
					}
				}

				return _recentSelectedEachTaskId;
			}

			set
			{
				if (_recentSelectedEachTaskId == value)
				{
					return;
				}
				else
				{
					_recentSelectedEachTaskId = value;
					UpdateSpecificConfigAsyncVoid(ConfigType.RecentEachTaskId, value);
					RecentInfoSetDate = DateTimeOffset.Now.LocalDateTime;
				}
			}
		}

		private static string _recentActionStatus = string.Empty;
		public static string RecentActionStatus
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_recentActionStatus))
				{
					_recentActionStatus = ConfigModel.GetSpecificConfigValue(ConfigType.RecentActivity);
				}

				return _recentActionStatus;
			}

			set
			{
				if (_recentActionStatus == value)
				{
					return;
				}
				else
				{
					_recentActionStatus = value;
					UpdateSpecificConfigAsyncVoid(ConfigType.RecentActivity, value);
				}
			}
		}

		public static string RecentSearchWord { get; set; } = string.Empty;
		public static bool IsRecentSearchConditionTaskName { get; set; } = false;
		public static bool IsRecentSearchConditionMemoContent { get; set; } = true;
		public static ObservableCollection<EachSearchResult> RecentSearchResult { get; set; } = new ObservableCollection<EachSearchResult>();

		public static string LangSetting { get; set; } = string.IsNullOrEmpty(GetSpecificConfigValue(ConfigType.Language)) ? LanguageConfig.EN.ToString() : GetSpecificConfigValue(ConfigType.Language);


		public static async void NotifySystemMessage(string msg)
		{
			ContentDialog timeInfoConditionMsg = new ContentDialog
			{
				Title = GetLangMessage.GetMessage("Msg1013"),
				Content = msg,
				CloseButtonText = "OK"
			};

			ContentDialogResult result = await timeInfoConditionMsg.ShowAsync();
		}


	}
}
