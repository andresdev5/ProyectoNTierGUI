using Microsoft.VisualBasic;
using ProyectoNTierGUI.Core;
using ProyectoNTierGUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Service
{
    public class EmployeeService
    {
        public EmployeeService() { }

        public List<Employee> GetAll()
        {
            List<Employee> result = new();
            var message = new SocketMessage()
            {
                Method = SocketMethod.OUTPUT,
                Entity = "Employee",
                Action = "Employee::list",
                Body = ""
            };

            var received = false;
            CommunicationHandler.Instance.OnReceive<EmployeeService>((message) =>
            {
                if (message.Action == "Employee::listed")
                {
                    var lines = message.Body.Split('\n');

                    foreach (var line in lines)
                    {
                        var fields = line.Split(";");
                        var employee = new Employee()
                        {
                            Id = int.Parse(fields[0]),
                            IdCard = fields[1],
                            FullName = fields[2],
                            HireDate = DateTime.Parse(fields[3]),
                            Salary = Double.Parse(fields[4])
                        };

                        result.Add(employee);
                    }

                    received = true;
                }
            });
            CommunicationHandler.Instance.Send(message);

            var thread = new Thread(() =>
            {
                while (!received) { }
            });

            thread.Start();

            if (!thread.Join(5000))
            {
                received = true;
            }

            return result;
        }

        public int SaveEmployee(Employee employee)
        {
            var result = 0;
            var received = false;
            var fields = new List<string>
            {
                employee.IdCard!,
                employee.FullName!,
                $"{employee.HireDate:yyyy/MM/dd}",
                employee.Salary.ToString()!
            };

            fields = fields.Select(field => field.Replace(";", "")).ToList();

            CommunicationHandler.Instance.Send(new SocketMessage()
            {
                Method = SocketMethod.POST,
                Entity = "Employee",
                Action = "Employee::add",
                Body = string.Join(";", fields)
            });

            CommunicationHandler.Instance.OnReceive<EmployeeService>((message) =>
            {
                if (message.Action == "Employee::added")
                {
                    result = int.Parse(message.Body);
                    received = true;
                }
            });

            var thread = new Thread(() =>
            {
                while (!received) { }
            });

            thread.Start();

            if (!thread.Join(5000))
            {
                received = true;
            }

            return result;
        }

        public void Update(Employee employee)
        {
            var received = false;
            var fields = new List<string>
            {
                employee.Id.ToString()!,
                employee.IdCard!,
                employee.FullName!,
                $"{employee.HireDate:yyyy/MM/dd}",
                employee.Salary.ToString()!
            };

            fields = fields.Select(field => field.Replace(";", "")).ToList();
            
            CommunicationHandler.Instance.Send(new SocketMessage()
            {
                Method = SocketMethod.PUT,
                Entity = "Employee",
                Action = "Employee::update",
                Body = string.Join(";", fields)
            });
            CommunicationHandler.Instance.OnReceive<EmployeeService>((message) =>
            {
                if (message.Action == "Employee::updated")
                {
                    received = true;
                }
            });

            var thread = new Thread(() =>
            {
                while (!received) { }
            });

            thread.Start();

            if (!thread.Join(5000))
            {
                received = true;
            }
        }

        public void Remove(int id)
        {
            var received = false;
            CommunicationHandler.Instance.Send(new SocketMessage()
            {
                Method = SocketMethod.DELETE,
                Entity = "Employee",
                Action = "Employee::delete",
                Body = id.ToString()
            });
            CommunicationHandler.Instance.OnReceive<EmployeeService>((message) =>
            {
                if (message.Action == "Employee::deleted")
                {
                    received = true;
                }
            });
            var thread = new Thread(() =>
            {
                while (!received) { }
            });
            thread.Start();
            if (!thread.Join(5000))
            {
                received = true;
            }
        }
    }
}
