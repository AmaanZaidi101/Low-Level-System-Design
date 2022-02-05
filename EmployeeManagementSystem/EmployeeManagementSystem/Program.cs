 using System;
using System.Collections.Generic;

namespace EmployeeManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee employee = new Employee("Achilles");
            Employee employee1 = new Employee("Hector");
            Employee employee2 = new Employee("Paris");
            Employee employee3 = new Employee("Helen");

            System system = new System();

            system.RegisterEmployee(employee);
            system.RegisterEmployee(employee1);
            system.RegisterEmployee(employee2);
            system.RegisterEmployee(employee3);

            system.RegisterManager(employee1.Id, employee.Id);
            system.RegisterManager(employee2.Id, employee.Id);
            system.RegisterManager(employee3.Id, employee.Id);

            system.PrintDetails(employee1.Id);

            string s = new string('*', 50);
            Console.WriteLine(s);

            system.PrintDetails("he");
            Console.WriteLine(s);

            foreach (var e in system.GetSubordinates(employee.Name))
            {
                Console.WriteLine($"{e.Name} {e.Id}");
            }

            Console.WriteLine(s);

            foreach (var e in system.GetSubordinates(employee.Id))
            {
                Console.WriteLine($"{e.Name} {e.Id}");
            }

            Console.WriteLine(s);

            foreach (var e in system.GetSubordinates(employee1.Id))
            {
                Console.WriteLine($"{e.Name} {e.Id}");
            }
        }
    }

    class Employee
    {
        public static int id = 1;
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Employee> Subordinates { get; set; }
        public int ManagerId { get; set; }

        public Employee(string name)
        {
            Name = name;
            Id = getUniqueId();
            ManagerId = 0;
            Subordinates = new List<Employee>();
        }

        private int getUniqueId()
        {
            return id++;
        }
        public void AddSubordinate(Employee employee)
        {
            Subordinates.Add(employee);
        }

    }

    class System
    {
        public List<Employee> Employees { get; set; }
        public Dictionary<int,Employee> EmployeeMap { get; set; }
        public System()
        {
            Employees = new List<Employee>();
            EmployeeMap = new Dictionary<int, Employee>();
        }
        public void RegisterEmployee(Employee employee)
        {
            Employees.Add(employee);
            EmployeeMap[employee.Id] = employee;
        }
        public void RegisterManager(int employeeId, int managerId)
        {
            if(!EmployeeMap.ContainsKey(employeeId) || !EmployeeMap.ContainsKey(managerId))
            {
                Console.WriteLine("Either employee or manager is not registered");
                return;
            }

            EmployeeMap[employeeId].ManagerId = managerId;
            EmployeeMap[managerId].AddSubordinate(EmployeeMap[employeeId]);
        }
        public void PrintDetails(int employeeId)
        {
            if (!EmployeeMap.ContainsKey(employeeId))
            {
                Console.WriteLine("Employee does not exist");
                return;
            }
            Console.Write($"Id:\t{employeeId}\tName:\t{EmployeeMap[employeeId].Name}\t");
            Console.WriteLine($"Manager:\t{EmployeeMap[EmployeeMap[employeeId].ManagerId].Name}");
        }
        public void PrintDetails(string prefix)
        {
            var emp = Employees.FindAll(x => x.Name.StartsWith(prefix,StringComparison.OrdinalIgnoreCase));
            if (emp == null)
            {
                Console.WriteLine("Could not find employee");
                return;
            }
            foreach (var e in emp)
            {
                Console.Write($"Id:\t{e.Id}\tName:\t{e.Name}\t");
                Console.WriteLine($"Manager:\t{EmployeeMap[e.ManagerId].Name}");
            }
        }
        public List<Employee> GetSubordinates(int managerId)
        {
            if (!EmployeeMap.ContainsKey(managerId))
            {
                Console.WriteLine("Manager does not exist");
                return new List<Employee>();
            }
            return EmployeeMap[managerId].Subordinates;
        }
        public List<Employee> GetSubordinates(string name)
        {
            var m = Employees.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if(m == null)
            {
                return new List<Employee>();
            }
            return m.Subordinates;
        }
    }
}
