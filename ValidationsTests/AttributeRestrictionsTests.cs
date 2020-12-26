using System.Collections.Generic;
using Rainsoft.Validations.Attributes;
using Xunit;

namespace ValidationsTests
{
    [Trait("Validations", "Attributes")]
    public class AttributeRestrictionsTests
    {
        class A0
        {
            [EndsWith("abc")]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidEndsWith_Throws()
        {
            // Arrange
            var a = new A0 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A1
        {
            [StartsWith("abc")]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidStartsWith_Throws()
        {
            // Arrange
            var a = new A1 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A2
        {
            [Length(1)]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidLength_Throws()
        {
            // Arrange
            var a = new A2 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A3
        {
            [LongerThan(1)]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidLongerThan_Throws()
        {
            // Arrange
            var a = new A3 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A4
        {
            [ShorterThan(1)]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidShorterThan_Throws()
        {
            // Arrange
            var a = new A4 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A5
        {
            [Matches("[0-9]")]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidMatches_Throws()
        {
            // Arrange
            var a = new A5 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A6
        {
            [NotFuture]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidNotFuture_Throws()
        {
            // Arrange
            var a = new A6 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A7
        {
            [Future]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidFuture_Throws()
        {
            // Arrange
            var a = new A7 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A8
        {
            [GreaterThan(1)]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidGreaterThan_Throws()
        {
            // Arrange
            var a = new A8 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A9
        {
            [LessThan(1)]
            public A0 A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidLessThan_Throws()
        {
            // Arrange
            var a = new A9 { A = new A0() };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }

        class A10
        {
            [NotWhiteSpace]
            public int A { get; set; }
        }

        [Fact]
        public void Validate_WithInvalidNotWhiteSpace_Throws()
        {
            // Arrange
            var a = new A10 { A = 1 };

            // Act
            void Validate() => a.IsValid();

            // Assert
            Assert.Throws<InvalidRuleException>(Validate);
        }
    }
}
