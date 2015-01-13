using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.Exceptions;

namespace PCExpert.DomainFramework.EF.Tests
{
	[TestFixture, Category("IntegrationTests")]
	public class EntityFrameworkUnitOfWorkTests
	{
		private EntityFrameworkUnitOfWork _unitOfWork;

		private TestDbContextProvider _provider;
		
		[SetUp]
		public void EstablishContext()
		{
			_provider = TestContextCreator.Create();
			_unitOfWork = new EntityFrameworkUnitOfWork(_provider);
		}

		[Test]
		public void Execute_DoesNotThrowException_ShouldCommitAllChanges()
		{
			//Arrange
			var addedComponent = CreateComponent();

			//Act
			_unitOfWork.Execute(() => { SaveComponent(addedComponent); }).Wait();
			var loadedComponents = LoadComponents();

			//Assert
			Assert.That(loadedComponents.Count == 1);
			Assert.That(loadedComponents.First().SameIdentityAs(addedComponent));
		}

		[Test]
		public async void Execute_ThrowsAnyException_ShouldNotCommitChanges()
		{
			//Arrange
			var addedComponent = CreateComponent();

			//Act
			try
			{
				await _unitOfWork.Execute(() =>
				{
					SaveComponent(addedComponent);
					throw new Exception();
				});
			}
			catch
			{
				// ignored
			}
			
			//Assert
			Assert.That(LoadComponents().Count == 0);
		}

		[Test]
		public async void Execute_PersistenceExceptionHandlerSpecified_ShouldCallHandlerWhenPersistenceExceptionOccurs()
		{
			//Arrange
			var handlerCalled = false;
			Action<PersistenceException> handler = x => { handlerCalled = true; };

			//Act
			await _unitOfWork.Execute(() =>
			{
				throw new PersistenceException("some msg");
			}, handler);

			//Assert
			Assert.That(handlerCalled);
		}

		private List<PCComponent> LoadComponents()
		{
			return _provider.PCExpertContext.PCComponents.ToList();
		}

		private void SaveComponent(PCComponent addedComponent)
		{
			_provider.PCExpertContext.PCComponents.Add(addedComponent);
		}

		private static PCComponent CreateComponent()
		{
			return DomainObjectsCreator.CreateComponent(1, ComponentType.HardDiskDrive);
		}
	}
}