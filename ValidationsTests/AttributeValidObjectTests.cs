using System;
using System.Collections.Generic;
using Rainsoft.Validations.Attributes;
using Xunit;

namespace ValidationsTests
{
    [Trait("Validations", "Attributes")]
    public class AttributeValidObjectTests
    {
        class A
        {
            [GreaterThan(1)]
            [LessThan(3)]
            public int I1 { get; set; }

            [LessThan(4)]
            public int I2 { get; set; }

            public B B { get; set; }
        }

        class B
        {
            [GreaterThan(-3)]
            [LessThan(-1)]
            public int I { get; set; }

            [NotFuture]
            public DateTime D1 { get; set; }

            [Future]
            public DateTime D2 { get; set; }

            [NotNull]
            public C C1 { get; set; }

            public C C2 { get; set; }
        }

        class C
        {
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

            [NotWhiteSpace]
            public string S2 { get; set; }

            [OneOf("bird", "cat", "dog")]
            public string S3 { get; set; }
        }

        [Fact]
        public void Validate_ComplexObject_AllSucceed()
        {
            // Arrange
            A a = new A
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
                        I2 = 2,
                        S2 = "xyz",
                        S3 = "cat"
                    },
                    C2 = null
                }
            };

            // Act
            IList<ValidationOffense> offenses = new List<ValidationOffense>();
            bool valid = a.IsValid(ref offenses);

            // Assert
            Assert.True(valid);
            Assert.Equal(0, offenses.Count);
        }
    }
}
