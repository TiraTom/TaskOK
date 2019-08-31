using MemoApp.ViewModels;
using Windows.UI.Xaml.Controls;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace MemoApp.Helpers
{
	public sealed partial class RegisterProgressDialog : ContentDialog
	{
		public ViewModels.RegisterProgressDialogViewModel ViewModel { get; private set; } = new ViewModels.RegisterProgressDialogViewModel();

		public RegisterProgressDialog()
		{
			this.InitializeComponent();
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}
