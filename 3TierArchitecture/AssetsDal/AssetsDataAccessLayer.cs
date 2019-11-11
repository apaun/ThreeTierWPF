using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Assets.Model;

namespace Assets.DAL
{
    public class Dal
    {
        private static readonly string ConnectionString;
        private const string GetAllAccounts = @"uspGetAllAccounts";
        private const string AddNewAccount = @"uspAddAccout";
        private const string DeleteAccount = @"uspDeleteAccount";
        private const string UpdateAccountDetails = @"uspUpdateAccountDetails";

        static Dal()
        {
            ConnectionString = $"Data Source={ConfigurationManager.AppSettings["DatabaseSource"]}; Initial Catalog = {ConfigurationManager.AppSettings["DatabaseName"]}; Integrated Security = True;";
        }

        public static IEnumerable<Account> GetAccounts()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State != ConnectionState.Open)
                    throw new Exception("Connection Not Open.");

                var command = new SqlCommand(GetAllAccounts, connection) { CommandType = CommandType.StoredProcedure };
                using (var rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        yield return new Account(rdr["Number"].ToString(), rdr["Bank"].ToString(), rdr["Branch"].ToString(), rdr["Amount"].ToString(), rdr["IfscCode"].ToString());
                    }
                }
            }
        }

        public static void AddAccount(Account account)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(AddNewAccount, connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@number", account.AccountNumber));
                command.Parameters.Add(new SqlParameter("@bank", account.Bank));
                command.Parameters.Add(new SqlParameter("@branch", account.Branch));
                command.Parameters.Add(new SqlParameter("@amount", account.Amount));
                command.Parameters.Add(new SqlParameter("@code", account.IfscCode));
                command.ExecuteNonQuery();
            }
        }

        public static void Delete(Account account)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(DeleteAccount, connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@number", account.AccountNumber));
                command.ExecuteNonQuery();
            }
        }

        public static void Update(Account account)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var command = new SqlCommand(UpdateAccountDetails, conn) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@number", account.AccountNumber));
                command.Parameters.Add(new SqlParameter("@bank", account.Bank));
                command.Parameters.Add(new SqlParameter("@branch", account.Branch));
                command.Parameters.Add(new SqlParameter("@amount", account.Amount));
                command.Parameters.Add(new SqlParameter("@code", account.IfscCode));
                command.ExecuteNonQuery();
            }
        }
    }
}
