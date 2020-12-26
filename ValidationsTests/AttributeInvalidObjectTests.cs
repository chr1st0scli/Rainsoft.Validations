using System;
using System.Collections.Generic;
using Rainsoft.Validations.Attributes;
using Xunit;

namespace ValidationsTests
{
    [Trait("Validations", "Attributes")]
    public class AttributeInvalidObjectTests
    {
        class A
        {
            [LessThan(1)]
            [GreaterThan(3)]
            public int I1 { get; set; }

            [LessThan(2)]
            public int I2 { get; set; }

            public B B { get; set; }
        }

        class B
        {
            [LessThan(-3)]
            [GreaterThan(-1)]
            public int I { get; set; }

            [Future]
            public DateTime D1 { get; set; }

            [NotFuture]
            public DateTime D2 { get; set; }

            public C C1 { get; set; }

            [NotNull]
            public C C2 { get; set; }
        }

        class C
        {
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

            [NotWhiteSpace]
            public string S2 { get; set; }

            [OneOf("bird", "cat", "dog")]
            public string S3 { get; set; }
        }

        private readonly A _a = new A
        {
            I1 = 2,
            I2 = 2,
            B = new B
            {
                I = -2,
                D1 = DateTime.Now - TimeSpan.FromDays(1),
                D2 = DateTime.Now + TimeSpan.FromDays(1),
                C1 = new C
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
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsValid(ref offenses);

            // Assert
            Assert.False(valid);
            Assert.Equal(23, offenses.Count);
        }

        [Fact]
        public void Validate_ComplexObject_StopsOnFirstFailure()
        {
            // Arrange
            IList<ValidationOffense> offenses = new List<ValidationOffense>();

            // Act
            bool valid = _a.IsValid(ref offenses, false);

            // Assert
            Assert.False(valid);
            Assert.Equal(1, offenses.Count);
        }
    }
}
