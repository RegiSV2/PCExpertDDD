using System.Collections.Generic;

namespace PCExpert.Web.Model.Core
{
	/// <summary>
	/// Represents a clickable item of a menu
	/// </summary>
	public class ReferenceMenuItemModel : MenuItemModel
	{
		public string Title { get; set; }
		public string Route { get; set; }
		public IList<MenuItemModel> ChildItems { get; set; }

		public ReferenceMenuItemModel(string title, string route)
			: this(title)
		{
			Route = route;
		}

		public ReferenceMenuItemModel(string title)
		{
			Title = title;
		}
	}
}