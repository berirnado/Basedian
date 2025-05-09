using System;
using System.Reflection;
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
                    Console.WriteLine("Database: {0}", connection.Database);

                    string path = GetProjectDirectory();

                    string dbName = connection.Database;

                    var transaction = connection.BeginTransaction();

                    SqlCommand command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'", connection);

                    command.Transaction = transaction;

                    string dbFolderPath = path + "/" + dbName;
                    Directory.CreateDirectory(dbFolderPath);

                    // Loops each table in database 
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string filePath = dbFolderPath + "/" + reader[0] + ".txt";
                            if (!File.Exists(filePath))
                            {
                                using (StreamWriter sw = File.CreateText(filePath))
                                {
                                    sw.WriteLine(reader[0]);
                                }
                                Console.WriteLine(String.Format("Created text file: {0}", reader[0] + ".txt"));
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error connecting to database:", ex);
            }
       
        }

        static string GetProjectDirectory()
        {
            string baseDir = AppContext.BaseDirectory;
            var directory = new DirectoryInfo(baseDir);

            while (directory != null && !directory.GetFiles("*.csproj").Any())
            {
                directory = directory.Parent;
            }

            return directory?.FullName ?? baseDir;
        }
    }
}