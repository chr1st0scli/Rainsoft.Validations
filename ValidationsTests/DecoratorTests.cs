using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Rainsoft.Validations.Core;
using Xunit;

namespace ValidationsTests
{
    public class DecoratorTests
    {
        [Fact]
        public void Validate_WithSingleLengthValidator_Correctly()
        {
            //Arrange
            IValueValidator<string> validator = new LengthValidator(5);

            //Act
            bool valid = validator.IsValid("hello");

            //Assert
            Assert.True(valid);
        }

        [Theory]
        [InlineData(typeof(StartsWithValidator))]
        [InlineData(typeof(EndsWithValidator))]
        public void Validate_NullValueStartsEnds_False(Type validatorType)
        {
            // Arrange
            var validator = (IValueValidator<string>)Activator.CreateInstance(validatorType, "", true, null);

            // Act
            bool isValid = validator.IsValid(null);

            // Assert
            // It can be said that a null value does not start or end with anything non null.
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(typeof(LengthValidator), 0, true)]      // null has a 0 length.
        [InlineData(typeof(LengthValidator), 10, false)]    // null does not have a length of 10.
        [InlineData(typeof(LongerValidator), 0, false)]     // null is not longer than 0.
        [InlineData(typeof(LongerValidator), 10, false)]    // null is not longer than 10.
        [InlineData(typeof(ShorterValidator), 0, false)]    // null is not shorter than 0, its length is 0.
        [InlineData(typeof(ShorterValidator), 10, true)]    // null is shorter than 10.
        public void Validate_NullValue_Correctly(Type validatorType, uint length, bool expected)
        {
            // Arrange
            var validator = (IValueValidator<string>)Activator.CreateInstance(validatorType, length, null);

            // Act
            bool isValid = validator.IsValid(null);

            // Assert
            Assert.Equal(expected, isValid);
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
            IValueValidator<string> validator =
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
            IValueValidator<DateTime> validator = new NotFutureValidator();

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
            IValueValidator<DateTime> validator = new FutureValidator();

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
            IValueValidator<string> validator =
                new LongerValidator(longer,
                new ShorterValidator(shorter));

            //Act
            bool valid = validator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(5, -4, 8, false, true)]    //5 is greater than -4 and less than 8
        [InlineData(5, 5, 8, false, false)]
        [InlineData(5, 5, 8, true, true)]
        [InlineData(5, -4, 5, false, false)]
        [InlineData(5, -4, 5, true, true)]
        [InlineData(5, 6, 8, false, false)]
        [InlineData(5, 2, 4, false, false)]
        public void Validate_NumberRange_Correctly(int value, int lowMargin, int highMargin, bool orEqualTo, bool expected)
        {
            //Arrange
            var validator =
                new GreaterValidator<int>(lowMargin, orEqualTo,
                new LesserValidator<int>(highMargin, orEqualTo));

            //Act
            bool valid = validator.IsValid(value);

            //Assert
            Assert.Equal(expected, valid);
        }

        [Theory]
        [InlineData(100.01, 0, 100, false, false)]
        [InlineData(49.99, 50, 100, false, false)]
        [InlineData(99.9, 0, 100, false, true)]
        [InlineData(64.01, 64.001, 64.02, false, true)]
        [InlineData(64.01, 64.01, 64.02, false, false)]
        [InlineData(64.01, 64.01, 64.02, true, true)]
        [InlineData(64.02, 64.01, 64.02, false, false)]
        [InlineData(64.02, 64.01, 64.02, true, true)]
        public void Validate_DoubleNumberRange_Correctly(double value, double lowMargin, double highMargin, bool orEqualTo, bool expected)
        {
            //Arrange
            var validator =
                new GreaterValidator<double>(lowMargin, orEqualTo,
                new LesserValidator<double>(highMargin, orEqualTo));

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
            IValueValidator<int> oneOfValidator = new OneOfValidator<int>(values);

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
            IValueValidator<string> validator = new RegexValidator(new Regex(regex));

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
            IValueValidator<Guy> workingAgeValidator =
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
            IValueValidator<Func<bool>> validator = new FuncValidator();

            //Act
            bool valid = validator.IsValid(() => _guy.Age >= 18);

            //Assert
            Assert.True(valid);
        }

        [Theory]
        [InlineData(6, 4, 7, false, true)] // 6 is between 4 and 7.
        [InlineData(6, 4, 7, true, true)]
        [InlineData(3, 4, 7, false, false)]
        [InlineData(3, 4, 7, true, false)]
        [InlineData(8, 4, 7, false, false)]
        [InlineData(8, 4, 7, true, false)]
        [InlineData(4, 4, 7, false, false)]
        [InlineData(4, 4, 7, true, true)]
        [InlineData(7, 4, 7, false, false)]
        [InlineData(7, 4, 7, true, true)]
        public void Validate_RangeWithIComparable_Correctly(int val, int low, int high, bool orEqualTo, bool expected)
        {
            // Arrange
            var lowMargin = new Comparable { I = low };
            var value = new Comparable { I = val };
            var highMargin = new Comparable { I = high };
            var validator =
                new GreaterValidator<Comparable>(lowMargin, orEqualTo,
                new LesserValidator<Comparable>(highMargin, orEqualTo));

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
            var validator = new OneOfValidator<TestClass>(values, new TestComparer());

            // Act
            bool isValid = validator.IsValid(new TestClass(value));

            // Assert
            Assert.Equal(expected, isValid);
        }
    }
}
