using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class PCComponentTests
	{
		[Test]
		public void Constructor_CanCreatePCComponent()
		{
			var componentName = "some name";
			var componentPrice = 100m;
			var component = new PCComponent(componentName, componentPrice);

			Assert.That(component.Name, Is.EqualTo(componentName));
			Assert.That(component.AveragePrice, Is.EqualTo(componentPrice));
		}
	}
}
