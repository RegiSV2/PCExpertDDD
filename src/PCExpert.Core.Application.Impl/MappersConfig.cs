using AutoMapper;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Core.Domain;

namespace PCExpert.Core.Application.Impl
{
	public static class MappersConfig
	{
		public static void Configure()
		{
			Mapper.CreateMap<ComponentInterface, ComponentInterfaceVO>();
		}
	}
}