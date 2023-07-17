using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Service
{
    using ProyectoNTierGUI.Core;
    using ProyectoNTierGUI.Model;
    using System.Reflection;
    using System.Threading;

    public class AccountingService
    {
        public List<AccountType> getTypes()
        {
            List<AccountType> result = new();
            var received = false;
            var message = new SocketMessage
            {
                Action = "Accounting::AccountType::list",
                Entity = "AccountType",
                Method = SocketMethod.OUTPUT,
                Body = ""
            };

            CommunicationHandler.Instance.Send(message);
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "Accounting::AccountType::list")
                {
                    var lines = message.Body.Split("\n");

                    foreach (var line in lines)
                    {
                        var fields = line.Split(";");
                        var accountType = new AccountType
                        {
                            Id = int.Parse(fields[0]),
                            Name = fields[1]
                        };
                        result.Add(accountType);
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

        public void AddType(AccountType accountType)
        {
            var message = new SocketMessage
            {
                Action = "Accounting::AccountType::create",
                Entity = "AccountType",
                Method = SocketMethod.POST,
                Body = $"{accountType.Name}"
            };
            CommunicationHandler.Instance.Send(message);

            var received = false;

            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "Accounting::AccountType::created")
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
