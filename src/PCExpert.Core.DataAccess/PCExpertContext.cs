using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using PCExpert.Core.DataAccess.Mappings;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess
{
	public class PCExpertContext : DbContext
	{
		private readonly IValidatorFactory _validatorFactory;

		public PCExpertContext()
		{
			_validatorFactory = null;
		}

		public PCExpertContext(IDatabaseInitializer<PCExpertContext> initializer,
			IValidatorFactory validatorFactory)
		{
			Database.SetInitializer(initializer);

			_validatorFactory = validatorFactory;
		}

		public PCExpertContext(string connectionStringName,
			IDatabaseInitializer<PCExpertContext> initializer,
			IValidatorFactory validatorFactory)
			: base(connectionStringName)
		{
			Database.SetInitializer(initializer);

			_validatorFactory = validatorFactory;
		}

		public DbSet<PCComponent> PCComponents { get; set; }
		public DbSet<ComponentInterface> ComponentInterfaces { get; set; }
		public DbSet<PCConfiguration> PCConfigurations { get; set; }
		public DbSet<ComponentCharacteristic> Characteristics { get; set; }
		public DbSet<CharacteristicValue> CharacteristicValues { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations
				.Add(new ComponentInterfaceConfiguration())
				.Add(new PCComponentConfiguration())
				.Add(new PCConfigurationConfiguration())
				.Add(new ComponentCharacteristicConfiguration())
				.Add(new CharacteristicValueConfiguration())
				.Add(new NumericCharacteristicValueConfiguration())
				.Add(new BoolCharacteristicValueConfiguration())
				.Add(new StringCharacteristicValueConfiguration());
		}

		protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry,
			IDictionary<object, object> items)
		{
			if (_validatorFactory != null)
			{
				var validator = _validatorFactory.GetValidator(entityEntry.Entity.GetType());
				if (validator != null)
				{
					var validationResult = validator.Validate(entityEntry.Entity);
					var validationErrors =
						validationResult.IsValid
							? Enumerable.Empty<DbValidationError>()
							: validator.Validate(entityEntry.Entity)
								.Errors.Select(x => new DbValidationError(x.PropertyName, x.ErrorMessage));

					return new DbEntityValidationResult(entityEntry, validationErrors);
				}
			}
			return base.ValidateEntity(entityEntry, items);
		}
	}
}