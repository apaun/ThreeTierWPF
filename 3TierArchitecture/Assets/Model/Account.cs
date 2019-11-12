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

        public float Amount { get; set; }

        public string IfscCode { get; set; }

        public AccountTypeEnum TypeEnum { get; set; }

        public Account(string number, string bank, string branch, float amount, string ifscCode, AccountTypeEnum typeEnum)
        {
            AccountNumber = number;
            Bank = bank;
            Branch = branch;
            Amount = amount;
            IfscCode = ifscCode;
            TypeEnum = typeEnum;
        }
    }

    public enum AccountTypeEnum
    {
        SAVINGS,
        PPF,
        RD,
        FD
    }
}
