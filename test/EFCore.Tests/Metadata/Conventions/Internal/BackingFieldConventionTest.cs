// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable PossibleInvalidOperationException
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToAutoProperty

#pragma warning disable RCS1213 // Remove unused member declaration.

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal
{
    public class BackingFieldConventionTest
    {
        [Fact]
        public void Auto_property_name_matching_field_is_used_as_first_preference()
            => FieldMatchTest<TheDarkSideOfTheMoon>("ComfortablyNumb", "<ComfortablyNumb>k__BackingField");

        [Fact]
        public void Property_name_matching_field_is_used_as_next_preference()
            => FieldMatchTest<TheDarkSideOfTheMoon>("IsThereAnybodyOutThere", "IsThereAnybodyOutThere");

        [Fact]
        public void Camel_case_matching_field_is_used_as_next_preference()
            => FieldMatchTest<TheDarkSideOfTheMoon>("Breathe", "breathe");

        [Fact]
        public void Camel_case_matching_field_is_not_used_if_type_is_not_compatible()
            => FieldMatchTest<TheDarkSideOfTheMoon>("OnTheRun", "_onTheRun");

        [Fact]
        public void Underscore_camel_case_matching_field_is_used_as_next_preference()
            => FieldMatchTest<TheDarkSideOfTheMoon>("Time", "_time");

        [Fact]
        public void Underscpre_camel_case_matching_field_is_not_used_if_type_is_not_compatible()
            => FieldMatchTest<TheDarkSideOfTheMoon>("TheGreatGigInTheSky", "_TheGreatGigInTheSky");

        [Fact]
        public void Underscpre_matching_field_is_used_as_next_preference()
            => FieldMatchTest<TheDarkSideOfTheMoon>("Money", "_Money");

        [Fact]
        public void Underscore_matching_field_is_not_used_if_type_is_not_compatible()
            => FieldMatchTest<TheDarkSideOfTheMoon>("UsAndThem", "m_usAndThem");

        [Fact]
        public void M_underscpre_camel_case_matching_field_is_used_as_next_preference()
            => FieldMatchTest<TheDarkSideOfTheMoon>("AnyColourYouLike", "m_anyColourYouLike");

        [Fact]
        public void M_underscore_camel_case_matching_field_is_not_used_if_type_is_not_compatible()
            => FieldMatchTest<TheDarkSideOfTheMoon>("BrainDamage", "m_BrainDamage");

        [Fact]
        public void M_underscore_matching_field_is_used_as_next_preference()
            => FieldMatchTest<TheDarkSideOfTheMoon>("Eclipse", "m_Eclipse");

        [Fact]
        public void M_underscore_matching_field_is_not_used_if_type_is_not_compatible()
        {
            var entityType = new Model().AddEntityType(typeof(TheDarkSideOfTheMoon));
            var property = entityType.AddProperty("SpeakToMe", typeof(int));
            new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>()).Apply(property.Builder);

            Assert.Null(property.GetFieldName());
        }

        [Fact]
        public void Property_name_matching_field_is_used_as_first_preference_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("IsThereAnybodyOutThere", "IsThereAnybodyOutThere");

        [Fact]
        public void Camel_case_matching_field_is_used_as_next_preference_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("Breathe", "breathe");

        [Fact]
        public void Camel_case_matching_field_is_not_used_if_type_is_not_compatible_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("OnTheRun", "_onTheRun");

        [Fact]
        public void Underscore_camel_case_matching_field_is_used_as_next_preference_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("Time", "_time");

        [Fact]
        public void Underscpre_camel_case_matching_field_is_not_used_if_type_is_not_compatible_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("TheGreatGigInTheSky", "_TheGreatGigInTheSky");

        [Fact]
        public void Underscpre_matching_field_is_used_as_next_preference_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("Money", "_Money");

        [Fact]
        public void Underscore_matching_field_is_not_used_if_type_is_not_compatible_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("UsAndThem", "m_usAndThem");

        [Fact]
        public void M_underscpre_camel_case_matching_field_is_used_as_next_preference_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("AnyColourYouLike", "m_anyColourYouLike");

        [Fact]
        public void M_underscore_camel_case_matching_field_is_not_used_if_type_is_not_compatible_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("BrainDamage", "m_BrainDamage");

        [Fact]
        public void M_underscore_matching_field_is_used_as_next_preference_for_field_only()
            => FieldMatchTest<TheDarkerSideOfTheMoon>("Eclipse", "m_Eclipse");

        [Fact]
        public void M_underscore_matching_field_is_not_used_if_type_is_not_compatible_for_field_only()
        {
            var entityType = new Model().AddEntityType(typeof(TheDarkerSideOfTheMoon));
            var property = entityType.AddProperty("SpeakToMe", typeof(int));
            new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>()).Apply(property.Builder);

            Assert.Null(property.GetFieldName());
        }

        private static void FieldMatchTest<TEntity>(string propertyName, string fieldName)
        {
            var entityType = new Model().AddEntityType(typeof(TEntity));
            var property = entityType.AddProperty(propertyName, typeof(int));

            new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>()).Apply(property.Builder);

            Assert.Equal(fieldName, property.GetFieldName());
        }

        [Fact]
        public void Field_in_base_type_is_matched()
        {
            var entityType = new Model().AddEntityType(typeof(TheDarkSide));
            var property = entityType.AddProperty(OfTheMoon.TheGreatGigInTheSkyProperty);

            new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>()).Apply(property.Builder);

            Assert.Equal("_theGreatGigInTheSky", property.GetFieldName());
        }

        [Fact]
        public void Matched_field_on_base_class_is_found()
        {
            var entityType = new Model().AddEntityType(typeof(TheDarkSide));
            var property = entityType.AddProperty(TheDarkSide.OnBaseProperty);

            new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>()).Apply(property.Builder);

            Assert.Equal("_onBase", property.GetFieldName());
        }

        [Fact]
        public void Multiple_matches_throws()
        {
            var entityType = new Model().AddEntityType(typeof(AlwaysLookOnTheBrightSideOfLife));
            var property = entityType.AddProperty("OnTheRun", typeof(int));

            var convention = new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>());

            Assert.Equal(
                CoreStrings.ConflictingBackingFields(
                    "OnTheRun", nameof(AlwaysLookOnTheBrightSideOfLife), "_onTheRun", "m_onTheRun"),
                Assert.Throws<InvalidOperationException>(
                    () => convention.Apply(property.Builder)).Message);
        }

        [Fact]
        public void Object_field_non_object_property_matches_and_throws_ambiguous()
        {
            var entityType = new Model().AddEntityType(typeof(HesNotTheMessiah));
            var property = entityType.AddProperty("OnTheRun", typeof(int));

            var convention = new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>());

            Assert.Equal(
                CoreStrings.ConflictingBackingFields(
                    "OnTheRun", nameof(HesNotTheMessiah), "_onTheRun", "m_onTheRun"),
                Assert.Throws<InvalidOperationException>(
                    () => convention.Apply(property.Builder)).Message);
        }

        [Fact]
        public void Object_property_non_object_field_matches_and_throws_ambiguous()
        {
            var entityType = new Model().AddEntityType(typeof(HesAVeryNaughtyBoy));
            var property = entityType.AddProperty("OnTheRun", typeof(object));

            var convention = new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>());

            Assert.Equal(
                CoreStrings.ConflictingBackingFields(
                    "OnTheRun", nameof(HesAVeryNaughtyBoy), "_onTheRun", "m_onTheRun"),
                Assert.Throws<InvalidOperationException>(
                    () => convention.Apply(property.Builder)).Message);
        }

        [Fact]
        public void Explicitly_set_FieldInfo_is_used()
        {
            var entityType = new Model().AddEntityType(typeof(AlwaysLookOnTheBrightSideOfLife));
            var property = entityType.AddProperty("OnTheRun", typeof(int));
            property.SetField("m_onTheRun");

            new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>()).Apply(property.Builder);

            Assert.Equal("m_onTheRun", property.GetFieldName());
        }

        [Fact]
        public void FieldInfo_set_by_annotation_is_used()
        {
            var entityType = new Model().AddEntityType(typeof(AlwaysLookOnTheBrightSideOfLife));
            var property = entityType.AddProperty("OnTheRun", typeof(int));
            property.SetField("m_onTheRun", ConfigurationSource.DataAnnotation);

            new BackingFieldConvention(new TestLogger<DbLoggerCategory.Model>()).Apply(property.Builder);

            Assert.Equal("m_onTheRun", property.GetFieldName());
        }

