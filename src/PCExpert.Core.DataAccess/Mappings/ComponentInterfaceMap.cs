using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq.Expressions;
using System.Reflection;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class ComponentInterfaceConfiguration : EntityTypeConfiguration<ComponentInterface>
	{
		public ComponentInterfaceConfiguration()
		{
			Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
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
			Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			HasMany(PrivateProperty<ICollection<PCComponent>>("Components"))
				.WithMany().Map(m =>
				{
					m.MapLeftKey("ParentComponent_id");
					m.MapRightKey("ChildComponent_id");
					m.ToTable("ParentToChildComponents");
				});
			HasMany(PrivateProperty<ICollection<ComponentInterface>>("Slots"))
				.WithMany().Map(m =>
				{
					m.MapLeftKey("Component_id");
					m.MapRightKey("ContainedSlot_id");
					m.ToTable("ComponentToSlots");
				});
			HasOptional(x => x.PlugSlot);
		}
	}
}