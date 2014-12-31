using Moq;
using NUnit.Framework;
using PCExpert.Core.DomainFramework.DataAccess;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests.Repositories
{
	public abstract class RepositoryTests<TRepository, TEntity>
		where TEntity : class
	{
		protected TRepository _repository;

		protected Mock<PersistenceWorkplace> MockWorkplace; 

		protected abstract TRepository CreateRepositoryWithWorkplace(PersistenceWorkplace workplace);

		protected abstract void Save(TRepository repository, TEntity entity);

		[SetUp]
		public void EstablishContext()
		{
			MockWorkplace = new Mock<PersistenceWorkplace>();
			_repository = CreateRepositoryWithWorkplace(MockWorkplace.Object);
		}

		public virtual void Save_AnyValue_ShouldDelegateToPersistenceWorkplace()
		{
			//Arrange
			var workplace = new TestPersistenceWorkplace();
			var repository = CreateRepositoryWithWorkplace(workplace);
			var componentToInsert = new Mock<TEntity>();

			//Act
			Save(repository, componentToInsert.Object);

			//Assert
			Assert.That(workplace.IsUpdateCalled);
		}
	}
}