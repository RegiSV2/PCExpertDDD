using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using System.Reflection;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class ComponentInterfaceMap : EntityMappingBase<ComponentInterface>
	{
		protected override void Map(EntityTypeConfiguration<ComponentInterface> mapping)
		{
			base.Map(mapping);
			mapping.HasRequired(x => x.Name);
		}
	}

	public class ComponentInterfaceConfiguration : EntityTypeConfiguration<ComponentInterface>
	{
		public ComponentInterfaceConfiguration()
		{
			HasKey(x => x.Id);
		}
	}

	public class PCComponentConfiguration : EntityTypeConfiguration<PCComponent>
	{
		private Expression<Func<PCComponent, T>> PrivateProperty<T>(string propertyName)
		{
			var property = typeof(PCComponent).GetProperty(propertyName,
				BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
			var param = Expression.Parameter(typeof(PCComponent));
			return Expression.Lambda<Func<PCComponent, T>>(
					Expression.Property(param, property), param);
		}

		public PCComponentConfiguration()
		{
			HasKey(x => x.Id);
			HasMany(PrivateProperty<ICollection<PCComponent>>("Components"))
				.WithMany().Map(m =>
				{
					m.MapLeftKey("ParentComponent_id");
					m.MapRightKey("ChildComponent_id");
					m.ToTable("ParentToChildComponents");
				});
			HasMany(PrivateProperty<ICollection<ComponentInterface>>("Slots"));
			HasRequired(x => x.PlugSlot);
		}
	}

	public class PCComponentMap : EntityMappingBase<PCComponent>
	{
		protected override void Map(EntityTypeConfiguration<PCComponent> mapping)
		{
			base.Map(mapping);
		}
	}
}