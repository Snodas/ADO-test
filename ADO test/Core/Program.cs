using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace ADO_test.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sakila;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");          

            Console.WriteLine("Enter actor's first name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter actor's last name: ");
            string lastName = Console.ReadLine();

            var query = @"
                SELECT f.title
                FROM Actor a
                    INNER JOIN Film_Actor fa ON a.actor_id = fa.actor_id
                    INNER JOIN Film f ON fa.film_id = f.film_id
                    WHERE a.first_name = @FirstName 
                    AND a.last_name = @LastName";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);

            connection.Open();
           
            var rec = command.ExecuteReader();

            if (rec.HasRows)
            {
                Console.WriteLine($"Movies featuring {firstName} {lastName}:");
                while (rec.Read())
                {
                    Console.WriteLine($"- {rec["title"]}");
                }
            }
            else
            {
                Console.WriteLine("No actor found with the specified name.");
            }

            connection.Close();
        }
    }   
}