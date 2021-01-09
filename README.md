# Rainsoft.Validations
A library for easy validations. You can easily validate instances of your classes (DTOs, POCOs) or any values, wheter it be method parameters or anything else.

It consists of two main mechanisms.

* The first one is combining different validations at runtime using decorators.

* The second one is declarative using attributes on class data members.

# Core #
At runtime, you may compose a complicated rule by combining simpler ones and then simply call the `IsValid` method with a value. You can of course pass that validator to whatever code you want that does not need to be aware of the validation details, i.e. what that validator does.

```
// Combine multiple validators to run.
IValidator<string> validator =
    new LengthValidator(21,
    new StartsWithValidator("hello", false,
    new EndsWithValidator("world", false)));

// Check if a value is valid.
bool valid = validator.IsValid("Hello Wonderful World");
```
This is available in the `Rainsoft.Validations.Core` namespace.

# Attributes #
You can declare a class and apply validation attributes on its properties or fields. The validations are performed on the class itself and all other related classes via aggregation/composition and inheritance relationships.

```
// Declare your class and the rules it must comply to.
class A
{
	[StartsWith("11")]
	[EndsWith("11")]
	[ShorterThan(4)]
	[LongerThan(2)]
	[Length(3)]
	[Matches("[0-9]{3}")]
	public int I { get; set; }

	[OneOf("bird", "cat", "dog")]
	public string S { get; set; }
}

// Create an object and fill in whatever values you want.
A a = new A();

// Use namespace Rainsoft.Validations.Attributes to have the IsValid extension method available on your classes.
// Simply call IsValid to determine if your instance satisfies all rules.
bool isValid = a.IsValid();

// Or you can find out exactly which rules are not satisfied by checking the offenses.
IList<ValidationOffense> offenses = new List<ValidationOffense>();
isValid = a.IsValid(ref offenses);
```
This is available in the `Rainsoft.Validations.Attributes` namespace.

# Further Details #
There are validators for all basic needs like the following.
* Starting and ending values.
* Length related.
* Comparisons on primitives or custom IComparable classes.
* Regular expressions.
* DateTime related.
* Set related.
* Predicates that can be used with custom classes.

For more needs or even business specific validations, you can easily extend the library. Additional examples and details can be found in the ValidationsTests project.
