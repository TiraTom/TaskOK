using MemoApp.Helpers;
using MemoApp.Models;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using static MemoApp.Models.ConfigModel;

namespace MemoApp.ViewModels
{
	public class ConfigPageViewModel
	{
		public string LanguageConfigENLabel { get; set; } = "English";
		public string LanguageConfigJALabel { get; set; } = "日本語";

		public bool _isLanguageConfigEN;
		public bool IsLanguageConfigEN
		{
			get
			{
				if (string.IsNullOrEmpty(GetSpecificConfigValue(ConfigType.Language)))
				{
					return true;
				}

				if (!this._isLanguageConfigEN && !this.IsLanguageConfigJA)
				{
					return GetSpecificConfigValue(ConfigType.Language) == nameof(LanguageConfig.EN);
				}

				return this._isLanguageConfigEN;
			}

			set
			{
				if (this._isLanguageConfigEN == value) { return; }
				this._isLanguageConfigEN = value;
			}
		}

		public bool IsLanguageConfigJA { get; set; } = (GetSpecificConfigValue(ConfigType.Language) == nameof(LanguageConfig.JA));
		public string NotificationToggleLabel = GetLangMessage.GetMessage("Msg0001");
		public string LanguageConfigLabel = "言語/Language";
		public string LanguageAnnotation = GetLangMessage.GetMessage("Msg0006");
		public bool NotificationToggleValue { get; set; } = (GetSpecificConfigValue(ConfigType.NotificationFlag) == true.ToString());
		public string NotificationToggleOnLabel { get; } = GetLangMessage.GetMessage("Msg0004");
		public string NotificationToggleOffLabel { get; } = GetLangMessage.GetMessage("Msg0005");
		public string NotificationSpanMinuteLabel = GetLangMessage.GetMessage("Msg0002");
		public string NotificationSpanMinuteValue { get; set; } = GetSpecificConfigValue(ConfigType.NotificationSpanMinute);
		public string UpdateButtonLabel = GetLangMessage.GetMessage("Msg0003");
		public string NotificationAnnotation = GetLangMessage.GetMessage("Msg0007");
		public async void UpdateConfig()
		{
			try
			{
				await UpdateSpecificConfigAsync(ConfigType.NotificationFlag, NotificationToggleValue.ToString());
				await UpdateSpecificConfigAsync(ConfigType.NotificationSpanMinute, NotificationSpanMinuteValue);
				await UpdateSpecificConfigAsync(ConfigType.Language, GetLanguageConfig().ToString());
				CommonViewModel.LangSetting = GetLanguageConfig().ToString();

			}
			catch
			{
				CommonViewModel.NotifySystemMessage(GetLangMessage.GetMessage("Exception"));
			}
		}

		private LanguageConfig GetLanguageConfig()
		{
			if (this.IsLanguageConfigEN)
			{
				return LanguageConfig.EN;
			}
			else
			{
				return LanguageConfig.JA;
			}
		}

		async public void PageLoaded()
		{
			string value = GetSpecificConfigValue(ConfigType.Language);

			if (string.IsNullOrWhiteSpace(value))
			{
				await UpdateSpecificConfigAsync(ConfigType.Language, GetLanguageConfig().ToString());
			}
		}
	}

	public enum LanguageConfig
	{
		EN, JA
	}
}
