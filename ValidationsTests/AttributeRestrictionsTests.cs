using Rainsoft.Validations.Attributes;
using Rainsoft.Validations.Attributes.Engine;
using Rainsoft.Validations.Core;
using System.Collections.Generic;
using Xunit;

namespace ValidationsTests
{
    public class AttributeRestrictionsTests
    {
        class A0
        {
            [EndsWith("abc")]
            public A0 A { get; set; }
        }

        class A1
        {
            [StartsWith("abc")]
            public A0 A { get; set; }
        }

        class A2
        {
            [Length(1)]
            public A0 A { get; set; }
        }

        class A3
        {
            [LongerThan(1)]
            public A0 A { get; set; }
        }

        class A4
        {
            [ShorterThan(1)]
            public A0 A { get; set; }
        }

        class A5
        {
            [Matches("[0-9]")]
            public A0 A { get; set; }
        }

        class A6
        {
            [NotFuture]
            public A0 A { get; set; }
        }

        class A7
        {
            [Future]
            public A0 A { get; set; }
        }

        class A8
        {
            [GreaterThan(1)]
            public A0 A { get; set; }
        }

        class A9
        {
            [LessThan(1)]
            public A0 A { get; set; }
        }

        class A10
        {
            [NotNullOrWhiteSpace]
            public int A { get; set; }
        }

        class A11
        {
            [GreaterThanOrEqualTo(1)]
            public A0 A { get; set; }
        }

        class A12
        {
            [LessThanOrEqualTo(1)]
            public A0 A { get; set; }
        }

        public static IEnumerable<object[]> GetA0()
        {
            yield return new[] { new A0 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA1()
        {
            yield return new[] { new A1 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA2()
        {
            yield return new[] { new A2 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA3()
        {
            yield return new[] { new A3 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA4()
        {
            yield return new[] { new A4 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA5()
        {
            yield return new[] { new A5 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA6()
        {
            yield return new[] { new A6 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA7()
        {
            yield return new[] { new A7 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA8()
        {
            yield return new[] { new A8 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA9()
        {
            yield return new[] { new A9 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA10()
        {
            yield return new[] { new A10 { A = 1 } };
        }

        public static IEnumerable<object[]> GetA11()
        {
            yield return new[] { new A11 { A = new A0() } };
        }

        public static IEnumerable<object[]> GetA12()
        {
            yield return new[] { new A12 { A = new A0() } };
        }

        [Theory]
        [MemberData(nameof(GetA0))]
        [MemberData(nameof(GetA1))]
        [MemberData(nameof(GetA2))]
        [MemberData(nameof(GetA3))]
        [MemberData(nameof(GetA4))]
        [MemberData(nameof(GetA5))]
        [MemberData(nameof(GetA6))]
        [MemberData(nameof(GetA7))]
        [MemberData(nameof(GetA8))]
        [MemberData(nameof(GetA9))]
        [MemberData(nameof(GetA10))]
        [MemberData(nameof(GetA11))]
        [MemberData(nameof(GetA12))]
        public void Validate_WithInvalidAttributes_Throws(object a)
        {
            // Arrange
            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }
    }
}
