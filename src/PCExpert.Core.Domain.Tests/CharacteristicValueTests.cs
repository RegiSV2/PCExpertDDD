using System;
using System.Globalization;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class CharacteristicValueTests
	{
		private ComponentCharacteristic _characteristic;

		private FakeCharacteristicValue _characteristicValue;

		private class FakeCharacteristicValue : CharacteristicValue
		{
			public FakeCharacteristicValue(ComponentCharacteristic characteristic)
				:base(characteristic)
			{ }

			protected override string DoFormat(CultureInfo cultureInfo)
			{
				IsDoFormatCalled = true;
				return "";
			}

			public bool IsDoFormatCalled { get; private set; }
		}

		[SetUp]
		public void EstablishContext()
		{
			_characteristic = new Mock<ComponentCharacteristic>().Object;
			_characteristicValue = CreateCharacteristicWithSpecifiedDefaults();
		}

		[Test]
		public void Constructor_ValidArguments_ShouldCreateWithSpecifiedValue()
		{
			Assert.That(_characteristicValue.Characteristic, Is.EqualTo(_characteristic));
			Assert.That(_characteristicValue.Component, Is.Null);
		}

		[Test]
		public void Constructor_NullCharacteristic_ShouldThrowArgumentNullException()
		{
			Assert.That(() => new FakeCharacteristicValue(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Format_NullCulture_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _characteristicValue.Format(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Format_AnyCulture_ShouldCallDoFormatWithThatCulture()
		{
			//Arrange
			_characteristicValue.Format(CultureInfo.CurrentCulture);

			Assert.That(_characteristicValue.IsDoFormatCalled);
		}

		[Test]
		public void AttachToComponent_NullComponent_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _characteristicValue.AttachToComponent(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void AttachToComponent_ComponentNotAdded_ShouldUpdateComponentProperty()
		{
			//Arrange
			var newComponent = CreateComponent();
			_characteristicValue.AttachToComponent(newComponent);

			Assert.That(_characteristicValue.Component.SameIdentityAs(newComponent));
		}

		[Test]
		public void AttachToComponent_ComponentNotAdded_ShouldUpdateComponentIdProperty()
		{
			//Arrange
			var newComponent = CreateComponent();
			_characteristicValue.AttachToComponent(newComponent);

			Assert.That(_characteristicValue.ComponentId, Is.EqualTo(newComponent.Id));
		}

		[Test]
		public void AttachToComponent_SomeComponentAlreadyAdded_ShouldThrowInvalidOperationException()
		{
			//Arrange
			_characteristicValue.AttachToComponent(CreateComponent());

			Assert.That(() => _characteristicValue.AttachToComponent(CreateComponent()),
				Throws.InstanceOf<InvalidOperationException>());
		}

		private PCComponent CreateComponent()
		{
			return new Mock<PCComponent>().WithId(Guid.NewGuid()).Object;
		}

		private FakeCharacteristicValue CreateCharacteristicWithSpecifiedDefaults()
		{
			return new FakeCharacteristicValue(_characteristic);
		}
	}
}