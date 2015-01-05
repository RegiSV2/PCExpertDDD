using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Mappings
{
	public class ComponentInterfaceConfiguration : EntityTypeConfiguration<ComponentInterface>
	{
		public ComponentInterfaceConfiguration()
		{
			Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(x => x.Name).IsRequired().HasMaxLength(250)
				.HasColumnAnnotation(IndexAnnotation.AnnotationName,
					new IndexAnnotation(new IndexAttribute("Idx_ComponentInterface_NameUnique") {IsUnique = true}));
		}
	}
}