﻿using MemoApp.Helpers;
using MempApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoApp.Models
{
	public static class TimeInfoModel
	{
		public static List<TimeInfo> GetSpecificTaskTimeInfo(string eachTaskId)
		{
			using (var db = new MemoAppContext())
			{
				return db.TimeInfos.Where(timeInfo => timeInfo.EachTask.EachTaskId == eachTaskId).Include(timeInfo => timeInfo.EachTask).ToList();
			}
		}

		public static string RegisterStart(string eachTaskId)
		{
			string msg = null;

			using (var db = new MemoAppContext())
			{
				var selectedTask = db.EachTasks.FirstOrDefault(eachTask => eachTask.EachTaskId == eachTaskId);

				List<TimeInfo> notStoppedInfo = db.TimeInfos.Where(timeInfo => timeInfo.Stop == DateTimeOffset.MinValue).Include(timeInfo => timeInfo.EachTask).ToList();
				TimeInfo doubleStartTask = notStoppedInfo.Where(timeInfo => timeInfo.EachTask.EachTaskId == eachTaskId).FirstOrDefault();

				if (doubleStartTask != null)
				{
					msg = GetLangMessage.GetMessage("Msg1015");
				}
				else
				{
					if (selectedTask.CompleteFlag == true)
					{
						EachTask completeTask = db.EachTasks.Where(eachTask => eachTask.EachTaskId == eachTaskId).FirstOrDefault();
						completeTask.CompleteFlag = false;

						msg = GetLangMessage.GetMessage("Msg1016");
					}
					else
					{
						TimeInfo startTimeInfo = new TimeInfo()
						{
							Start = DateTimeOffset.Now.ToLocalTime(),
							EachTask = db.EachTasks.FirstOrDefault(eachTask => eachTask.EachTaskId == eachTaskId)
						};
						db.TimeInfos.Add(startTimeInfo);

						selectedTask.StartedFlag = true;
					}

					notStoppedInfo.ForEach(timeInfo => timeInfo.Stop = DateTimeOffset.Now.ToLocalTime());

					db.SaveChanges();
				}
			}

			return msg;

		}

		public static string RegisterPause(string eachTaskId)
		{
			string msg = null;

			switch (CheckTaskStatus(eachTaskId))
			{
				case TaskStatus.AlreadyPaused:
					msg = GetLangMessage.GetMessage("Msg1017");
					break;

				case TaskStatus.AlreadyFinished:
					msg = GetLangMessage.GetMessage("Msg1018");
					break;

				case TaskStatus.NoProblem:
					using (var db = new MemoAppContext())
					{
						TimeInfo stopTimeInfo = db.TimeInfos.Where(timeInfo => timeInfo.EachTask.EachTaskId == eachTaskId && timeInfo.Stop == DateTimeOffset.MinValue).FirstOrDefault();
						stopTimeInfo.Stop = DateTimeOffset.Now.ToLocalTime();

						db.SaveChanges();
					}
					break;

				case TaskStatus.NotYetStarted:
					msg = GetLangMessage.GetMessage("Msg1019");
					break;

			}
			return msg;
		}

		public static string RegisterStop(string eachTaskId)
		{
			string msg = null;

			switch (CheckTaskStatus(eachTaskId))
			{
				case TaskStatus.AlreadyPaused:
					msg = GetLangMessage.GetMessage("Msg1020");
					using (var db = new MemoAppContext())
					{
						EachTask thisTask = db.EachTasks.Where(eachTask => eachTask.EachTaskId == eachTaskId).FirstOrDefault();
						thisTask.CompleteFlag = true;

						db.SaveChanges();
					}
					break;

				case TaskStatus.AlreadyFinished:
					msg = GetLangMessage.GetMessage("Msg1018");
					break;

				case TaskStatus.NoProblem:
					using (var db = new MemoAppContext())
					{
						EachTask thisTask = db.EachTasks.Where(eachTask => eachTask.EachTaskId == eachTaskId).FirstOrDefault();
						thisTask.CompleteFlag = true;
						TimeInfo stopTimeInfo = db.TimeInfos.Where(timeInfo => timeInfo.EachTask.EachTaskId == eachTaskId && timeInfo.Stop == DateTimeOffset.MinValue).FirstOrDefault();
						stopTimeInfo.Stop = DateTimeOffset.Now.ToLocalTime();

						db.SaveChanges();
					}
					break;

				case TaskStatus.NotYetStarted:
					msg = GetLangMessage.GetMessage("Msg1019");
					break;

			}

			return msg;

		}


		public static TaskStatus CheckTaskStatus(string eachTaskId)
		{
			using (var db = new MemoAppContext())
			{
				EachTask thisTask = db.EachTasks.Where(eachTask => eachTask.EachTaskId == eachTaskId).FirstOrDefault();
				if (thisTask.CompleteFlag == true)
				{
					return TaskStatus.AlreadyFinished;
				}
				else if (thisTask.StartedFlag == false)
				{
					return TaskStatus.NotYetStarted;
				}
				else
				{
					TimeInfo stopTimeInfo = db.TimeInfos.Where(timeInfo => timeInfo.EachTask.EachTaskId == eachTaskId && timeInfo.Stop == DateTimeOffset.MinValue).FirstOrDefault();

					if (stopTimeInfo == null)
					{
						return TaskStatus.AlreadyPaused;
					}
					else
					{
						return TaskStatus.NoProblem;
					}
				}
			}
		}

		async public static Task ChangeComplateFlag(string targetTaskId, bool flagValue)
		{
			using(var db = new MemoAppContext())
			{
				EachTask targetTask = db.EachTasks.Where(eachTask => eachTask.EachTaskId == targetTaskId).FirstOrDefault();
				targetTask.CompleteFlag = flagValue;
				await db.SaveChangesAsync();
			}
		} 

		public static string AlreadyStartedEachTaskId(DateTimeOffset dateTime)
		{
			using(var db = new MemoAppContext())
			{
				return db.TimeInfos.Where(timeInfo => timeInfo.Stop == DateTimeOffset.MinValue && timeInfo.Start.Date == dateTime.Date)
									  .Include(timeInfo => timeInfo.EachTask)
								   .FirstOrDefault()?
								   .EachTask?.EachTaskId;
			}
		}
	}

	public enum TaskStatus
	{
		NotYetStarted, AlreadyPaused, NoProblem, AlreadyFinished
	}
}
