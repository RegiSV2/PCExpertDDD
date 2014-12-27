﻿using System;
using System.Linq;
using System.Linq.Expressions;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     And specification
	/// </summary>
	public class PersistenceAndSpecification<TEntity> : PersistenceAwareSpecification<TEntity>
		where TEntity : Entity
	{
		private readonly Expression<Func<TEntity, bool>> _cominedExpression;

		public PersistenceAndSpecification(params IPersistenceAwareSpecification<TEntity>[] specifications)
		{
			_cominedExpression = specifications
				.Select(x => x.GetConditionExpression())
				.Aggregate((combined, expr) => combined.And(expr));
		}

		public override Expression<Func<TEntity, bool>> GetConditionExpression()
		{
			return _cominedExpression;
		}
	}
}