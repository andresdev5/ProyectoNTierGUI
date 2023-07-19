using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public AccountType AccountType { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
