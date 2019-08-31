using MemoApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Resources;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace MemoApp.Helpers
{
	public static class GetLangMessage
	{
		public static string GetMessage(string msgId)
		{
			string msg = string.Empty;

			try
			{
				switch (CommonViewModel.LangSetting)
				{
					case nameof(LanguageConfig.EN):

						msg = LangMsg.Resouces.EN[msgId];
						break;

					case nameof(LanguageConfig.JA):

						msg = LangMsg.Resouces.JA[msgId];
						break;

					default:
						msg = string.Empty;
						break;
				}

			}
			catch 
			{
				msg = string.Empty;
			}

			return msg;
		}

	}
}
