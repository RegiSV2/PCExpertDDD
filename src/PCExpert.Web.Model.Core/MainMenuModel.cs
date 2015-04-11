using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PCExpert.Web.Model.Core
{
	/// <summary>
	/// Represents main menu of the application
	/// </summary>
	public class MainMenuModel
	{
		public IList<MenuItemModel> RootMenu { get; set; }
	}
}