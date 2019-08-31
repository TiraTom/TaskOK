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
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MemoApp.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public Views.MainPage View { get; private set; } = null;
		public event PropertyChangedEventHandler PropertyChanged;


		public void Initialize(Views.MainPage mainPage)
		{
			View = mainPage;
		}

		public string TaskChoicePlaceholder { get; } = GetLangMessage.GetMessage("Msg1001");
		public string StartLabel { get; } = GetLangMessage.GetMessage("Msg1002");
		public string PauseLabel { get; } = GetLangMessage.GetMessage("Msg1003");
		public string FinishLabel { get; } = GetLangMessage.GetMessage("Msg1004");
		public string MemoLabel { get; } = GetLangMessage.GetMessage("Msg1005");
		public string MemoPlaceholder { get; } = GetLangMessage.GetMessage("Msg1014");
		public string RegisterButtonLabel { get; } = GetLangMessage.GetMessage("Msg1006");
		public ObservableCollection<TaskDisplayInfo> TaskListData { get; set; } = new ObservableCollection<TaskDisplayInfo>();
		public List<Memo> MemoListData { get; set; } = new List<Memo>();
		public TaskDisplayInfo TaskDisplayInfo { get; set; }
		public ObservableCollection<SmallTaskInfo> SmallTaskListData { get; set; } = new ObservableCollection<SmallTaskInfo>();
		private ThreadPoolTimer NotifyRestTime;
		private static readonly PropertyChangedEventArgs TaskChoiceLabelPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(TaskChoiceLabel));
		private string _taskChoiceLabel = $"{GetLangMessage.GetMessage("Msg1007")}  {ShowRecentInfo()}";
		private static readonly PropertyChangedEventArgs SelectedEachTaskIdPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedEachTaskId));
		private string _selectedEachTaskId = CommonViewModel.RecentSelectedEachTaskId;
		private static readonly PropertyChangedEventArgs SelectedEachTaskItemPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedEachTaskItem));
		private TaskDisplayInfo _selectedEachTaskItem;
		private static readonly PropertyChangedEventArgs MemoContentPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(MemoContent));
		private string _memoContent = MemoModel.GetSpecificEachTaskMemo(CommonViewModel.RecentSelectedEachTaskId) ?? string.Empty;
		private static readonly PropertyChangedEventArgs RecentActionInfoPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(RecentActionInfo));
		private string _recentActionInfo = CommonViewModel.RecentActionStatus;
		private DateTimeOffset _mainDate = DateTimeOffset.Now.ToLocalTime();
		private static readonly PropertyChangedEventArgs MainDatePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(MainDate));
		private bool _isButtonEnabled = true;
		private static readonly PropertyChangedEventArgs IsButtonEnabledProeprtyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsButtonEnabled));

		public DateTimeOffset MainDate
		{
			get { return this._mainDate; }
			set
			{
				if (this._mainDate == value) { return; }
				this._mainDate = value;
				this.PropertyChanged?.Invoke(this, MainDatePropertyChangedEventArgs);
				SetTaskDisplayInfo();
				SetSmallTaskInfo();

				if (MainDate.Date == DateTimeOffset.Now.LocalDateTime.Date)
				{
					IsButtonEnabled = true;
				}
				else
				{
					IsButtonEnabled = false;
				}
			}
		}

		public bool IsButtonEnabled
		{
			get
			{
				return _isButtonEnabled;
			}
			set
			{
				if (this._isButtonEnabled == value) { return; }
				this._isButtonEnabled = value;
				this.PropertyChanged?.Invoke(this, IsButtonEnabledProeprtyChangedEventArgs);
			}
		}

		public string TaskChoiceLabel
		{
			get { return this._taskChoiceLabel; }
			set
			{
				if (this._taskChoiceLabel == value) { return; }
				this._taskChoiceLabel = value;
				this.PropertyChanged?.Invoke(this, TaskChoiceLabelPropertyChangedEventArgs);
			}
		}


		public string SelectedEachTaskId
		{
			get { return this._selectedEachTaskId; }
			set
			{
				if (this._selectedEachTaskId == value) { return; }
				this._selectedEachTaskId = value;
				this.PropertyChanged?.Invoke(this, SelectedEachTaskIdPropertyChangedEventArgs);
				MemoContent = MemoModel.GetSpecificEachTaskMemo(this._selectedEachTaskId) ?? "";
				SetSmallTaskInfo();
				SetSelectedEachTask();
			}
		}

		public TaskDisplayInfo SelectedEachTaskItem
		{
			get { return this._selectedEachTaskItem; }
			set
			{
				if (this._selectedEachTaskItem == value) { return; }
				this._selectedEachTaskItem = value;
				this.PropertyChanged?.Invoke(this, SelectedEachTaskIdPropertyChangedEventArgs);
			}
		}

		public string MemoContent
		{
			get { return this._memoContent; }
			set
			{
				if (this._memoContent == value) { return; }

				this._memoContent = value;
				this.PropertyChanged?.Invoke(this, MemoContentPropertyChangedEventArgs);
			}
		}


		public string RecentActionInfo
		{
			get { return this._recentActionInfo; }
			set
			{
				if (this._recentActionInfo == value) { return; }

				this._recentActionInfo = value;
				this.PropertyChanged?.Invoke(this, RecentActionInfoPropertyChangedEventArgs);
			}
		}


		// Mas is used in each method
		public string Msg { get; set; }

		public void Page_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				SetTaskDisplayInfo();
				SetSmallTaskInfo();
				SetSelectedEachTask();
				this.SelectedEachTaskId = CommonViewModel.RecentSelectedEachTaskId;
			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}


		public void SetRecentInfo(string msgNo)
		{
			try
			{
				CommonViewModel.RecentSelectedEachTaskId = SelectedEachTaskId;
				CommonViewModel.RecentActionStatus = msgNo;

				TaskChoiceLabel = $"{GetLangMessage.GetMessage("Msg1007")}       {ShowRecentInfo()}";

			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}

		public static string ShowRecentInfo()
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(CommonViewModel.RecentActionStatus)
						&& EachTaskModel.IsSpecificDateTask(CommonViewModel.RecentSelectedEachTaskId, DateTimeOffset.Now.LocalDateTime.Date))
				{
					EachTask eachTask = EachTaskModel.GetEachTask(CommonViewModel.RecentSelectedEachTaskId);

					return $"{GetLangMessage.GetMessage("Msg1008")}：{eachTask.Content} {GetLangMessage.GetMessage(CommonViewModel.RecentActionStatus)}";
				}
				else
				{
					return $"{GetLangMessage.GetMessage("Msg1008")}：{GetLangMessage.GetMessage("Msg1009")}";
				}

			}
			catch
			{
				return GetLangMessage.GetMessage("Exception");
			}

		}

		public async Task MemoRegister(object sender, RoutedEventArgs e)
		{

			try
			{
				// Control Progress Dialog
				var progressDialog = new RegisterProgressDialog();
				var showingDialog = progressDialog.ShowAsync();

				if (IsEachTaskIdEmpty())
				{
					CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Msg1001"));
					return;
				}

				if (!string.IsNullOrWhiteSpace(MemoContent))
				{
					MemoModel.Register(SelectedEachTaskId, MemoContent);
				}

				showingDialog.Cancel();
				progressDialog.Hide();

				// Control Progress Dialog
				var completeDialog = new RegisterCompleteDialog();
				var noticeCompleteDialog = completeDialog.ShowAsync();
				await Task.Delay(700);
				noticeCompleteDialog.Cancel();
				completeDialog.Hide();

			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}

		public void TaskStart(object sender, RoutedEventArgs e)
		{

			try
			{
				if (IsEachTaskIdEmpty())
				{
					CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Msg1001"));
					return;
				}


				Msg = TimeInfoModel.RegisterStart(SelectedEachTaskId);
				if (Msg != null)
				{
					CommonViewModel.NotifySystemMessage(Msg);

					if (Msg != GetLangMessage.GetMessage("Msg1016"))
					{
						// If the button pushing is invalid, return and not update the status.
						return;
					}
				}

				if (ConfigModel.GetSpecificConfigValue(ConfigModel.ConfigType.NotificationFlag) == true.ToString())
				{
					this.NotifyRestTime = ThreadPoolTimer.CreatePeriodicTimer((source) =>
					{
						Notification notification = new Notification();
						notification.NotifyRestTime();
					}, TimeSpan.FromMinutes(float.Parse(ConfigModel.GetSpecificConfigValue(ConfigModel.ConfigType.NotificationSpanMinute))));
				}

				// Update Recent status
				SetRecentInfo("Msg1010");


			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}

		public void TaskPause(object sender, RoutedEventArgs e)
		{
			try
			{
				if (IsEachTaskIdEmpty())
				{
					CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Msg1001"));
					return;
				}

				Msg = TimeInfoModel.RegisterPause(SelectedEachTaskId);
				if (Msg != null)
				{
					CommonViewModel.NotifySystemMessage(Msg);
					return;

				}

				if (NotifyRestTime != null)
				{
					NotifyRestTime.Cancel();
				}

				CommonViewModel.RecentSelectedEachTaskId = SelectedEachTaskId;

				// Update Recent status
				SetRecentInfo("Msg1011");

			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}

		}

		public void TaskStop(object sender, RoutedEventArgs e)
		{

			try
			{
				if (IsEachTaskIdEmpty())
				{
					CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Msg1001"));
					return;
				}

				Msg = TimeInfoModel.RegisterStop(SelectedEachTaskId);
				if (Msg != null)
				{
					CommonViewModel.NotifySystemMessage(Msg);

					if (Msg != GetLangMessage.GetMessage("Msg1020"))
					{
						// If the button pushing is invalid, return and not update the status.
						return;
					}
				}

				if (NotifyRestTime != null)
				{
					NotifyRestTime.Cancel();
				}

				CommonViewModel.RecentSelectedEachTaskId = SelectedEachTaskId;

				// Update Recent status
				SetRecentInfo("Msg1012");


			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}



		public bool IsEachTaskIdEmpty() => string.IsNullOrWhiteSpace(SelectedEachTaskId) ? true : false;


		public static List<TaskDisplayInfo> GetTaskDisplayInfo(DateTimeOffset mainDate)
		{

			try
			{
				List<EachTask> specificDateEachTaskList = EachTaskModel.GetSpecificDateEachTasks(mainDate).ToList();

				List<TaskDisplayInfo> result = new List<TaskDisplayInfo>();
				specificDateEachTaskList?.ForEach(eachTask =>
				{
					TaskDisplayInfo info = new TaskDisplayInfo
					{
						EachTaskId = eachTask.EachTaskId,
						Content = (eachTask.CompleteFlag ? "✔" : "").PadRight(3) + eachTask.Content
					};
					result.Add(info);
				});

				return result;

			}
			catch
			{
				return new List<TaskDisplayInfo>();
			}


		}

		public void SetTaskDisplayInfo()
		{
			try
			{

				List<EachTask> specificDateEachTaskList = EachTaskModel.GetSpecificDateEachTasks(this.MainDate).ToList();

				TaskListData.Clear();

				specificDateEachTaskList?.ForEach(eachTask =>
				{
					TaskDisplayInfo info = new TaskDisplayInfo
					{
						EachTaskId = eachTask.EachTaskId,
						Content = (eachTask.CompleteFlag ? "✔" : "").PadRight(3) + eachTask.Content
					};
					TaskListData.Add(info);
				});
			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}

		public void SetSelectedEachTask()
		{
			try
			{
				EachTask selectedTask = EachTaskModel.GetEachTask(CommonViewModel.RecentSelectedEachTaskId);

				if (selectedTask != null)
				{
					this.SelectedEachTaskItem = new TaskDisplayInfo()
					{
						EachTaskId = selectedTask.EachTaskId,
						Content = (selectedTask.CompleteFlag ? "✔" : "").PadRight(3) + selectedTask.Content
					};
				}

			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}

		public void SetSmallTaskInfo()
		{

			try
			{
				List<EachTask> smallEachTaskList = EachTaskModel.GetSpecificTaskSmallTasks(SelectedEachTaskId);

				SmallTaskListData.Clear();

				smallEachTaskList?.ForEach(eachSmallTask =>
				{
					SmallTaskInfo smallTaskInfo = new SmallTaskInfo()
					{
						EachTaskId = eachSmallTask.EachTaskId,
						ParentEachTaskId = SelectedEachTaskId,
						Content = eachSmallTask.Content,
						IsComplete = eachSmallTask.CompleteFlag
					};

					SmallTaskListData.Add(smallTaskInfo);
				});


			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}
	}

	public class TaskDisplayInfo
	{
		public string EachTaskId { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
	}

	public class SmallTaskInfo
	{
		public string EachTaskId { get; set; } = string.Empty;
		public string ParentEachTaskId { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public bool IsComplete { get; set; } = false;

		public async void ChangeSmallTaskCompleteFlag()
		{
			await TimeInfoModel.ChangeComplateFlag(EachTaskId, !IsComplete);
		}
	}
}
