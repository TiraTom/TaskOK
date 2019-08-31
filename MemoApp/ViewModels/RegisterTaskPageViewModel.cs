using MemoApp.Helpers;
using MemoApp.Models;
using MempApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static MemoApp.Models.EachTaskModel;

namespace MemoApp.ViewModels
{
	public class RegisterTaskPageViewModel : UserControl, INotifyPropertyChanged
	{
		public Views.RegisterTaskPage View { get; private set; } = null;
		public event PropertyChangedEventHandler PropertyChanged;

		public string RegisterButtonLabel { get; } = GetLangMessage.GetMessage("Msg2002");
		public string TaskDataPlaceHolder { get; } = GetLangMessage.GetMessage("Msg2003");
		private static readonly PropertyChangedEventArgs PlanDatePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(PlanDate));
		private DateTimeOffset _planDate = DateTimeOffset.Now.ToLocalTime();
		private static readonly PropertyChangedEventArgs TaskDataPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(TaskData));
		public string _taskData = GetTaskData(DateTimeOffset.Now.ToLocalTime());


		public DateTimeOffset PlanDate
		{
			get { return this._planDate; }
			set
			{
				if (this._planDate == value) { return; }
				this._planDate = value;
				this.PropertyChanged?.Invoke(this, PlanDatePropertyChangedEventArgs);
				TaskData = GetTaskData(PlanDate);
			}
		}

		public string TaskData
		{
			get { return this._taskData; }
			set
			{
				if (this._taskData == value) { return; }
				this._taskData = value;
				this.PropertyChanged?.Invoke(this, TaskDataPropertyChangedEventArgs);
			}
		}


		public void Initialize(Views.RegisterTaskPage registerTaskPage)
		{
			View = registerTaskPage;
		}

		async public void RegisterTaskData(object sender, RoutedEventArgs e)
		{

			try
			{
				string pattern = @"<.*?>";
				TaskData = Regex.Replace(TaskData, pattern, string.Empty);

				// Control Progress Dialog
				var progressDialog = new RegisterProgressDialog();
				var showingDialog = progressDialog.ShowAsync();

				// Changes ValidFlag value of All tasks for this plan date to false 
				// If there is a task data in TaskData, the ValidFlag value of the task will change to true after this below processing
				List<EachTask> specificDateClassificationData = GetSpecificDateEachTasks(PlanDate);
				foreach (EachTask classificationTask in specificDateClassificationData)
				{
					EachTaskModel.ChangeValidFlag(classificationTask.EachTaskId, false);
					List<EachTask> smallTaskList = GetSpecificTaskSmallTasks(classificationTask.EachTaskId);
					foreach (EachTask smallTask in smallTaskList)
					{
						EachTaskModel.ChangeValidFlag(smallTask.EachTaskId, false);
					}
				}

				if (!TaskData.StartsWith("#"))
				{
					showingDialog.Cancel();
					progressDialog.Hide();
					CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Msg1001"));

					return;
				}

				String TaskDataToRegister = "\r" + TaskData;

				TaskDataToRegister = TaskDataToRegister.Replace("\r\r", "\r");

				string[] largeTaskStrings = TaskDataToRegister.Split("\r#");
				int LargetTaskRank = 1;
				bool reviewRegisterFlag = false;

				foreach (string eachLargeTask in largeTaskStrings)
				{
					if (string.IsNullOrWhiteSpace(eachLargeTask))
					{
						continue;
					}

					List<string> taskStrings = eachLargeTask.Split("\r-").ToList();
					taskStrings = taskStrings.Select(str => str.Replace("\r", string.Empty)).ToList();

					string classificationTask = taskStrings[0].TrimStart();

					// If review task has been registerd already, change the flag to on to avoid registering twice 
					if (classificationTask == GetLangMessage.GetMessage("Msg2005"))
					{
						reviewRegisterFlag = true;
					}


					EachTask newClassificationTask = new EachTask()
					{
						PlanDate = this.PlanDate,
						Content = classificationTask,
						RegisteredDate = DateTime.Now.ToLocalTime(),
						ParentEachTaskId = string.Empty,
						Rank = LargetTaskRank,
						ValidFlag = true
					};

					// Checks if the task has been already registered by checking EachTaskId
					string classificationId = EachTaskModel.GetEachTaskId(newClassificationTask);
					if (string.IsNullOrWhiteSpace(classificationId))
					{
						classificationId = EachTaskModel.RegisterTask(newClassificationTask);
					}
					else
					{
						await EachTaskModel.UpdateTaskRankAsync(classificationId, LargetTaskRank);
						EachTaskModel.ChangeValidFlag(classificationId, true);
					}

					int smallTaskRank = 1;
					string smallTaskId = string.Empty;

					foreach (string eachTaskString in taskStrings.GetRange(1, taskStrings.Count - 1))
					{
						EachTask newTask = new EachTask()
						{
							PlanDate = this.PlanDate,
							Content = eachTaskString.Substring(1).TrimStart(),
							RegisteredDate = DateTimeOffset.Now.ToLocalTime(),
							ParentEachTaskId = classificationId.ToString(),
							Rank = smallTaskRank,
							ValidFlag = true
						};

						smallTaskId = EachTaskModel.GetEachTaskId(newTask);
						if (string.IsNullOrWhiteSpace(smallTaskId))
						{
							await EachTaskModel.RegisterTaskAsync(newTask);
						}
						else
						{
							await EachTaskModel.UpdateTaskRankAsync(smallTaskId, smallTaskRank);
							EachTaskModel.ChangeValidFlag(smallTaskId, true);
						}

						smallTaskRank = smallTaskRank + 1;
					}

					LargetTaskRank = LargetTaskRank + 1;

				}

				// Add ReviewTask at last
				if (!reviewRegisterFlag)
				{
					EachTask newClassificationTask = new EachTask()
					{
						PlanDate = this.PlanDate,
						Content = GetLangMessage.GetMessage("Msg2005"),
						RegisteredDate = DateTime.Now.ToLocalTime(),
						ParentEachTaskId = string.Empty,
						Rank = LargetTaskRank,
						ValidFlag = true
					};

					EachTaskModel.RegisterTask(newClassificationTask);

				}

				// Close Dialog Window
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


		public static string GetTaskData(DateTimeOffset planDate)
		{
			try
			{

				string taskData = string.Empty;

				List<EachTask> specificDateClassificationData = GetSpecificDateEachTasks(planDate);

				foreach (EachTask classificationTask in specificDateClassificationData)
				{
					taskData = $"{taskData}# {classificationTask.Content}\r";

					List<EachTask> smallTaskList = GetSpecificTaskSmallTasks(classificationTask.EachTaskId);

					foreach (EachTask smallTask in smallTaskList)
					{
						taskData = $"{taskData}- {smallTask.Content}\r";
					}

					taskData = $"{taskData}\r";

				}

				return taskData;


			}
			catch
			{
				return string.Empty;
			}
		}
		

	}
}
