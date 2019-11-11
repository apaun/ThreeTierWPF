using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using _3TierArchitecture.Model;

namespace _3TierArchitecture.DAL
{
	public class Dal
	{
		private static readonly string ConnectionString;
		private const string GetAllEmployees = @"sp_GetAllEmployees";
		private const string AddEmployeeData = @"sp_AddEmployee";
		private const string DeleteEmployeeData = @"sp_DeleteEmployee";
		private const string UpdateEmployeeData = @"sp_UpdateEmployee";

        static Dal()
        {
            ConnectionString = $"Data Source={ConfigurationManager.AppSettings["DatabaseSource"]}; Initial Catalog = {ConfigurationManager.AppSettings["DatabaseName"]}; Integrated Security = True;";
        }

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
						yield return new Employee(int.Parse(rdr["Id"].ToString()), rdr["FirstName"].ToString(), rdr["LastName"].ToString());
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
				command.Parameters.Add(new SqlParameter("@id", emp.EmployeeId));
				command.Parameters.Add(new SqlParameter("@firstName", emp.EmployeeFirstName));
				command.Parameters.Add(new SqlParameter("@lastName", emp.EmployeeLastName));
				command.ExecuteNonQuery();
			}
		}

		public static void DeleteEmployee(Employee emp)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				var command = new SqlCommand(DeleteEmployeeData, connection) { CommandType = CommandType.StoredProcedure };
				command.Parameters.Add(new SqlParameter("@id", emp.EmployeeId));
				command.ExecuteNonQuery();
			}
		}

		public static void Update(Employee emp)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var command = new SqlCommand(UpdateEmployeeData, conn) {CommandType = CommandType.StoredProcedure};
				command.Parameters.Add(new SqlParameter("@id", emp.EmployeeId));
				command.Parameters.Add(new SqlParameter("@firstName", emp.EmployeeFirstName));
				command.Parameters.Add(new SqlParameter("@lastName", emp.EmployeeLastName));
				command.ExecuteNonQuery();
			}
		}
	}
}
