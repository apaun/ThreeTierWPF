using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    public class Account
    {
        public string AccountNumber { get; set; }

        public string Bank { get; set; }

        public string Branch { get; set; }

        public int Amount { get; set; }

        public string IfscCode { get; set; }

        public Account(string number, string bank, string branch, string amount, string ifscCode)
        {
            AccountNumber = number;
            Bank = bank;
            Branch = branch;
            Amount = int.Parse(amount);
            IfscCode = ifscCode;
        }
    }
}
