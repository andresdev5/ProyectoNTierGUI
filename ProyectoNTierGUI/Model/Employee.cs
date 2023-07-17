using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNTierGUI.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string? IdCard { get; set; }
        public string? FullName { get; set; }
        public DateTime? HireDate { get; set; }
        public double? Salary { get; set; }
    }
}
