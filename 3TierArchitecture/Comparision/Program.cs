using System;
using System.Collections.Generic;
using System.Reflection;

namespace Comparision
{
    class Program
    {
        static void Main(string[] args)
        {
            var carList = new List<Car>
            {
                new Car() {Name = "Car 3", MaxSpeed = 10}, new Car() {Name = "Car 1", MaxSpeed = 10}, new Car() {Name = "Car 2", MaxSpeed = 20}
            };
            Console.WriteLine("Before Sort");
            carList.ForEach((x) => Console.WriteLine(x.Name));
            carList.Sort();
            Console.WriteLine("After Sort");
            carList.ForEach((x)=> Console.WriteLine(x.Name));

            carList.Sort(new CarComparer().Compare);
            Console.WriteLine("After Sort using car comparer");
            carList.ForEach((x) => Console.WriteLine(x.Name));
        }
    }

    public class Car : IComparable, IEquatable<Car>
    {
        public string Name { get; set; }

        public int MaxSpeed { get; set; }


        public int CompareTo(object obj)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod() + " called.");
            var car = obj as Car;
            if(car == null)
                throw new ArgumentException("Invalid object");

            return Name.CompareTo(car.Name);
        }

        public bool Equals(Car other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && MaxSpeed == other.MaxSpeed;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Car) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ MaxSpeed;
            }
        }
    }

    public class CarComparer : IComparer<Car>
    {
        public int Compare(Car x, Car y)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod() + " called");
            return y.Name.CompareTo(x.Name);
        }
    }

    public class CarEqualityComparer : IEqualityComparer<Car>
    {
        public bool Equals(Car x, Car y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(Car obj)
        {
            throw new NotImplementedException();
        }
    }

}
