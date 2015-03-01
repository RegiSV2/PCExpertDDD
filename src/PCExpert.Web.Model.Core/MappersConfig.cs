using System;
using AutoMapper;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Model.Core
{
	public static class MappersConfig
	{
		public static void Configure(Func<ILinkSetterEngine> getEngine)
		{
			CreateMap<ComponentInterfaceVO, ComponentInterfaceModel>(getEngine);
		}

		private static void CreateMap<TFrom, TTo>(Func<ILinkSetterEngine> getEngine)
			where TTo : ILinksContaining
		{
			Mapper.CreateMap<TFrom, TTo>()
				.AfterMap((_, x) =>
				{
					var engine = getEngine();
					engine.SetLinks(x);
				});
		}
	}
}