# Rainsoft.Validations
A library for easy validations. You can easily validate instances of your classes or any values, whether it be method parameters or anything else. It can also be used for validating user input or writing tests.

You may combine different validations at runtime using decorators to validate individual values. It also allows you to expressibly validate instances of your classes, either in source code or declaratively using attributes on class data members.

# Decorators #
Decorators are particularly useful for checking individual values against several validation rules.

At runtime, you may compose a complicated rule by combining simpler ones and then simply call the `IsValid` method with a value.

```csharp
// Combine multiple validators to run.
IValueValidator<string> validator =
    new LengthValidator(10,
    new StartsWithValidator("POS", true,
    new EndsWithValidator("Z", true)));

// Check if a single value complies with several rules.
string employeePositionCode = "POS123456Z";
bool isValid = validator.IsValid(employeePositionCode);
```
This is available in the `Rainsoft.Validations.Core` namespace with several more validators.

# Instance Validators #
Instance validators are particularly useful for checking whole instances of classes and their members.

```csharp
// Given a class instance.
var employee = new Employee
{
    Name = "Bob",
    TaxNo = 23434546,
    Position = "POS123456Z",
    Salary = 1150.50m
};

// You can create a validator for it.
var employeeValidator = new InstanceValidator<Employee>(employee);

// For every member you want to validate, specify the rules it must comply with.
employeeValidator.Checks(empl => empl.Position)
    .OfLength(10)
    .StartsWith("POS")
    .EndsWith("Z");

// You can also specify appropriate error messages for each violated rule.
employeeValidator.Checks(empl => empl.Name)
    .LongerThan(1, "Name should be longer than 1 character.")
    .ShorterThan(100, "Name should be shorter than 100 characters.");

employeeValidator.Checks(empl => empl.TaxNo)
    .Matches(new Regex("[0-9]{8}"));

employeeValidator.Checks(empl => empl.Salary)
    .GreaterThan(1000, true);

// Simply call IsValid to determine if your instance is valid.
bool isValid = employeeValidator.IsValid();

// Or you can find out exactly which rules are not satisfied by checking the offenses.
IList<ValidationOffense> offenses = null;
isValid = employeeValidator.IsValid(ref offenses);
```
This is also available in the `Rainsoft.Validations.Core` namespace with more validations.

# Attributes #
Attributes are particularly useful for defining validations declaratively on simple data-centric classes like DTOs.

You can declare a class and apply validation attributes on its properties or fields. The validations are performed on the class itself and all other related classes via aggregation/composition and inheritance relationships.

Given the following class.

```csharp
// Declaratively specify the validation rules and optional error messages in your class declaration.
public class Employee
{
    [LongerThan(1, ErrorMessage = "Name should be longer than 1 character.")]
    [ShorterThan(100, ErrorMessage = "Name should be shorter than 100 characters.")]
    public string Name { get; set; }

    [Length(10)]
    [StartsWith("POS")]
    [EndsWith("Z")]
    public string Position { get; set; }

    [Matches("[0-9]{8}")]
    public int TaxNo { get; set; }

    [GreaterThanOrEqualTo(1000.0)]
    public decimal Salary { get; set; }
}
```

You can validate it as follows.

```csharp
// Given a class instance.
var employee = new Employee
{
    Name = "Bob",
    TaxNo = 23434546,
    Position = "POS123456Z",
    Salary = 1150.50m
};

// Use namespace Rainsoft.Validations.Attributes.Engine to have the IsValid extension method available on your classes.
// Simply call IsValid to determine if your instance satisfies all rules.
bool isValid = employee.IsValid();

// Or you can find out exactly which rules are not satisfied by checking the offenses.
IList<ValidationOffense> offenses = null;
isValid = employee.IsValid(ref offenses);

// You can also validate a specific member of your object.
isValid = employee.IsMemberValid(nameof(Employee.Position));

// Or again...
IList<ValidationOffense> memberOffenses = null;
isValid = employee.IsMemberValid(nameof(Employee.Position), ref memberOffenses);
```
This is available in the `Rainsoft.Validations.Attributes` namespace.

# MVC and Web API Integration #
If you want to use or combine Rainsoft.Validations attributes with Microsoft's MVC or Web API infrastructure, you can use the Rainsoft.Validations.MSAnnotations library.
You can then use all Rainsoft attributes and the additional MSValidationAdapter to enable the .NET infrastructure use your attributes implicitly.

```csharp
public class Employee
{
    [MSValidationAdapter]
    [LongerThan(1, ErrorMessage = "Name should be longer than 1 character.")]
    [ShorterThan(100, ErrorMessage = "Name should be shorter than 100 characters.")]
    public string Name { get; set; }

    [MSValidationAdapter]
    [Length(10, ErrorMessage = "Position should be 10 characters long.")]
    [StartsWith("POS", ErrorMessage = "Position should start with POS.")]
    [EndsWith("Z", ErrorMessage = "Position should end with Z.")]
    public string Position { get; set; }

    [MSValidationAdapter]
    [Matches("[0-9]{8}", ErrorMessage = "Tax number should be 8 digits.")]
    public int TaxNo { get; set; }

    [MSValidationAdapter]
    [GreaterThanOrEqualTo(1000, ErrorMessage = "Salary should be greater than or equal to 1000.")]
    public decimal Salary { get; set; }
}

// Then, in your Controller or ApiController, you can check if the validation fails.
if (!ModelState.IsValid)
{
	//...
}
```

# Further Details #
There are validators for all basic needs like the following.
* Starting and ending values.
* Length related (equal, shorter or longer than a given length).
* Comparisons on primitives or custom IComparable classes.
* Regular expressions.
* DateTime related.
* Set related.
* Predicates that can be used with custom classes.

For more needs or even business specific validations, you can easily extend the library. Additional examples and details can be found in the ValidationsTests project.
