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
    public class PayrollService
    {
        public PayrollService() { }

        // generar asiento contable
        public int SaveAccountingEntry(AccountingEntry entry)
        {
            var result = 0;
            var received = false;
            var mapped = entry.Details.Select(detail => $"{detail.TransactionReason.Code},{detail.Credit},{detail.Debit}");
            string detailsEncoded = "[";

            foreach (var detail in mapped)
            {
                detailsEncoded += detail;

                if (mapped.Last() != detail)
                {
                    detailsEncoded += "|";
                }
            }

            detailsEncoded += "]";

            double creditSum = entry.Details.Sum(detail => detail.Credit);
            double debitSum = entry.Details.Sum(detail => detail.Debit);

            entry.CreditSum = creditSum;
            entry.DebitSum = debitSum;

            var fields = new List<string>
            {
                entry.CreditSum.ToString(),
                entry.DebitSum.ToString(),
                entry.Employee.Id.ToString(),
                detailsEncoded
            };

            fields = fields.Select(field => field.Replace(";", "")).ToList();

            var message = new SocketMessage()
            {
                Method = SocketMethod.POST,
                Entity = "AccountingEntry",
                Action = "AccountingEntry::add",
                Body = string.Join(";", fields)
            };

            CommunicationHandler.Instance.Send(message);
            CommunicationHandler.Instance.OnReceive<AccountingService>((message) =>
            {
                if (message.Action == "AccountingEntry::added")
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
    }
}
