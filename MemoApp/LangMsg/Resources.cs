using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoApp.LangMsg
{
	public static class Resouces
	{
		public static readonly Dictionary<string, string> EN = new Dictionary<string, string>()
		{
			// MenuLabel
			{"MainPageButtonLabel", "MAIN"},
			{"RegisterPageButtonLabel", "TASK"},
			{"LogPageButtonLabel", "LOG"},
			{"MemoSearchPageButtonLabel", "SEARCH"},
			{"HowToPageButtonLabel", "HowTo"},
			{"ConfigPageButtonLabel", "CONFIG" },

			// ProgramMessage
			{"Exception", "Really Sorry, SystemError happened," },


			// Config
			{"Msg0001", "Notice"},
			{"Msg0002", "Interval(Min)" },
			{"Msg0003", "Update" },
			{"Msg0004", "On" },
			{"Msg0005", "Off" },
			{"Msg0006", "言語設定はページ切り替え後に反映されます。" },
			{"Msg0007", "This will be updated after clicking the update button, and restarting this app." },

			// Main
			{"Msg1001", "Please select a task" },
			{"Msg1002", "Start" },
			{"Msg1003", "Pause" },
			{"Msg1004", "Finish" },
			{"Msg1005", "Take a note" },
			{"Msg1006", "Register" },
			{"Msg1007", "TaskSelect" },
			{"Msg1008", "Now Status" },
			{"Msg1009", "Not Started Yet" },
			{"Msg1010", "Now Executing" },
			{"Msg1011", "Interruption" },
			{"Msg1012", "Completed" },
			{"Msg1013", "Message from this App" },
			{"Msg1014", "Write anything you like* about the task. \r\r*'<' and '>' will be deleted." },
			{"Msg1015", "It has been already started" },
			{"Msg1016", "You started the task which was already completed. The complete flag is removed from the task." },
			{"Msg1017", "It has been already paused." },
			{"Msg1018", "It has been already finished." },
			{"Msg1019", "It has not been started yet." },
			{"Msg1020", "It was marked completed while paused" },



			// RegisterTask
			{"Msg2001", "Today's Task" },
			{"Msg2002", "Register" },
			{"Msg2003", "Please write your tasks here. \rIf you write a task with '#' on the head of it, it will be recognized as one task. \rIf you write a task with '-' on the head of it, it will be recognized as a small task belonging to the above \r\rNOTE: '<' and '>' will be removed.\r\rEx) \r#Cook a cup noodle \r-Open it \r-Pour hot water \r-Wait for 3 minutes\r" },
			{"Msg2004", "Please start with '#'(by this, this app can recognize these letters as tasks)" },
			{"Msg2005", "Review Today" },

			// Log
			{"Msg3001", "Log" },
			{"Msg3002", "Note" },
			{"Msg3003", "ActivityLog" },
			{"Msg3004", "All tasks" },

			// Search
			{"Msg4001", "Please type a search word" },
			{"Msg4002", "TaskName of memo Search" },
			{"Msg4003", "Note Search" },

			// Notification
			{"Msg5001", "Message from this App" },
			{"Msg5002", "Registering has completed" },
			{"Msg5003", "Now Loading..." },
			{"Msg5004", "minutes have passed since you had started the task. \rHow about taking a rest?" },


			// HowToUse
			{"Msg6001", "MainPage" },
			{"Msg6002", "TaskRegisterPage" },
			{"Msg6003", "LogPage" },
			{"Msg6004", "SearchPage" },
			{"Msg6005", "ConfigPage" },

		};


		public static readonly Dictionary<string, string> JA = new Dictionary<string, string>()
		{
			// MenuLabel
			{"MainPageButtonLabel", "MAIN"},
			{"RegisterPageButtonLabel", "TASK"},
			{"LogPageButtonLabel", "LOG"},
			{"MemoSearchPageButtonLabel", "SEARCH"},
			{"HowToPageButtonLabel", "使い方"},
			{"ConfigPageButtonLabel", "CONFIG" },

			// ProgramMessage
			{"Exception", "申し訳ございません。システムエラーが発生しました。" },


			// Config
			{"Msg0001", "通知"},
			{"Msg0002", "間隔(分)" },
			{"Msg0003", "更新" },
			{"Msg0004", "オン" },
			{"Msg0005", "オフ" },
			{"Msg0006", "The language setting will be reload after changing pages" },
			{"Msg0007", "「更新」ボタンをクリックしたうえでアプリを再起動した時に反映されます。" },

			// Main
			{"Msg1001", "タスクを選択してください" },
			{"Msg1002", "開始" },
			{"Msg1003", "中断" },
			{"Msg1004", "完了" },
			{"Msg1005", "メモを残す" },
			{"Msg1006", "登録" },
			{"Msg1007", "タスク選択" },
			{"Msg1008", "現在の状態" },
			{"Msg1009", "作業開始前" },
			{"Msg1010", "実行中" },
			{"Msg1011", "中断中" },
			{"Msg1012", "完了済" },
			{"Msg1013", "アプリからのお知らせ" },
			{"Msg1014", "タスクに関するメモを自由に書き込んでください。\r\rなお、'<'と'>'は削除されるので注意してください。" },
			{"Msg1015", "既に開始済みです。" },
			{"Msg1016", "完了済みタスクを再開しました。完了済みのマークは削除されます。" },
			{"Msg1017", "すでに中断中です。" },
			{"Msg1018", "すでに完了済みです。" },
			{"Msg1019", "まだ開始されていません。" },
			{"Msg1020", "中断中のまま完了となりました" },

			// RegisterTask
			{"Msg2001", "本日のタスク" },
			{"Msg2002", "登録" },
			{"Msg2003", "タスク入力欄\r\r「#」から始まる行は１つのタスク、「-」で始まる行はタスクに属する小タスクとして認識されます。\r\r'<'や'>'は削除されるので注意してください。\r\r入力例）\r#カップ麺を作る\r-ふたを開ける\r-お湯を注ぐ\r-３分待つ\r\r\r" },
			{"Msg2004", "タスクは「#」から始まるようにしてください（そうしないとタスクをうまく登録できません）" },
			{"Msg2005", "本日の振り返り" },

			// Log
			{"Msg3001", "ログ" },
			{"Msg3002", "メモ" },
			{"Msg3003", "行動ログ" },
			{"Msg3004", "全てのタスク" },

			// Search
			{"Msg4001", "検索ワードを入れてください" },
			{"Msg4002", "メモのタスク名検索" },
			{"Msg4003", "メモ内検索" },

			// Notification
			{"Msg5001", "アプリからのお知らせ" },
			{"Msg5002", "登録処理が完了しました" },
			{"Msg5003", "処理中です..." },
			{"Msg5004", "分が作業開始から経過しました。\r少し休憩をはさみましょう。" },


			// HowToUse
			{"Msg6001", "メインページ" },
			{"Msg6002", "タスク登録ページ" },
			{"Msg6003", "ログページ" },
			{"Msg6004", "検索ページ" },
			{"Msg6005", "設定ページ" },

		};
	}
}
