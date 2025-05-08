using System;
using Microsoft.Data.SqlClient;


namespace Basedian
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Welcome to the Basedian converter --");
            Console.WriteLine("-- Database connection string: --");
            string? connectionString = Console.ReadLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.ConnectionString = connectionString;

                    connection.Open();

                    Console.WriteLine("State: {0}", connection.State);
                    Console.WriteLine("ConnectionString: {0}", connection.ConnectionString);

                    var transaction = connection.BeginTransaction();

                    SqlCommand command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'", connection);

                    command.Transaction = transaction;  

                    // Loops each table in database 
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(String.Format("{0}",
                                reader[0]));
                            Thread.Sleep(20);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error connecting to database:", ex);
            }
       
        }
    }
}