using System;
using System.Reflection;
using NUnit.Framework;
using PduSerializer.Internal.Reflection;
using PduSerializer.Internal.Reflection.DelegateFactory;

namespace PduSerializer.Tests.Internals
{
    [TestFixture]
    public class DelegateFactoryTests
    {
        public delegate void SetValueDelegate(ref ValueSource source, string value);

        internal delegate void DoIt3(ref ValueSource source, string value);

        public struct ValueSource
        {
            public string Value { get; set; }
        }

        public interface ISource
        {
            int Value { get; set; }
        }

        public class Source : ISource
        {
            public int Value2;
            public string Value3;
            public string Value4 { get; set; }

            #region ISource Members

            public int Value { get; set; }

            #endregion
        }

        [Test]
        public void FieldTests()
        {
            FieldInfo field = typeof (Source).GetField("Value2");
            var callback = (LateBoundFieldGet) new FieldDelegateFactory().CreateGet(field);

            var source = new Source {Value2 = 15};
            var result = (int) callback(source);

            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void PropertyTests()
        {
            PropertyInfo property = typeof (Source).GetProperty("Value", typeof (int));
            var callback = (LateBoundPropertyGet) new PropertyDelegateFactory().CreateGet(property);

            var source = new Source {Value = 5};
            var result = (int) callback(source);

            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Should_set_field_when_field_is_a_reference_type()
        {
            Type sourceType = typeof (Source);
            FieldInfo field = sourceType.GetField("Value3");
            var callback = (LateBoundFieldSet) new FieldDelegateFactory().CreateSet(field);

            var source = new Source();
            callback(source, "hello");

            Assert.That(source.Value3, Is.EqualTo("hello"));
        }

        [Test]
        public void Should_set_field_when_field_is_a_value_type()
        {
            Type sourceType = typeof (Source);
            FieldInfo field = sourceType.GetField("Value2");
            var callback = (LateBoundFieldSet) new FieldDelegateFactory().CreateSet(field);

            var source = new Source();
            callback(source, 5);

            Assert.That(source.Value2, Is.EqualTo(5));
        }

        [Test]
        public void Should_set_property_when_property_is_a_reference_type()
        {
            Type sourceType = typeof (Source);
            PropertyInfo property = sourceType.GetProperty("Value4");
            var callback = (LateBoundPropertySet) new PropertyDelegateFactory().CreateSet(property);

            var source = new Source();
            callback(source, "hello");

            Assert.That(source.Value4, Is.EqualTo("hello"));
        }

        [Test]
        public void Should_set_property_when_property_is_a_value_type()
        {
            Type sourceType = typeof (Source);
            PropertyInfo property = sourceType.GetProperty("Value");
            var callback = (LateBoundPropertySet) new PropertyDelegateFactory().CreateSet(property);

            var source = new Source();
            callback(source, 5);

            Assert.That(source.Value, Is.EqualTo(5));
        }

        [Test]
        public void Should_set_property_when_property_is_a_value_type_and_type_is_interface()
        {
            Type sourceType = typeof (ISource);
            PropertyInfo property = sourceType.GetProperty("Value");
            var callback = (LateBoundPropertySet) new PropertyDelegateFactory().CreateSet(property);

            var source = new Source();
            callback(source, 5);

            Assert.That(source.Value, Is.EqualTo(5));
        }


        [Test]
        public void Test_with_create_ctor()
        {
            Type sourceType = typeof (Source);

            object target = ObjectFactory.CreateObject(sourceType);

            Assert.That(target, Is.InstanceOf<Source>());
        }

        [Test]
        public void Test_with_value_object_create_ctor()
        {
            Type sourceType = typeof (ValueSource);

            object target = ObjectFactory.CreateObject(sourceType);

            Assert.That(target, Is.InstanceOf<ValueSource>());
        }
    }
}