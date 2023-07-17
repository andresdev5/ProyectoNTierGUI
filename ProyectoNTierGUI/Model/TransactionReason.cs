using Lombok.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Model
{
    public partial class TransactionReason : IModel
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Reason { get; set;}
        public double Amount { get; set; }
        public Employee? Employee { get; set; }
        public bool IsChecked { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
