using System;
using System.Threading.Tasks;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Exceptions;

namespace PCExpert.Core.Tests.Utils
{
	public class FakeUnitOfWork : IUnitOfWork
	{
		public bool ShouldThrowPersistenceException { get; set; }

		public async Task Execute(Action action)
		{
			await StartTask(() => ExecuteActionInternal(action));
		}

		public async Task Execute(Action action, Action<PersistenceException> exceptionHandler)
		{
			try
			{
				await Execute(action);
			}
			catch (PersistenceException ex)
			{
				exceptionHandler(ex);
			}
		}

		private Task StartTask(Action action)
		{
			var task = new Task(action);
			task.Start();
			return task;
		}

		private void ExecuteActionInternal(Action action)
		{
			if (ShouldThrowPersistenceException)
				throw new PersistenceException("some msg");
			action();
		}
	}
}