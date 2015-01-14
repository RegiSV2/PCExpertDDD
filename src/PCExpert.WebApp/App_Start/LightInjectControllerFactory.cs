using System;
using System.Web.Mvc;
using System.Web.Routing;
using LightInject;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.WebApp
{
	public class LightInjectControllerFactory : DefaultControllerFactory
	{
		private readonly IServiceContainer _iocContainer;

		public LightInjectControllerFactory(IServiceContainer iocContainer)
		{
			Argument.NotNull(iocContainer);
			_iocContainer = iocContainer;

		}

		protected override IController GetControllerInstance(
			RequestContext requestContext, Type controllerType)
		{
			if (controllerType == null)
				return null;

			return (IController) _iocContainer.Create(controllerType);
		}
	}
}