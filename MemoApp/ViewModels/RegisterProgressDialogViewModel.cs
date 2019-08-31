using MemoApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoApp.ViewModels
{
	public class RegisterProgressDialogViewModel
	{
		public string TitleLabel { get; } = GetLangMessage.GetMessage("Msg5003");
	}
}
