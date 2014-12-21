using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace PCExpert.Core.DataAccess
{
	public class EntityMappingsList
	{
		private readonly IDictionary<Type, IMapping> _registeredMappings = new Dictionary<Type, IMapping>();

		public void AddMapping<T>(MappingBase<T> mapping)
			where T : class
		{
			_registeredMappings.Add(typeof (T), mapping);
		}

		public void ApplyMappings(DbModelBuilder modelBuilder)
		{
			foreach (var entityMapping in _registeredMappings.Values)
				entityMapping.MapEntity(modelBuilder);
		}
	}
}