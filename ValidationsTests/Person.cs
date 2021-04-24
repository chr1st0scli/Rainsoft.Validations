using System;

namespace ValidationsTests
{
    class Person
    {
        public string Name { get; set; }

        public int TaxNo { get; set; }

        public DateTime BirthDate { get; set; }
    }

    class Employee : Person
    {
        public string Position { get; set; }

        public string Department { get; set; }

        public double Salary { get; set; }

        public HireDetails HireInfo { get; set; }

        public WorkExperience[] FormerEmployers { get; set; }
    }

    class HireDetails
    {
        public DateTime HireDate { get; set; }

        public string HiredBy { get; set; }

        public DateTime? ContractEnd { get; set; }
    }

    class WorkExperience
    {
        public string Employer { get; set; }

        public int Years { get; set; }
    }
}
