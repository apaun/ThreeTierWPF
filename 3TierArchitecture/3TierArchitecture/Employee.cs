using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3TierArchitecture
{
	public class Employee
	{
		public int EmployeeId { get; set; }
		public string EmployeeFirstName { get; set; }
		public string EmployeeLastName { get; set; }

		public Employee(int id, string fName = "", string sName = "")
		{
			EmployeeId = id;
			EmployeeFirstName = fName;
			EmployeeLastName = sName;
		}
	}
}
