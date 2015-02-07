using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http.Dependencies;
using LightInject;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Web.Api.Common
{
	public class LightInjectDependencyResolver : IDependencyResolver
	{
		private readonly IServiceContainer _serviceContainer;

		public LightInjectDependencyResolver(IServiceContainer serviceContainer)
		{
			Argument.NotNull(serviceContainer);
			_serviceContainer = serviceContainer;
		}

		public void Dispose()
		{
			_serviceContainer.Dispose();
		}

		public object GetService(Type serviceType)
		{
			return _serviceContainer.TryGetInstance(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _serviceContainer.GetAllInstances(serviceType);
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}
}