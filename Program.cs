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

                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error connecting to database:", ex);
            }
       
        }
    }
}