using System;
using System.Collections.Generic;
using Rainsoft.Validations.Attributes;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;
using Xunit;

namespace ValidationsTests
{
    public class AttributeInvalidObjectTests
    {
        class BaseA0
        {
            public string A0S0P
            {
                get => A0S0;
                set => A0S0 = value;
            }

            public double A0D
            {
                get => a0D;
                set => a0D = value;
            }

            public string A0S
            {
                get => _a0S;
                set => _a0S = value;
            }

            [StartsWith("ab")]
            [EndsWith("CD")]
            public string A0S1 { get; set; }

            [StartsWith("1")]
            [Length(2)]
            [LessThan(20)]
            public int A0I { get; set; }

            [ShorterThan(3)]
            protected string A0S0 { get; set; }

            [GreaterThan(1.5)]
            [LessThan(0.5)]
            [GreaterThanOrEqualTo(1.11)]
            [LessThanOrEqualTo(1.09)]
            protected double a0D;

            [LongerThan(3)]
            private string _a0S;
        }

        class BaseA1 : BaseA0
        {
            public BaseA1(string s0, string s1)
            {
                this.s0 = s0;
                _s1 = s1;
            }

            [GreaterThan(5)]
            [LessThan(-5)]
            [GreaterThanOrEqualTo(1)]
            [LessThanOrEqualTo(-1)]
            public int I0 { get; set; }

            public string S1 => _s1;

            [StartsWith("Hello", false)]
            protected readonly string s0;

            [EndsWith("World", false)]
            private readonly string _s1;
        }

        class A : BaseA1
        {
            public A(int i3, string s0, string s1) : base(s0, s1) => _i3 = i3;

            [LessThan(1)]
            [GreaterThan(3)]
            [GreaterThanOrEqualTo(3)]
            [LessThanOrEqualTo(1)]
            public int I1 { get; set; }

            [LessThan(2)]
            public int I2 { get; set; }

            public B B { get; set; }

            public int I3 => _i3;

            [LessThan(1)]
            [GreaterThan(3)]
            [GreaterThanOrEqualTo(2.1)]
            [LessThanOrEqualTo(1.9)]
            private readonly int _i3;
        }

        class B
        {
            public B(DateTime d3, DateTime d4)
            {
                _d3 = d3;
                _d4 = d4;
                _c3 = null;
            }

            [LessThan(-3)]
            [GreaterThan(-1)]
            [GreaterThanOrEqualTo(-1)]
            [LessThanOrEqualTo(-3)]
            public int I { get; set; }

            [Future]
            public DateTime D1 { get; set; }

            [NotFuture]
            public DateTime D2 { get; set; }

            public C C1 { get; set; }

            [NotNull]
            public C C2 { get; set; }

            public DateTime D3 => _d3;

            public DateTime D4 => _d4;

            public C C3 => _c3;

            [Future]
            private readonly DateTime _d3;

            [NotFuture]
            private readonly DateTime _d4;

            [NotNull]
            private readonly C _c3;
        }

        class C
        {
            public C(string s4, string s5)
            {
                _s4 = s4;
                _s5 = s5;
            }

            [StartsWith("ab")]
            [EndsWith("cd")]
            [ShorterThan(2)]
            [LongerThan(4)]
            [Length(4)]
            [Matches("[a-z]{4}")]
            public string S1 { get; set; }

            [StartsWith("123")]
            [EndsWith("456")]
            [ShorterThan(2)]
            [LongerThan(4)]
            [Length(4)]
            [Matches("[0-9]{4}")]
            public int I1 { get; set; }

            [OneOf(1, 2, 3, 4)]
            public int I2 { get; set; }

            [NotNullOrWhiteSpace]
            public string S2 { get; set; }

            [OneOf("bird", "cat", "dog")]
            public string S3 { get; set; }

            public string S4 => _s4;

            public string S5 => _s5;

            [StartsWith("123")]
            [EndsWith("456")]
            [ShorterThan(2)]
            [LongerThan(4)]
            [Length(4)]
            [Matches("[0-9]{4}")]
            [OneOf("abc", "def", "ghi")]
            private readonly string _s4;

            [NotNullOrWhiteSpace]
            private readonly string _s5;
        }

        private static readonly DateTime _past = DateTime.Now.AddDays(-1);
        private static readonly DateTime _future = DateTime.Now.AddDays(1);

        private readonly A _a = new A(i3: 2, s0: "abc", s1: "def")
        {
            A0I = 232,
            A0S0P = "123",
            A0D = 1.1,
            A0S = "123",
            A0S1 = "ABcd",
            I0 = 0,
            I1 = 2,
            I2 = 2,
            B = new B(d3: _past, d4: _future)
            {
                I = -2,
                D1 = _past,
                D2 = _future,
                C1 = new C(s4: "xyz", s5: " \t")
                {
                    S1 = "xyz",
                    I1 = 111,
                    I2 = 5,
                    S2 = "\t \n \r",
                    S3 = "camel"
                },
                C2 = null
            }
        };

        [Fact]
        public void Validate_ComplexObject_AllFailed()
        {
            // Arrange
            IList<ValidationOffense> propertyOffenses = new List<ValidationOffense>();
            IList<ValidationOffense> fieldOffenses = new List<ValidationOffense>();

            // Act
            bool propertiesValid = _a.IsValid(ref propertyOffenses);
            bool fieldsValid = _a.IsValid(ref fieldOffenses, ValidationMode.Fields);

            // Assert
            Assert.False(propertiesValid);
            Assert.Equal(37, propertyOffenses.Count);

            Assert.False(fieldsValid);
            Assert.Equal(22, fieldOffenses.Count);
        }

        [Fact]
        public void Validate_ComplexObject_StopsOnFirstFailure()
        {
            // Arrange
            IList<ValidationOffense> propertyOffenses = new List<ValidationOffense>();
            IList<ValidationOffense> fieldOffenses = new List<ValidationOffense>();

            // Act
            bool propertiesValid = _a.IsValid(ref propertyOffenses, ValidationMode.Properties, false);
            bool fieldsValid = _a.IsValid(ref fieldOffenses, ValidationMode.Fields, false);

            // Assert
            Assert.False(propertiesValid);
            Assert.Equal(1, propertyOffenses.Count);

            Assert.False(fieldsValid);
            Assert.Equal(1, fieldOffenses.Count);
        }

        [Fact]
        public void Validate_Property_AllFailed()
        {
            // Arrange
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsMemberValid(nameof(A.I1), ref offenses);

            // Assert
            Assert.False(valid);
            Assert.Equal(4, offenses.Count);
        }

        [Fact]
        public void Validate_Property_StopsOnFirstFailure()
        {
            // Arrange
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsMemberValid(nameof(A.I1), ref offenses, false);

            // Assert
            Assert.False(valid);
            Assert.Equal(1, offenses.Count);
        }

        [Fact]
        public void Validate_InheritedProperty_AllFailed()
        {
            // Arrange
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsMemberValid(nameof(A.A0I), ref offenses);

            // Assert
            Assert.False(valid);
            Assert.Equal(3, offenses.Count);
        }

        [Fact]
        public void Validate_Field_AllFailed()
        {
            // Arrange
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsMemberValid("_i3", ref offenses);

            // Assert
            Assert.False(valid);
            Assert.Equal(4, offenses.Count);
        }

        [Fact]
        public void Validate_InheritedField_AllFailed()
        {
            // Arrange
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsMemberValid("a0D", ref offenses);

            // Assert
            Assert.False(valid);
            Assert.Equal(4, offenses.Count);
        }
    }
}
