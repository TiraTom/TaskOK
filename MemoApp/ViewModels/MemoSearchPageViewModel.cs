using MemoApp.Helpers;
using MemoApp.Models;
using MempApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace MemoApp.ViewModels
{
	public class MemoSearchPageViewModel : INotifyPropertyChanged
	{
		public Views.MemoSearchPage View { get; private set; } = null;
		public void Initialize(Views.MemoSearchPage memoSearchPage)
		{
			View = memoSearchPage;
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public string SearchWord { get; set; } = string.Empty;
		public string SearchBoxPlaceholder { get; } = GetLangMessage.GetMessage("Msg4001");

		public string RadioButtonTaskNameSearchLabel { get; } = GetLangMessage.GetMessage("Msg4002");
		public string RadioButtonMemoContentSearchLabel { get; } = GetLangMessage.GetMessage("Msg4003");

		private bool _isSearchConditionTaskName = CommonViewModel.IsRecentSearchConditionTaskName;
		private static readonly PropertyChangedEventArgs IsSearchConditionTaskNamePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsSearchConditionTaskName));
		private bool _isSearchConditionMemoContent = CommonViewModel.IsRecentSearchConditionMemoContent;
		private static readonly PropertyChangedEventArgs IsSearchConditionMemoContentPropertyChangdEventArgs = new PropertyChangedEventArgs(nameof(IsSearchConditionMemoContent));

		public string HtmlContent { get; set; } = string.Empty;

		public ObservableCollection<EachSearchResult> SearchResult { get; set; } = CommonViewModel.RecentSearchResult;


		public bool IsSearchConditionTaskName
		{
			get
			{
				return this._isSearchConditionTaskName;
			}
			set
			{
				if (this._isSearchConditionTaskName == value) { return; }

				this._isSearchConditionTaskName = value;
				this.PropertyChanged?.Invoke(this, IsSearchConditionTaskNamePropertyChangedEventArgs);
			}
		}

		public bool IsSearchConditionMemoContent
		{
			get
			{
				return this._isSearchConditionMemoContent;
			}
			set
			{
				if (this._isSearchConditionMemoContent == value) { return; }

				this._isSearchConditionMemoContent = value;
				this.PropertyChanged?.Invoke(this, IsSearchConditionMemoContentPropertyChangdEventArgs);
			}
		}

		public void Search(object sender, RoutedEventArgs e)
		{

			try
			{
				CommonViewModel.RecentSearchWord = this.SearchWord;

				List<Memo> result = new List<Memo>();

				if (this.IsSearchConditionTaskName)
				{
					result = MemoModel.SearchTaskName(SearchWord);
					CommonViewModel.IsRecentSearchConditionTaskName = true;
					CommonViewModel.IsRecentSearchConditionMemoContent = false;
				}
				else
				{
					result = MemoModel.SearchMemoContent(SearchWord);
					CommonViewModel.IsRecentSearchConditionTaskName = false;
					CommonViewModel.IsRecentSearchConditionMemoContent = true;
				}

				SearchResult.Clear();

				result.ForEach(memo =>
				{
					string targetContent = string.Empty;
					string contentHtml = string.Empty;
					int index = 0;

					if (this.IsSearchConditionTaskName)
					{
						targetContent = memo.EachTask.Content.Replace("\r", "<br>");
					}
					else
					{
						targetContent = memo.Content.Replace("\r", "<br>");
					}

					while (true)
					{
						index = targetContent.IndexOf(this.SearchWord, StringComparison.OrdinalIgnoreCase);
						if (index < 0)
						{
							contentHtml += targetContent;
							break;
						}
						else if (index > 0)
						{
							contentHtml = targetContent.Substring(0, index);
						}

						contentHtml += $"<strong>{targetContent.Substring(index, this.SearchWord.Length)}</strong>";
						targetContent = targetContent.Substring(index + this.SearchWord.Length);
					}
					contentHtml = $"<span>{contentHtml}</span>";
					EachSearchResult eachSearchResult = new EachSearchResult();

					if (this.IsSearchConditionTaskName)
					{
						eachSearchResult.TaskName = contentHtml;
						eachSearchResult.Date = $"[{memo.EachTask.PlanDate.Date.ToString().Substring(0, 10)}({memo.EachTask.PlanDate.DayOfWeek.ToString().Substring(0, 3)})]";
						eachSearchResult.ContentHtml = memo.Content;
					}
					else
					{
						eachSearchResult.TaskName = memo.EachTask.Content;
						eachSearchResult.Date = $"[{memo.EachTask.PlanDate.Date.ToString().Substring(0, 10)}({memo.EachTask.PlanDate.DayOfWeek.ToString().Substring(0, 3)})]";
						eachSearchResult.ContentHtml = contentHtml;
					}
					SearchResult.Add(eachSearchResult);
				});

				CommonViewModel.RecentSearchResult = new ObservableCollection<EachSearchResult>(SearchResult);


			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}
	}

	public class EachSearchResult
	{
		public string TaskName { get; set; }
		public string Date { get; set; }
		public string Content { get; set; }
		public string ContentHtml { get; set; }
	}
}