#pragma warning disable RCS1222 // Merge preprocessor directives.
#pragma warning disable 649, 169
#pragma warning disable IDE0027 // Use expression body for accessors
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0044 // Add readonly modifier
        private class TheDarkSideOfTheMoon
#pragma warning restore RCS1222 // Merge preprocessor directives.
        {
            private readonly string m_SpeakToMe;
            private int _notSpeakToMe;

            public int SpeakToMe
            {
                get { return _notSpeakToMe; }
                set { _notSpeakToMe = value; }
            }

            private int? comfortablyNumb;

            public int ComfortablyNumb { get; set; }

            private readonly int IsThereAnybodyOutThere;

            private int? breathe;

            public int Breathe
            {
                get { return (int)breathe; }
                set { breathe = value; }
            }

            private readonly string onTheRun;
            private int? _onTheRun;

            public int OnTheRun
            {
                get { return (int)_onTheRun; }
                set { _onTheRun = value; }
            }

            private int? _time;

            public int Time
            {
                get { return (int)_time; }
                set { _time = value; }
            }

            private readonly string _theGreatGigInTheSky;
            private int? _TheGreatGigInTheSky;

            public int TheGreatGigInTheSky
            {
                get { return (int)_TheGreatGigInTheSky; }
                set { _TheGreatGigInTheSky = value; }
            }

            private int? _Money;

            public int Money
            {
                get { return (int)_Money; }
                set { _Money = value; }
            }

            private readonly string _UsAndThem;
            private int? m_usAndThem;

            public int UsAndThem
            {
                get { return (int)m_usAndThem; }
                set { m_usAndThem = value; }
            }

            private int? m_anyColourYouLike;

            public int AnyColourYouLike
            {
                get { return (int)m_anyColourYouLike; }
                set { m_anyColourYouLike = value; }
            }

            private readonly string m_brainDamage;
            private int? m_BrainDamage;

            public int BrainDamage
            {
                get { return (int)m_BrainDamage; }
                set { m_BrainDamage = value; }
            }

            private int? m_Eclipse;

            public int Eclipse
            {
                get { return (int)m_Eclipse; }
                set { m_Eclipse = value; }
            }
        }

        private class TheDarkerSideOfTheMoon
        {
            private readonly string m_SpeakToMe;

            private readonly int IsThereAnybodyOutThere;

            private int? breathe;

            private readonly string onTheRun;
            private int? _onTheRun;

            private int? _time;

            private readonly string _theGreatGigInTheSky;
            private int? _TheGreatGigInTheSky;

            private int? _Money;

            private readonly string _UsAndThem;
            private int? m_usAndThem;

            private int? m_anyColourYouLike;

            private readonly string m_brainDamage;
            private int? m_BrainDamage;

            private int? m_Eclipse;
        }

        private class TheDarkSide : OfTheMoon
        {
            public static readonly PropertyInfo OnBaseProperty
                = typeof(TheDarkSide).GetProperty(nameof(OnBase));

            public int OnBase
            {
                get { return _onBase; }
                set { _onBase = value; }
            }

            public new int Unrelated = 1;
        }

        private class OfTheMoon
        {
            public static readonly PropertyInfo TheGreatGigInTheSkyProperty =
                typeof(OfTheMoon).GetProperty(nameof(TheGreatGigInTheSky));

            private int? _theGreatGigInTheSky;

            public int TheGreatGigInTheSky
            {
                get { return (int)_theGreatGigInTheSky; }
                set { _theGreatGigInTheSky = value; }
            }

            protected int _onBase;

            public int Unrelated = 2;
        }

        private class AlwaysLookOnTheBrightSideOfLife
        {
            private readonly string onTheRun;
            private int? _onTheRun;
            private int? m_onTheRun;

            public int OnTheRun
            {
                get { return (int)m_onTheRun; }
                set { m_onTheRun = value; }
            }
        }

        private class HesNotTheMessiah
        {
            private object _onTheRun;
            private int m_onTheRun;

            public int OnTheRun
            {
                get { return (int)m_onTheRun; }
                set { m_onTheRun = value; }
            }
        }

        private class HesAVeryNaughtyBoy
        {
            private object _onTheRun;
            private int m_onTheRun;

            public object OnTheRun
            {
                get { return m_onTheRun; }
                set { m_onTheRun = (int)value; }
            }
        }

#pragma warning disable RCS1222 // Merge preprocessor directives.
#pragma warning restore 649, 169
#pragma warning restore IDE0027 // Use expression body for accessors
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore IDE0044 // Add readonly modifier
    }
#pragma warning restore RCS1222 // Merge preprocessor directives.
}
