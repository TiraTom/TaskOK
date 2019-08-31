using MemoApp.Helpers;
using MemoApp.Models;
using MempApp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static MemoApp.Views.LogPage;

namespace MemoApp.ViewModels
{
	public class LogPageViewModel
	{
		public string LogLabel { get; } = GetLangMessage.GetMessage("Msg3001");
		public string MemoLabel { get; } = GetLangMessage.GetMessage("Msg3002");
		public string TaskChoicePlaceholder { get; } = GetLangMessage.GetMessage("Msg3004");
		public ObservableCollection<EachTask> TaskListData { get; set; } = new ObservableCollection<EachTask>(GetSpecificDateEachTaskList(DateTimeOffset.Now.ToLocalTime()));
		public CalendarDatePicker LogDateCalendarDatePicker { get; set; } = new CalendarDatePicker();
		public ObservableCollection<Activity> ActivityLog { get; set; } = new ObservableCollection<Activity>(ModelsHelpers.GetSpecificDateActivityLog(DateTimeOffset.Now.ToLocalTime()));
		public ObservableCollection<Note> NoteList { get; set; } = new ObservableCollection<Note>(ChangeMemoListToNoteList(MemoModel.GetSpecificDateMemo(DateTimeOffset.Now.ToLocalTime())));
		public Note Note { get; set; }
		public Activity Activity { get; set; }

		public DateTimeOffset? _logDate;
		public DateTimeOffset LogDate {
			get { return this._logDate ?? DateTimeOffset.Now.ToLocalTime(); }
			set {
				this._logDate = value;
				ShorOtherDateLog();
			}

		}
		public string SelectedEachTaskId { get; set; } = string.Empty;

		public static List<Note> ChangeMemoListToNoteList(List<Memo> memoList)
		{
			try
			{

				List<Note> noteList = new List<Note>();

				foreach (Memo memo in memoList ?? new List<Memo>())
				{
					Note note = new Note()
					{
						TaskContent = $"{memo.EachTask?.Content}",
						Memo = memo.Content,
						EachTaskId = memo.EachTask.EachTaskId
					};
					noteList.Insert(0, note);
				}
				return noteList;


			}
			catch
			{
				return new List<Note>();
			}
		}



		private static List<EachTask> AddAllTask(List<EachTask> baseList)
		{

			try
			{
				if (baseList.Count >= 1)
				{
					if (!baseList.Any(eachTask => eachTask.Content == GetLangMessage.GetMessage("Msg3004")))
					{

						EachTask all = new EachTask()
						{
							EachTaskId = null,
							Content = GetLangMessage.GetMessage("Msg3004")
						};

						baseList.Insert(0, all);
					}
				}

				return baseList;


			}
			catch
			{
				return new List<EachTask>();
			}
		}

		public void ShowSpecificTaskLog()
		{
			try
			{
				ActivityLog.Clear();
				List<Activity> SpecificDateActivityList = ModelsHelpers.GetSpecificDateActivityLog(LogDate);
				NoteList.Clear();
				List<Note> SpecificDateNote = ChangeMemoListToNoteList(MemoModel.GetSpecificDateMemo(LogDate));

				if (SelectedEachTaskId != null)
				{
					SpecificDateActivityList = SpecificDateActivityList.Where(activity => activity.EachTaskId == SelectedEachTaskId).ToList();
					SpecificDateNote = SpecificDateNote.Where(note => note.EachTaskId == SelectedEachTaskId).ToList();
				}

				SpecificDateActivityList.ForEach(activity => ActivityLog.Add(activity));
				SpecificDateNote.ForEach(note => NoteList.Add(note));


			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}


		public void ShorOtherDateLog()
		{
			try
			{


				TaskListData.Clear();

				List<EachTask> specificTaskList = GetSpecificDateEachTaskList(LogDate);
				specificTaskList.ForEach(eachTask => TaskListData.Add(eachTask));

				ActivityLog.Clear();
				ModelsHelpers.GetSpecificDateActivityLog(LogDate).ForEach(activity => ActivityLog.Add(activity));

				NoteList.Clear();
				ChangeMemoListToNoteList(MemoModel.GetSpecificDateMemo(LogDate)).ForEach(memo => NoteList.Add(memo));


			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}

		public static List<EachTask> GetSpecificDateEachTaskList(DateTimeOffset specificDatetime)
		{

			try
			{

				List<EachTask> eachTaskList = new List<EachTask>();

				eachTaskList.AddRange(EachTaskModel.GetSpecificDateEachTasks(specificDatetime));
				eachTaskList = AddAllTask(eachTaskList);

				return eachTaskList;


			}
			catch
			{
				return new List<EachTask>();
			}
		} 

	}


	public class Note
	{
		public string TaskContent { get; set; } = string.Empty;
		public string Memo { get; set; } = string.Empty;
		public string EachTaskId { get; set; } = string.Empty;
	}

	public class Activity
	{
		public DateTimeOffset ExactStartTime { get; set; } = DateTimeOffset.MinValue;
		public string StartTime { get; set; } = string.Empty;
		public string StopTime { get; set; } = string.Empty;
		public string TaskContent { get; set; } = string.Empty;
		public string EachTaskId { get; set; } = string.Empty;
	}
}
