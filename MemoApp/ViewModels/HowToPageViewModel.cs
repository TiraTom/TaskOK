using MemoApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoApp.ViewModels
{
	public class HowToPageViewModel
	{
		public Views.HowToPage View { get; private set; } = null;

		public void Initialize(Views.HowToPage howToPage)
		{
			View = howToPage;
		}

		public string img1Path { get; } = $"ms-appx:///HowToUseImage/{CommonViewModel.LangSetting}/manual1.png";
		public string img2Path { get; } = $"ms-appx:///HowToUseImage/{CommonViewModel.LangSetting}/manual2.png";
		public string img3Path { get; } = $"ms-appx:///HowToUseImage/{CommonViewModel.LangSetting}/manual3.png";
		public string img4Path { get; } = $"ms-appx:///HowToUseImage/{CommonViewModel.LangSetting}/manual4.png";
		public string img5Path { get; } = $"ms-appx:///HowToUseImage/{CommonViewModel.LangSetting}/manual5.png";
		public string img6Path { get; } = $"ms-appx:///HowToUseImage/{CommonViewModel.LangSetting}/manual6.png";
		public string img7Path { get; } = $"ms-appx:///HowToUseImage/{CommonViewModel.LangSetting}/manual7.png";

		public string img2Title { get; } = GetLangMessage.GetMessage("Msg6001");
		public string img4Title { get; } = GetLangMessage.GetMessage("Msg6002");
		public string img5Title { get; } = GetLangMessage.GetMessage("Msg6003");
		public string img6Title { get; } = GetLangMessage.GetMessage("Msg6004");
		public string img7Title { get; } = GetLangMessage.GetMessage("Msg6005");
	}
}
