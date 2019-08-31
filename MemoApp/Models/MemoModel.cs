using MempApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MemoApp.Models
{
	public static class MemoModel
	{
		public static void Register(string eachTaskId, string memoContent)
		{
			string pattern = @"<.*?>";
			memoContent = Regex.Replace(memoContent, pattern, string.Empty); 

			using (var db = new MemoAppContext())
			{
				EachTask targetTask = db.EachTasks.Where(eachTask => eachTask.EachTaskId == eachTaskId).FirstOrDefault();

				if (targetTask == null)
				{
					return;
				}

				Memo sameMemo = db.Memos.Where(memo => memo.EachTask.EachTaskId == eachTaskId).FirstOrDefault();

				if (sameMemo == null)
				{
					Memo newMemo = new Memo
					{
						CreateTime = DateTimeOffset.Now.ToLocalTime(),
						Content = memoContent,
						EachTask = targetTask
					};

					db.Memos.Add(newMemo);
				}
				else
				{
					sameMemo.Content = memoContent;
					sameMemo.UpdateTime = DateTimeOffset.Now.ToLocalTime();
				}

				db.SaveChanges();

			}
		}

		public static List<Memo> GetSpecificDateMemo(DateTimeOffset specificDate)
		{
			using(var db = new MemoAppContext())
			{
				List<Memo> memoList = db.Memos.Where(memo => memo.CreateTime.Date == specificDate.Date).Include(memo => memo.EachTask).ToList();

				return memoList;
			}
		}

		public static string GetSpecificEachTaskMemo(string eachTaskId)
		{
			using (var db = new MemoAppContext())
			{
				return db.Memos.Where(memo => memo.EachTask.EachTaskId == eachTaskId).FirstOrDefault()?.Content;
			}
		}

		public static List<Memo> SearchMemoContent(string searchWord)
		{
			string[] searchWordList = searchWord.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);

			using (var db = new MemoAppContext())
			{
				List<Memo> rtnList = db.Memos.Include(memo => memo.EachTask).ToList();

				for (int i = 0; i< searchWordList.Length; i++)
				{
					if (searchWordList[i].StartsWith("-") || searchWordList[i].StartsWith("－"))
					{
						rtnList = rtnList.Where(memo => (!memo.Content.ToLower().Contains(searchWordList[i].Substring(1).ToLower()))).ToList();
					}
					else
					{
						rtnList = rtnList.Where(memo => memo.Content.ToLower().Contains(searchWordList[i].ToLower())).ToList();
					}
				}

				return rtnList.GroupBy(memo => memo.EachTask.PlanDate)
							.SelectMany(memo => memo.OrderByDescending(m => m.EachTask.Rank))
							.GroupBy(memo => memo.EachTask.PlanDate)
							.SelectMany(memo => memo.OrderBy(m => m.EachTask.PlanDate))
							.Reverse()
							.ToList();
			}
		}

		public static List<Memo> SearchTaskName(string searchWord)
		{
			string[] searchWordList = searchWord.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);

			using (var db = new MemoAppContext())
			{
				List<Memo> rtnList = db.Memos.Include(memo => memo.EachTask).ToList();

				for (int i = 0; i < searchWordList.Length; i++)
				{
					if (searchWordList[i].StartsWith("-") || searchWordList[i].StartsWith("－"))
					{
						rtnList = rtnList.Where(memo => (!memo.EachTask.Content.ToLower().Contains(searchWordList[i].Substring(1).ToLower()))).ToList();
					}
					else
					{
						rtnList = rtnList.Where(memo => memo.EachTask.Content.ToLower().Contains(searchWordList[i].ToLower())).ToList();
					}
				}

				return rtnList.GroupBy(memo => memo.EachTask.PlanDate)
							.SelectMany(memo => memo.OrderByDescending(m => m.EachTask.Rank))
							.GroupBy(memo => memo.EachTask.PlanDate)
							.SelectMany(memo => memo.OrderBy(m => m.EachTask.PlanDate))
							.Reverse()
							.ToList();
			}
		}


	}
}
