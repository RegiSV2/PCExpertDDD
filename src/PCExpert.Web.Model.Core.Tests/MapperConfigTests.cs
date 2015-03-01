using System;
using AutoMapper;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Core.Tests.Utils;
using PCExpert.Web.Api.Common.WebModel;

namespace PCExpert.Web.Model.Core.Tests
{
	[TestFixture]
	public class MapperConfigTests
	{
		private Mock<ILinkSetterEngine> _linkSettingEngine;

		[SetUp]
		public void MapperConfigSetup()
		{
			_linkSettingEngine = new Mock<ILinkSetterEngine>();
			_linkSettingEngine.Setup(x => x.SetLinks(It.IsAny<ILinksContaining>())).Verifiable();
			MappersConfig.Configure(() => _linkSettingEngine.Object);
		}

		[Test]
		public void Map_ComponentInterfaceVO()
		{
			var from = new ComponentInterfaceVO
			{
				Id = Guid.NewGuid(),
				Name = NamesGenerator.ComponentInterfaceName()
			};
			var to = Mapper.Map<ComponentInterfaceModel>(from);

			AssertAllPropertiesSet(from, to);
			AssertLinksSet(to);
		}

		private void AssertAllPropertiesSet<TFrom, TTo>(TFrom from, TTo to)
			where TTo : TFrom, ILinksContaining
		{
			var properties = typeof (TFrom).GetProperties();
			foreach (var property in properties)
				Assert.That(property.GetValue(from).Equals(property.GetValue(to)));
		}

		private void AssertLinksSet<TLinksContaining>(TLinksContaining to)
			where TLinksContaining : ILinksContaining
		{
			_linkSettingEngine.Verify(x => x.SetLinks(to), Times.Once);
		}
	}
}