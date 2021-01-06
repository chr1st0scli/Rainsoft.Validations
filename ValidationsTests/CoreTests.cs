using System;
using System.Text.RegularExpressions;
using Rainsoft.Validations.Core;
using Xunit;

namespace ValidationsTests
{
    [Trait("Validations", "Core")]
    public class CoreTests
    {
        [Fact]
        public void Validate_WithSingleLengthValidator_Correctly()
        {
            //Arrange
            IValidator<string> validator = new LengthValidator(5);

            //Act
            bool valid = validator.IsValid("hello");

            //Assert
            Assert.True(valid);
        }

        [Theory]
        [InlineData("abcde", "a", "de", true, 5, true)]   //starts with 'a', ends with 'de', case sensitive and has length of 5
        [InlineData("abcde", "ab", "cde", true, 5, true)]
        [InlineData("abcde", "AB", "CDE", true, 5, false)]
        [InlineData("abcde", "AB", "CdE", false, 5, true)]
        [InlineData("abcde", "b", "cde", true, 5, false)]
        [InlineData("abcde", "ab", "f", true, 5, false)]
        [InlineData("abcde", "ab", "cde", true, 6, false)]
        [InlineData("abcde", "b", "c", true, 6, false)]
        public void Validate_LengthStartEnd_Correctly(string value, string start, string end, bool caseSensitive, uint length, bool expected)
        {
            //Arrange
            //Combine multiple validators to run.
            IValidator<string> validator =
                new LengthValidator(length,
                new StartsWithValidator(start, caseSensitive,
                new EndsWithValidator(end, caseSensitive)));

            //Act
            bool valid = validator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(-2, true)]  //2 days in the past
        [InlineData(-1, true)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        public void Validate_NotFutureDateTime_Correctly(int daysToAdd, bool expected)
        {
            //Arrange
            DateTime dt = DateTime.Now.AddDays(daysToAdd);
            IValidator<DateTime> validator = new NotFutureValidator();

            //Act
            bool valid = validator.IsValid(dt);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(-2, false)] //2 days in the past
        [InlineData(-1, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        public void Validate_FutureDateTime_Correctly(int daysToAdd, bool expected)
        {
            //Arrange
            IValidator<DateTime> validator = new FutureValidator();

            //Act
            bool valid = validator.IsValid(DateTime.Now.AddDays(daysToAdd));

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData("abc", 1, 4, true)] //longer than 1, shorter than 4
        [InlineData("abc", 3, 4, false)]
        [InlineData("abc", 1, 3, false)]
        [InlineData("abc", 3, 3, false)]
        public void Validate_LengthRange_Correctly(string value, uint longer, uint shorter, bool expected)
        {
            //Arrange
            IValidator<string> validator =
                new LongerValidator(longer,
                new ShorterValidator(shorter));

            //Act
            bool valid = validator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(5, -4, 8, true)]    //5 is greater than -4 and less than 8
        [InlineData(5, 5, 8, false)]
        [InlineData(5, -4, 5, false)]
        [InlineData(5, 6, 8, false)]
        public void Validate_NumberRange_Correctly(int value, int lowMargin, int highMargin, bool expected)
        {
            //Arrange
            var validator =
                new GreaterValidator<int>(lowMargin,
                new LesserValidator<int>(highMargin));

            //Act
            bool valid = validator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(100.01, 0, 100, false)]
        [InlineData(99.9, 0, 100, true)]
        public void Validate_DoubleNumberRange_Correctly(double value, int lowMargin, int highMargin, bool expected)
        {
            //Arrange
            var validator =
                new GreaterValidator<double>(lowMargin,
                new LesserValidator<double>(highMargin));

            //Act
            bool valid = validator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(3, true)]
        [InlineData(5, true)]
        [InlineData(7, false)]
        [InlineData(28, false)]
        public void Validate_OneOf_Correcly(int value, bool expected)
        {
            //Arrange
            int[] values = new[] { 1, 2, 3, 4, 5 };
            IValidator<int> oneOfValidator = new OneOfValidator<int>(values);

            //Act
            bool valid = oneOfValidator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(@"^[A-Z]{2}\d{2}$", "DF43", true)]
        [InlineData(@"^[A-Z]{2}\d{2}$", "DF4", false)]
        [InlineData(@"^[A-Z]{2}\d{2}$", "DF456", false)]
        [InlineData(@"^[A-Z]{2}\d{2}$", "DDF43", false)]
        public void Validate_WithRegex_Valid(string regex, string value, bool expected)
        {
            //Arrange
            IValidator<string> validator = new RegexValidator(new Regex(regex));

            //Act
            bool valid = validator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        class Guy
        {
            public int Age { get; set; }
            public string Name { get; set; }
        }
        readonly Guy _guy = new Guy { Age = 39, Name = "CG" };

        [Fact]
        public void Validate_WithPredicate_Valid()
        {
            //Arrange    
            IValidator<Guy> workingAgeValidator = 
                new PredicateValidator<Guy>(g => g.Age >= 18,
                new PredicateValidator<Guy>(g => g.Age <= 80));

            //Act
            bool valid = workingAgeValidator.IsValid(_guy);

            //Assert
            Assert.True(valid);
        }

        [Fact]
        public void Validate_WithFunc_Valid()
        {
            //Arrange
            IValidator<Func<bool>> validator = new FuncValidator();

            //Act
            bool valid = validator.IsValid(() => _guy.Age >= 18);

            //Assert
            Assert.True(valid);
        }

        [Theory]
        [InlineData(6, 4, 7, true)] // 6 is between 4 and 7.
        [InlineData(3, 4, 7, false)]
        [InlineData(8, 4, 7, false)]
        [InlineData(4, 4, 7, false)]
        [InlineData(7, 4, 7, false)]
        public void Validate_RangeWithIComparable_Correctly(int val, int low, int high, bool expected)
        {
            // Arrange
            var lowMargin = new Comparable { I = low };
            var value = new Comparable { I = val };
            var highMargin = new Comparable { I = high };
            var validator = new GreaterValidator<Comparable>(lowMargin,
                new LesserValidator<Comparable>(highMargin));

            // Act
            bool isValid = validator.IsValid(value);

            // Assert
            Assert.Equal(expected, isValid);
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(4, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(3, true)]
        public void Validate_OneOfWithIComparer_Correctly(int value, bool expected)
        {
            // Arrange
            var values = new[] { new TestClass(1), new TestClass(2), new TestClass(3) };
            var validator = new OneOfValidator<TestClass>(values, null, new TestComparer());

            // Act
            bool isValid = validator.IsValid(new TestClass(value));

            // Assert
            Assert.Equal(expected, isValid);
        }
    }
}
