using lamda_practice.Data;
using System;
using System.Linq;

namespace lambda_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            string ciudad = "Chihuahua";
            using (var ctx = new DatabaseContext())
            {
                //1. Listar todos los empleados cuyo departamento tenga una sede en Chihuahua

                var query = ctx.Employees.Where(employee => employee.City.Name == ciudad).Select(employee => new
                {
                    Name = employee.FirstName,
                    LastN = employee.LastName
                });

                foreach (var employee in query)
                {
                    Console.WriteLine("Name: {0},  Last Name: {1}", employee.Name, employee.LastN);

                }

                //2. Listar todos los departamentos y el numero de empleados que pertenezcan a cada departamento.

                var query2 = ctx.Employees.GroupBy(employee => employee.Department.Name)
                    .Select(employee => new
                    {
                        DName = employee.Key,
                        Count = employee.Count()
                    });

                foreach (var deparment in query2)
                {
                    Console.WriteLine("Department: {0},  Count: {1}", deparment.DName, deparment.Count);
                }

                //3. Listar todos los empleados remotos. Estos son los empleados cuya ciudad no se encuentre entre las sedes de su departamento.
                var query3 = ctx.Employees
                             .Where(employee => employee.Department.Cities.Any(sucursal => sucursal.Name == employee.City.Name))
                             .Distinct().Select(sucursal => new
                             {
                                 sucursal.FirstName,
                                 sucursal.LastName

                             });

                foreach (var employee in query3)
                {
                    Console.WriteLine("First Name: {0}, Last Name: {1} ", employee.FirstName, employee.LastName);
                }


                //4. Listar todos los empleados cuyo aniversario de contratación sea el próximo mes.

                var query4 = ctx.Employees.Where(employee => employee.HireDate.Month == 4);

                foreach (var employee in query4)
                {
                    Console.WriteLine("Name: {0} {1} ", employee.FirstName, employee.LastName);
                }

                //Listar los 12 meses del año y el numero de empleados contratados por cada mes.

                var query5 = ctx.Employees.GroupBy(employee => employee.HireDate.Month)
                    .OrderBy(group => group.Key).Select(select => new
                    {
                        month = select.Key,
                        count = select.Count()
                    });

                foreach (var month in query5)
                {
                    Console.WriteLine("Month: {0}, Employees: {1}", month.month, month.count);
                }
            }
            Console.Read();
        }
    }
}