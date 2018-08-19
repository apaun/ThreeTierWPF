using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace _3TierArchitecture
{
	public class Dal
	{

		private const string ConnectionString = @"Data Source=localhost\HISTORIAN; Initial Catalog = TestDb; Integrated Security = True;";
		private const string GetAllEmployees = @"sp_GetAllEmployees";
		private const string AddEmployeeData = @"sp_AddEmployee";
		private const string DeleteEmployeeData = @"sp_DeleteEmployee";
		private const string UpdateEmployeeData = @"sp_UpdateEmployee";

		public static IEnumerable<Employee> GetEmployees()
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				if (connection.State != ConnectionState.Open)
					throw new Exception("Connection Not Open.");

				var command = new SqlCommand(GetAllEmployees, connection) { CommandType = CommandType.StoredProcedure };
				//cmd.Parameters.Add(new SqlParameter("@CustomerID", custId));
				using (var rdr = command.ExecuteReader())
				{
					while (rdr.Read())
					{
						yield return new Employee(int.Parse(rdr["EmployeeId"].ToString()), rdr["EmployeeFirstName"].ToString(), rdr["EmployeeLastName"].ToString());
					}
				}
			}
		}

		public static void AddEmployee(Employee emp)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				var command = new SqlCommand(AddEmployeeData, connection) { CommandType = CommandType.StoredProcedure };
				command.Parameters.Add(new SqlParameter("@EmployeeId", emp.EmployeeId));
				command.Parameters.Add(new SqlParameter("@EmployeeFirstName", emp.EmployeeFirstName));
				command.Parameters.Add(new SqlParameter("@EmployeeLastName", emp.EmployeeLastName));
				command.ExecuteNonQuery();
			}
		}

		public static void DeleteEmployee(Employee emp)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				var command = new SqlCommand(DeleteEmployeeData, connection) { CommandType = CommandType.StoredProcedure };
				command.Parameters.Add(new SqlParameter("@EmployeeId", emp.EmployeeId));
				command.ExecuteNonQuery();
			}
		}

		public static void Update(Employee emp)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var command = new SqlCommand(UpdateEmployeeData, conn) {CommandType = CommandType.StoredProcedure};
				command.Parameters.Add(new SqlParameter("@EmployeeId", emp.EmployeeId));
				command.Parameters.Add(new SqlParameter("@EmployeeFirstName", emp.EmployeeFirstName));
				command.Parameters.Add(new SqlParameter("@EmployeeLastName", emp.EmployeeLastName));
				command.ExecuteNonQuery();
			}
		}
	}
}
