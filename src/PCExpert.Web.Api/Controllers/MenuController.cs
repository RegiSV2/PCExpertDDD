using System.Web.Http;
using PCExpert.Web.Model.Core;

namespace PCExpert.Web.Api.Controllers
{
	// GET: api/Menu
    public class MenuController : ApiController
    {
	    public MainMenuModel Get()
	    {
		    return new MainMenuModel
		    {
			    RootMenu = new MenuItemModel[]
			    {
				    new ReferenceMenuItemModel("Administration")
				    {
					    ChildItems = new MenuItemModel[]
					    {
						    new ReferenceMenuItemModel("Interfaces", "/admin/componentInterfaces"),
						    new SeparatorItemModel()
					    }
				    }
			    }
		    };
	    }
    }
}
