using System;
using AutoMapper;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Model.Core
{
	public class MappersConfig
	{
		public static void Configure(Func<LinkSettingEngine> engine)
		{
			CreateMap<ComponentInterfaceVO, ComponentInterfaceModel>(engine);
		}

		private static void CreateMap<TFrom, TTo>(Func<LinkSettingEngine> engine)
			where TTo : ILinksContaining
		{
			Mapper.CreateMap<TFrom, TTo>()
				.AfterMap((_, x) => engine().SetLinks(x));
		}
	}
}