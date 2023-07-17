using Newtonsoft.Json;
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
    public class TransactionReasonService
    {
        private EmployeeService _employeeService;

        public TransactionReasonService() {
            _employeeService = ContainerProvider.Resolve<EmployeeService>();
        }

        public List<TransactionReason> GetAll()
        {
            List<TransactionReason> result = new();
            var employees = _employeeService.GetAll();
            var message = new SocketMessage()
            {
                Method = SocketMethod.OUTPUT,
                Entity = "TransactionReason",
                Action = "TransactionReason::list",
                Body = "",
            };

            CommunicationHandler.Instance.Send(message);

            var received = false;

            CommunicationHandler.Instance.OnReceive<TransactionReasonService>((message) =>
            {
                if (message.Action == "TransactionReason::listed")
                {
                    var rows = message.Body.Split('\n');

                    foreach (string row in rows)
                    {
                        var columns = row.Split(';');
                        var amount = 0.0d;
                        string employeeCodeStr = columns[4];
                        int employeeCode = int.TryParse(employeeCodeStr, out employeeCode) ? employeeCode : 0;
                        bool isChecked = columns[5] == "1";

                        try { amount = double.Parse(columns[3]); } 
                        catch (Exception) {}

                        TransactionReason transactionReason = new()
                        {
                            Code = columns[0],
                            Type = columns[1],
                            Reason = columns[2],
                            Amount = amount,
                            Employee = employees.Find(e => e.Id == employeeCode),
                            IsChecked = isChecked,
                            CreatedAt = DateTime.Parse(columns[6]),
                        };
                        result.Add(transactionReason);
                    }

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

        public int Add(TransactionReason transactionReason)
        {
            var reason = transactionReason.Reason.Replace(";", "");

            SocketMessage message = new SocketMessage()
            {
                Method = SocketMethod.POST,
                Entity = "TransactionReason",
                Action = "TransactionReason::create",
                Body = $"{transactionReason.Code};{transactionReason.Type};{reason};{transactionReason.Amount};{transactionReason.Employee?.Id};{(transactionReason.IsChecked ? "1" : "0")}",
            };

            var received = false;
            var result = 0;

            CommunicationHandler.Instance.Send(message);
            CommunicationHandler.Instance.OnReceive<TransactionReasonService>((message) =>
            {
                if (message.Action == "TransactionReason::created")
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

        public void Update(TransactionReason transactionReason)
        {
            var reason = transactionReason.Reason.Replace(";", "");
            
            SocketMessage message = new SocketMessage()
            {
                Method = SocketMethod.PUT,
                Entity = "TransactionReason",
                Action = "TransactionReason::update",
                Body = $"{transactionReason.Code};{transactionReason.Type};{reason};{transactionReason.Amount};{transactionReason.Employee?.Id}{(transactionReason.IsChecked ? "1" : "0")}",
            };

            var received = false;
            CommunicationHandler.Instance.Send(message);
            CommunicationHandler.Instance.OnReceive<TransactionReasonService>((message) =>
            {
                if (message.Action == "TransactionReason::updated")
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

        public void Remove(string code)
        {
            SocketMessage message = new SocketMessage()
            {
                Method = SocketMethod.DELETE,
                Entity = "TransactionReason",
                Action = "TransactionReason::delete",
                Body = code,
            };

            var received = false;
            
            CommunicationHandler.Instance.Send(message);
            CommunicationHandler.Instance.OnReceive<TransactionReasonService>((message) =>
            {
                if (message.Action == "TransactionReason::deleted")
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
