﻿using System;
using System.Collections.Generic;
using Rainsoft.Validations.Attributes;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;
using Xunit;

namespace ValidationsTests
{
    public class AttributeValidObjectTests
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

            [StartsWith("1")]
            [Length(2)]
            [LessThan(20)]
            public int A0I { get; set; }

            [ShorterThan(3)]
            protected string A0S0 { get; set; }

            [GreaterThan(1.5)]
            [LessThan(2)]
            [GreaterThanOrEqualTo(1.9)]
            [LessThanOrEqualTo(1.9)]
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

            [GreaterThan(-5)]
            [LessThan(5)]
            [GreaterThanOrEqualTo(-1)]
            [LessThanOrEqualTo(1)]
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

            [GreaterThan(1)]
            [LessThan(3)]
            [GreaterThanOrEqualTo(2)]
            [LessThanOrEqualTo(3)]
            public int I1 { get; set; }

            [LessThan(4)]
            public int I2 { get; set; }

            public B B { get; set; }

            public int I3 => _i3;

            [GreaterThan(1)]
            [LessThan(3)]
            [GreaterThanOrEqualTo(1)]
            [LessThanOrEqualTo(2)]
            private readonly int _i3;
        }

        class B
        {
            public B(DateTime d3, DateTime d4)
            {
                _d3 = d3;
                _d4 = d4;
                _c3 = new C(s4: "xyz")
                {
                    S1 = "xyz",
                    I1 = 111,
                    I2 = 2,
                    S2 = "xyz",
                    S3 = "cat"
                };
            }

            [GreaterThan(-3)]
            [LessThan(-1)]
            [GreaterThanOrEqualTo(-2)]
            [LessThanOrEqualTo(-2)]
            public int I { get; set; }

            [NotFuture]
            public DateTime D1 { get; set; }

            [Future]
            public DateTime D2 { get; set; }

            [NotNull]
            public C C1 { get; set; }

            public C C2 { get; set; }

            public DateTime D3 => _d3;

            public DateTime D4 => _d4;

            public C C3 => _c3;

            [NotFuture]
            private readonly DateTime _d3;

            [Future]
            private readonly DateTime _d4;

            [NotNull]
            private readonly C _c3;
        }

        class C
        {
            public C(string s4) => _s4 = s4;

            [StartsWith("xy")]
            [EndsWith("yz")]
            [ShorterThan(4)]
            [LongerThan(2)]
            [Length(3)]
            [Matches("[a-z]{3}")]
            public string S1 { get; set; }

            [StartsWith("11")]
            [EndsWith("11")]
            [ShorterThan(4)]
            [LongerThan(2)]
            [Length(3)]
            [Matches("[0-9]{3}")]
            public int I1 { get; set; }

            [OneOf(1, 2, 3, 4)]
            public int I2 { get; set; }

            [NotNullOrWhiteSpace]
            public string S2 { get; set; }

            [OneOf("bird", "cat", "dog")]
            public string S3 { get; set; }

            public string S4 => _s4;

            [StartsWith("xy")]
            [EndsWith("yz")]
            [ShorterThan(4)]
            [LongerThan(2)]
            [Length(3)]
            [Matches("[a-z]{3}")]
            [NotNullOrWhiteSpace]
            [OneOf("abc", "def", "xyz")]
            private readonly string _s4;
        }

        private readonly A _a = new A(i3: 2, s0: "hello there", s1: "wonderful world")
        {
            A0I = 15,
            A0S0P = "12",
            A0D = 1.9,
            A0S = "1234",
            I0 = 0,
            I1 = 2,
            I2 = 2,
            B = new B(d3: DateTime.Now.AddDays(-1), d4: DateTime.Now.AddDays(1))
            {
                I = -2,
                D1 = DateTime.Now - TimeSpan.FromDays(1),
                D2 = DateTime.Now + TimeSpan.FromDays(1),
                C1 = new C(s4: "xyz")
                {
                    S1 = "xyz",
                    I1 = 111,
                    I2 = 2,
                    S2 = "xyz",
                    S3 = "cat"
                },
                C2 = null
            }
        };

        [Fact]
        public void Validate_ComplexObject_AllSucceed()
        {
            // Arrange
            IList<ValidationOffense> propertyOffenses = new List<ValidationOffense>();
            IList<ValidationOffense> fieldOffenses = new List<ValidationOffense>();

            // Act
            bool propertiesValid = _a.IsValid(ref propertyOffenses);
            bool fieldsValid = _a.IsValid(ref fieldOffenses, ValidationMode.Fields);

            // Assert
            Assert.True(propertiesValid);
            Assert.Equal(0, propertyOffenses.Count);

            Assert.True(fieldsValid);
            Assert.Equal(0, fieldOffenses.Count);
        }

        [Theory]
        [InlineData(nameof(A.I1))]  // Property
        [InlineData(nameof(A.A0I))] // Inherited property
        [InlineData("_i3")] // Field
        [InlineData("a0D")] // Inherited field
        public void Validate_Property_AllSucceed(string memberName)
        {
            // Arrange
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsMemberValid(memberName, ref offenses);

            // Assert
            Assert.True(valid);
            Assert.Equal(0, offenses.Count);
        }
    }
}
