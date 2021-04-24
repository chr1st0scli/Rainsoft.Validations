using Rainsoft.Validations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace ValidationsTests
{
    public class TypeValidatorTests
    {
        [Theory]
        [InlineData("004463499", 1234, true, 0)]
        [InlineData("004463439", 1234, false, 1)]
        [InlineData("014463499", 1234, false, 1)]
        [InlineData("104463439", 1234, false, 2)]
        [InlineData("104463439", 999, false, 3)]
        [InlineData("104463439", 10000, false, 3)]
        public void Validate_InstanceMembers_Correctly(string position, int taxNo, bool expectedIsValid, int expectedOffenses)
        {
            // Arrange
            IList<ValidationOffense> offenses = null;

            var employee = new Employee
            {
                Position = position,
                TaxNo = taxNo
            };

            var validator = new InstanceValidator<Employee>(employee);

            validator.Checks(e => e.Position)
                .StartsWith("00")
                .EndsWith("99");

            validator.Checks(e => e.TaxNo)
                .GreaterThan(999)
                .LessThan(10000);

            // Act
            bool isValid = validator.IsValid(ref offenses);

            // Assert
            Assert.Equal(expectedIsValid, isValid);
            Assert.Equal(expectedOffenses, offenses.Count);
        }

        [Fact]
        public void Validate_ErrorMessages()
        {
            // Arrange
            IList<ValidationOffense> offenses = null;

            string[] errorMessages =
            {
                "Position does not start with 0.",
                "Position does not end with 9.",
                "TaxNo is not less than 10000.",
                "Salary is not greater than 0."
            };

            var employee = new Employee
            {
                Position = "1",
                TaxNo = 10001,
                Salary = 0
            };

            var validator = new InstanceValidator<Employee>(employee);

            validator.Checks(e => e.Position)
                .StartsWith("0", errorMessage: errorMessages[0])
                .EndsWith("9", errorMessage: errorMessages[1]);

            validator.Checks(e => e.TaxNo)
                .LessThan(10000, errorMessage: errorMessages[2]);

            validator.Checks(e => e.Salary)
                .GreaterThan(0, errorMessage: errorMessages[3]);

            // Act
            bool isValid = validator.IsValid(ref offenses);

            // Assert
            Assert.False(isValid);
            Assert.Equal(4, offenses.Count);

            // Validate that error messages are in offenses.
            Assert.Equal(errorMessages[0], offenses[0].ErrorMessage);
            Assert.Equal(errorMessages[1], offenses[1].ErrorMessage);
            Assert.Equal(errorMessages[2], offenses[2].ErrorMessage);
            Assert.Equal(errorMessages[3], offenses[3].ErrorMessage);
        }

        [Fact]
        public void Validate_InstanceMembers_AllFailed()
        {
            // Arrange
            IList<ValidationOffense> offenses = null, offenses2 = null;

            var employee = new Employee
            {
                Name = "Jim",
                BirthDate = new DateTime(1945, 5, 7),
                TaxNo = 999,
                Position = "ABC",
                Department = "",
                Salary = -100,
                HireInfo = new HireDetails
                {
                    HireDate = DateTime.Now.AddDays(1),
                    HiredBy = null,
                    ContractEnd = DateTime.Now.AddYears(-1)
                },
                FormerEmployers = new[]
                {
                    new WorkExperience
                    {
                        Employer = "",
                        Years = -1
                    },
                    new WorkExperience
                    {
                        Employer = null,
                        Years = -2
                    }
                }
            };
            var validator = new InstanceValidator<Employee>(employee);

            validator.Checks(e => e.Name)
                .StartsWith("b")
                .StartsWith("j")
                .EndsWith("c")
                .EndsWith("M")
                .OfLength(1)
                .LongerThan(10)
                .ShorterThan(1);

            validator.Checks(e => e.Position)
                .Matches(new Regex("[0-9]{3}"));

            validator.Checks(e => e.TaxNo)
                .GreaterThan(1000, true)
                .LessThan(998, true)
                .StartsWith("1")
                .EndsWith("1")
                .OfLength(1)
                .LongerThan(10)
                .ShorterThan(1);

            validator.Checks(e => e.Salary)
                .GreaterThan(0)
                .LessThan(-200);

            validator.Checks(e => e.Department)
                .OneOf(new[] { "Sales", "HR", "Development" })
                .NotNullOrWhiteSpace();

            validator.Checks(e => e.BirthDate)
                .InFuture();

            validator.Checks(e => e.HireInfo.HireDate)
                .NotInFuture();

            validator.Checks(e => e.HireInfo.ContractEnd)
                .InFuture();

            validator.Checks(e => e.HireInfo.HiredBy)
                .NotNull();

            var detailValidators = employee.FormerEmployers
                .Select(e =>
                {
                    var experienceValidator = new InstanceValidator<WorkExperience>(e);

                    experienceValidator.Checks(e => e.Employer).NotNullOrWhiteSpace();
                    experienceValidator.Checks(e => e.Years).GreaterThan(0, true);

                    return experienceValidator;
                });

            // Act
            bool isValid = validator.IsValid(ref offenses);

            // Check that it stops on first error if asked so.
            validator.IsValid(ref offenses2, false);

            // Check the details.
            foreach (var detailValidator in detailValidators)
                detailValidator.IsValid(ref offenses);

            // Check with no offenses.
            bool isValid2 = validator.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.False(isValid2);
            Assert.Equal(27, offenses.Count);
            Assert.Equal(1, offenses2.Count);
        }

        [Fact]
        public void Validate_InstanceMembers_AllSucceeded()
        {
            // Arrange
            IList<ValidationOffense> offenses = null;

            var employee = new Employee
            {
                Name = "Jim",
                BirthDate = new DateTime(1945, 5, 7),
                TaxNo = 999,
                Position = "ABC",
                Department = "Sales",
                Salary = 5000.52,
                HireInfo = new HireDetails
                {
                    HireDate = DateTime.Now.AddYears(-5),
                    HiredBy = "James",
                    ContractEnd = DateTime.Now.AddYears(15),
                },
                FormerEmployers = new[]
                {
                    new WorkExperience
                    {
                        Employer = "SL",
                        Years = 10
                    },
                    new WorkExperience
                    {
                        Employer = "RL",
                        Years = 2
                    }
                }
            };
            var validator = new InstanceValidator<Employee>(employee);

            validator.Checks(e => e.Name)
                .StartsWith("J")
                .StartsWith("j", false)
                .EndsWith("m")
                .EndsWith("M", false)
                .OfLength(3)
                .LongerThan(1)
                .ShorterThan(4);

            validator.Checks(e => e.Position)
                .Matches(new Regex("[A-Z]{3}"));

            validator.Checks(e => e.TaxNo)
                .GreaterThan(998)
                .GreaterThan(999, true)
                .LessThan(1000)
                .LessThan(999, true)
                .StartsWith("9")
                .EndsWith("99")
                .OfLength(3)
                .LongerThan(1)
                .ShorterThan(4);

            validator.Checks(e => e.Salary)
                .GreaterThan(5000)
                .GreaterThan(5000.52, true)
                .LessThan(1000000)
                .LessThan(1000000, true);

            validator.Checks(e => e.Department)
                .OneOf(new[] { "Sales", "HR", "Development" })
                .NotNullOrWhiteSpace();

            validator.Checks(e => e.BirthDate)
                .NotInFuture();

            validator.Checks(e => e.HireInfo.HireDate)
                .NotInFuture();

            validator.Checks(e => e.HireInfo.ContractEnd)
                .InFuture();

            validator.Checks(e => e.HireInfo.HiredBy)
                .NotNull();

            var detailValidators = employee.FormerEmployers
                .Select(e =>
                {
                    var experienceValidator = new InstanceValidator<WorkExperience>(e);

                    experienceValidator.Checks(e => e.Employer).NotNullOrWhiteSpace();
                    experienceValidator.Checks(e => e.Years).GreaterThan(0, true);

                    return experienceValidator;
                });

            // Act
            bool isValid = validator.IsValid(ref offenses);
            // Check the details.
            foreach (var detailValidator in detailValidators)
                detailValidator.IsValid(ref offenses);

            // Check with no offenses.
            bool isValid2 = validator.IsValid();

            // Assert
            Assert.True(isValid);
            Assert.True(isValid2);
            Assert.Equal(0, offenses.Count);
        }
    }
}
