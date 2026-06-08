using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloCSharp
{
    class Employee
    {
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int Salary { get; set; }
        public int YearsExperience { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee { Name = "Amir", Department = "Engineering", Salary = 55000, YearsExperience = 1 },
                new Employee { Name = "Sara", Department = "Engineering", Salary = 75000, YearsExperience = 5 },
                new Employee { Name = "Johan", Department = "Marketing", Salary = 50000, YearsExperience = 3 },
                new Employee { Name = "Lisa", Department = "Engineering", Salary = 90000, YearsExperience = 8 },
                new Employee { Name = "Ahmed", Department = "Marketing", Salary = 60000, YearsExperience = 4 },
                new Employee { Name = "Emma", Department = "HR", Salary = 48000, YearsExperience = 2 },
            };

            // 1. All engineers
            var engineers = employees.Where(e => e.Department == "Engineering");
            Console.WriteLine("Engineers:");
            foreach (var e in engineers)
                Console.WriteLine($"  {e.Name} - {e.Salary} SEK");

            // 2. Average salary per department
            var avgByDept = employees
                .GroupBy(e => e.Department)
                .Select(g => new { Department = g.Key, AvgSalary = g.Average(e => e.Salary) });

            Console.WriteLine("\nAverage salary by department:");
            foreach (var d in avgByDept)
                Console.WriteLine($"  {d.Department}: {d.AvgSalary:F0} SEK");

            // 3. Top earner
            var topEarner = employees.OrderByDescending(e => e.Salary).First();
            Console.WriteLine($"\nTop earner: {topEarner.Name} ({topEarner.Salary} SEK)");

            // 4. Employees with 3+ years experience, sorted by salary
            var experienced = employees
                .Where(e => e.YearsExperience >= 3)
                .OrderByDescending(e => e.Salary)
                .ToList();

            Console.WriteLine("\nExperienced employees (3+ years), by salary:");
            foreach (var e in experienced)
                Console.WriteLine($"  {e.Name} - {e.YearsExperience} yrs - {e.Salary} SEK");

            var aboveSixtyThousand = employees .Where(e => e.Salary > 60000); 
            Console.WriteLine("Employees with salary above 60k SEK");
            foreach (var e in aboveSixtyThousand)
            {
                Console.WriteLine($" {e.Name} - {e.Department} - {e.Salary}  ");
            }

            var EngineersBudget = employees
                .Where(e => e.Department == "Engineering")
                .Sum(e => e.Salary);
            Console.WriteLine($"Engineering budget: {EngineersBudget} SEK");

            var leastExp = employees.OrderBy(e => e.YearsExperience).First();
            Console.WriteLine("Employees with least years of experience at the bottom");
            Console.WriteLine($" {leastExp.Name} - {leastExp.Department} - {leastExp.YearsExperience}  ");
        }
    }
}