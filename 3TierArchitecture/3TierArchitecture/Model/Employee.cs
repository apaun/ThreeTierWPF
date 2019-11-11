namespace _3TierArchitecture.Model
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
