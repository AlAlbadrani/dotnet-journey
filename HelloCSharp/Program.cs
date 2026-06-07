using System;

namespace HelloCSharp
{
    class Person
    {
        // Properties - cleaner than C++ getters/setters
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }

        // Constructor
        public Person(string name, int age, string city)
        {
            Name = name;
            Age = age;
            City = city;
        }

        // Method
        public string Introduce(Car? car)
        {
            return $"Hi, I'm {Name}, {Age} years old, living in {City}. Owns {car}";
        }
    }

    interface IVehicle
        {
            string Make {get;}
            string Model {get;}
            int Year {get;}
            string Describe();
        }

    abstract class Vehicle : IVehicle
    {
        public string Make {get; set;} = string.Empty;
        public string Model {get; set;} = string.Empty;
        public int Year {get; set;}

        public string Describe()
        {
            return $"{Make} {Model} {Year}"; 
        }
    }

    class Car : Vehicle
    {
        public bool IsElectric { get; set;}

        public Car (string make, string model, int year, bool isElectric)
        {
            Make = make;
            Model = model;
            Year = year;
            IsElectric = isElectric;
        }

        public override string ToString()
        {
            string carType = IsElectric ? "Electric" : "Petrol"; 
            return $"{Make} {Model} {Year} ({carType})";
        }

    }

    class Motorcycle : Vehicle
    {
        public Motorcycle (string make, string model, int year)
            {
                Make = make;
                Model = model;
                Year = year;
            }
    }
    class Program
    {
        static void Main(string[] args)
        {   
            Person amir = new Person("Amir", 29, "Malmö");
            Car prius = new Car ("Toyota", "Prius", 2008, false);
            Console.WriteLine(amir.Introduce(prius));

            // Object initializer syntax - alternative way
            Person someone = new Person("Sara", 25, "Stockholm");
            Car a3 = new Car( "Audi", "A3", 2014, false);
            Console.WriteLine(someone.Introduce(a3));
            
            Motorcycle mt07 = new Motorcycle("Yamaha", "MT07", 2019); 
            List<IVehicle> vehicles = [prius, a3, mt07];

            foreach (IVehicle vehicle in vehicles)
            {
                Console.WriteLine(vehicle.Describe());
            }
        }
    }

    
}