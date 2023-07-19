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
                Action = "AccountType::list",
                Entity = "AccountType",
                Method = SocketMethod.OUTPUT,
                Body = ""
            };

            CommunicationHandler.Instance.Send(message);
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "AccountType::listed")
                {
                    var lines = message.Body.Split("\n");

                    foreach (var line in lines)
                    {
                        if (line.Trim() == "")
                        {
                            continue;
                        }

                        var fields = line.Split(";");
                        var accountType = new AccountType
                        {
                            Id = int.Parse(fields[0]),
                            Name = fields[1],
                            CreatedAt = DateTime.Parse(fields[2])
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
                Action = "AccountType::create",
                Entity = "AccountType",
                Method = SocketMethod.POST,
                Body = $"{accountType.Name}"
            };
            CommunicationHandler.Instance.Send(message);

            var received = false;

            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "AccountType::created")
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

        public void DeleteType(AccountType accountType)
        {
            var message = new SocketMessage
            {
                Action = "AccountType::delete",
                Entity = "AccountType",
                Method = SocketMethod.DELETE,
                Body = $"{accountType.Id}"
            };
            CommunicationHandler.Instance.Send(message);
            var received = false;
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "AccountType::deleted")
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

        public void UpdateType(AccountType accountType)
        {
            var message = new SocketMessage
            {
                Action = "AccountType::update",
                Entity = "AccountType",
                Method = SocketMethod.PUT,
                Body = $"{accountType.Id};{accountType.Name}"
            };
            CommunicationHandler.Instance.Send(message);
            var received = false;
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "AccountType::updated")
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

        public List<Account> getAccounts()
        {
            var list = new List<Account>();
            var received = false;

            var message = new SocketMessage
            {
                Action = "Account::list",
                Entity = "Account",
                Method = SocketMethod.OUTPUT,
                Body = ""
            };

            CommunicationHandler.Instance.Send(message);
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "Account::listed")
                {
                    var lines = message.Body.Split("\n");
                    foreach (var line in lines)
                    {
                        if (line.Trim() == "")
                        {
                            continue;
                        }
                        var fields = line.Split(";");
                        var account = new Account
                        {
                            Id = int.Parse(fields[0]),
                            Name = fields[1],
                            CreatedAt = DateTime.Parse(fields[2]),
                            AccountType = new AccountType
                            {
                                Id = int.Parse(fields[3]),
                                Name = fields[4]
                            }
                        };
                        list.Add(account);
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
            return list;
        }

        public void AddAccount(Account account)
        {
            var message = new SocketMessage
            {
                Action = "Account::create",
                Entity = "Account",
                Method = SocketMethod.POST,
                Body = $"{account.Name};{account.AccountType.Id}"
            };
            CommunicationHandler.Instance.Send(message);
            var received = false;
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "Account::created")
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

        public void DeleteAccount(Account account)
        {
            var message = new SocketMessage
            {
                Action = "Account::delete",
                Entity = "Account",
                Method = SocketMethod.DELETE,
                Body = $"{account.Id}"
            };
            CommunicationHandler.Instance.Send(message);
            var received = false;
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "Account::deleted")
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

        public void UpdateAccount(Account account)
        {
            var message = new SocketMessage
            {
                Action = "Account::update",
                Entity = "Account",
                Method = SocketMethod.PUT,
                Body = $"{account.Id};{account.Name};{account.AccountType.Id}"
            };
            CommunicationHandler.Instance.Send(message);
            var received = false;
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "Account::updated")
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
