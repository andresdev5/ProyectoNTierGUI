using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Model
{
    public class AccountingEntryItem
    {
        public int Id { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public TransactionReason TransactionReason { get; set; }
    }

    public class AccountingEntry
    {
        public int Id { get; set; }
        public double CreditSum { get; set; }
        public double DebitSum { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Employee Employee { get; set; }

        public Collection<AccountingEntryItem> Details { get; set; } = new();
    }
}
