using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.DataAccess.Tests
{
	[TestFixture, Category("IntegrationTests")]
	public class MappingTests
	{
		[Test]
		public void DbOperationsTest()
		{
			using (var dbContext = new PCExpertContext("testConString",
				new DropCreateDatabaseAlways<PCExpertContext>()))
			{
				var savedModel = InsertModel(dbContext);
				dbContext.SaveChanges();

				var loadedModel = LoadModel(dbContext);

				AssertModelsAreEqual(savedModel, loadedModel);
			}
		}

		private void AssertModelsAreEqual(PCExpertModel savedModel, PCExpertModel loadedModel)
		{
			CompareInterfaces(savedModel.Interfaces, loadedModel.Interfaces);
			CompareComponents(savedModel.Components, loadedModel.Components);
		}

		private void CompareComponents(List<PCComponent> savedComps, List<PCComponent> loadedComps)
		{
			foreach (var savedComp in savedComps)
			{
				var loadedComp = loadedComps.FirstOrDefault(x => x.SameIdentityAs(savedComp));

				AssertComponentsEqual(loadedComp, savedComp);
			}
		}

		private static void AssertComponentsEqual(PCComponent loadedComp, PCComponent savedComp)
		{
			Assert.That(loadedComp, Is.Not.Null);
			Assert.That(loadedComp.AveragePrice, Is.EqualTo(savedComp.AveragePrice));
			Assert.That(loadedComp.Name, Is.EqualTo(savedComp.Name));
			Assert.That(loadedComp.PlugSlot.SameIdentityAs(savedComp.PlugSlot));
			Assert.That(loadedComp.ContainedSlots.Count == savedComp.ContainedSlots.Count);
			foreach (var containedSlot in loadedComp.ContainedSlots)
				Assert.That(savedComp.ContainedSlots.Count(x => x.SameIdentityAs(containedSlot)) == 1);
		}

		private void CompareInterfaces(List<ComponentInterface> savedInts, List<ComponentInterface> loadedInts)
		{
			foreach (var savedInt in savedInts)
			{
				var loadedInt = loadedInts.FirstOrDefault(x => x.SameIdentityAs(savedInt));
				AssertInterfacesEqual(loadedInt, savedInt);
			}
		}

		private static void AssertInterfacesEqual(ComponentInterface loadedInt, ComponentInterface savedInt)
		{
			Assert.That(loadedInt, Is.Not.Null);
			Debug.Assert(loadedInt != null, "loadedInt != null");
			Assert.That(loadedInt.Name, Is.EqualTo(savedInt.Name));
		}

		private PCExpertModel LoadModel(PCExpertContext dbContext)
		{
			return new PCExpertModel
			{
				Interfaces = dbContext.ComponentInterfaces.ToList(),
				Components = dbContext.PCComponents.ToList()
			};
		}

		private PCExpertModel InsertModel(PCExpertContext dbContext)
		{
			var slotsToInsert = CreateSlots();
			var componentsToInsert = CreateComponents(slotsToInsert);

			dbContext.ComponentInterfaces.AddRange(slotsToInsert);
			dbContext.PCComponents.AddRange(componentsToInsert);

			return new PCExpertModel
			{
				Components = componentsToInsert,
				Interfaces = slotsToInsert
			};
		}

		private static void CreateRandomLinks(List<PCComponent> componentsToInsert)
		{
			for (var i = 0; i < 5; i++)
			{
				var parentElement = componentsToInsert.RandomElement();
				var childElement = componentsToInsert.RandomElementExcept(parentElement);
				if (!parentElement.ContainedComponents.Contains(childElement))
					parentElement.WithContainedComponent(childElement);
			}
		}

		private static List<PCComponent> CreateComponents(List<ComponentInterface> slotsToInsert)
		{
			var componentsToInsert = new List<PCComponent>();
			for (var i = 0; i < 5; i++)
			{
				var newComponent = new PCComponent(NamesGenerator.ComponentName(i), ComponentType.PowerSupply);
				newComponent
					.WithAveragePrice(100*(i + 1))
					.WithPlugSlot(slotsToInsert.RandomElement())
					.WithContainedSlot(slotsToInsert.RandomElement())
					.WithContainedSlot(slotsToInsert.RandomElementExcept(newComponent.ContainedSlots.ToList()));
				componentsToInsert.Add(newComponent);
			}
			CreateRandomLinks(componentsToInsert);
			return componentsToInsert;
		}

		private static List<ComponentInterface> CreateSlots()
		{
			var slotsToInsert = new List<ComponentInterface>();
			for (var i = 0; i < 5; i++)
				slotsToInsert.Add(new ComponentInterface(NamesGenerator.ComponentInterfaceName(i)));
			return slotsToInsert;
		}

		public class PCExpertModel
		{
			public List<PCComponent> Components { get; set; }
			public List<ComponentInterface> Interfaces { get; set; }
		}
	}
}