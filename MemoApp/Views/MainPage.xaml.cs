using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static MemoApp.Models.ConfigModel;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace MemoApp.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
		public ViewModels.MainPageViewModel ViewModel { get; private set; } = new ViewModels.MainPageViewModel();
		public ViewModels.CommonViewModel Common { get; private set; } = new ViewModels.CommonViewModel();

		public MainPage()
        {
            this.InitializeComponent();
			ApplicationView.PreferredLaunchViewSize = new Size(500, 600);
			ViewModel.Initialize(this);
			this.DataContext = ViewModel;

			if(Models.ConfigModel.GetSpecificConfigValue(ConfigType.NotificationSpanMinute) == null)
			{
				Models.ConfigModel.UpdateSpecificConfigAsyncVoid(ConfigType.NotificationSpanMinute, "60");
			}

			if (Models.ConfigModel.GetSpecificConfigValue(ConfigType.Language) == null)
			{
				Models.ConfigModel.UpdateSpecificConfigAsyncVoid(ConfigType.Language, ViewModels.LanguageConfig.EN.ToString());
			}

			if (Models.ConfigModel.GetSpecificConfigValue(ConfigType.NotificationFlag) == null)
			{
				Models.ConfigModel.UpdateSpecificConfigAsyncVoid(ConfigType.NotificationFlag, true.ToString());
			}
		}

		public void MoveMainPage(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainPage));
		}

		public void MoveRegisterTaskPage(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(RegisterTaskPage));
		}

		public void MoveLogPage(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(LogPage));
		}

		public void MoveConfigPage(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(ConfigPage));
		}

		public void MoveMemoSearchPage(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MemoSearchPage));
		}

		public void MoveHowToPage(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(HowToPage));
		}

	}
}
