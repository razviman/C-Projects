using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace Seminar1SGBD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan; // Set the background color
            Console.Clear(); // Clear the console   
            Console.ForegroundColor = ConsoleColor.Black; // Set the text color         
            Console.WriteLine("Hello, World!");
            String connectionString = @"Server=DESKTOP-PB5S12C\SQLEXPRESS;Database=s1;
            Integrated Security = True;TrustServerCertificate=True;"; // Connection string pt baza de date
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    Console.WriteLine("Starea conexiunii: {0}", con.State);
                    con.Open(); // deschidem conexiunea
                    Console.WriteLine("Starea conexiunii: {0}", con.State);
                    // adaugam o comanda de inserare in baza de date
                    SqlCommand insertCommand = new SqlCommand("Insert into Produse(nume, pret, producator) values" +
                        "(@nume1, @pret1, @prod1), (@nume2, @pret2, @prod2);", con);
                    // adaugam parametrii
                    insertCommand.Parameters.AddWithValue("@nume1", "burete");
                    insertCommand.Parameters.AddWithValue("@pret1", 10);
                    insertCommand.Parameters.AddWithValue("@prod1", "Lidl");
                    insertCommand.Parameters.AddWithValue("@nume2", "shaorma");
                    insertCommand.Parameters.AddWithValue("@pret2", 30);
                    insertCommand.Parameters.AddWithValue("@prod2", "KFC");

                    int insertedRows = insertCommand.ExecuteNonQuery(); // executam comanda
                    Console.WriteLine("Au fost adaugate {0} randuri", insertedRows);
                    //citirea datelor
                    Console.WriteLine("Citirea si afisarea datelor din baza de date");
                    SqlCommand selectCommand = new SqlCommand("Select nume, pret, producator from Produse", con);
                    SqlDataReader reader = selectCommand.ExecuteReader(); // executam comanda
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Nume: {0}, Pret: {1}, Producator: {2}",
                                reader.GetString(0), reader.GetDouble(1), reader.GetString(2));
                        }
                    }
                    reader.Close();

                    // actualizarea datelor
                    SqlCommand updateCommand = new SqlCommand("Update Produse set pret=@pretnou where nume =@nume", con);
                    updateCommand.Parameters.AddWithValue("@pretnou", 24.99);
                    updateCommand.Parameters.AddWithValue("@nume", "shaorma");
                    //putem executa metoda ExecuteNonQuery SI FARA a stoca number of row affected
                    // intr o var locala
                    int updatedRows = updateCommand.ExecuteNonQuery();
                    Console.WriteLine("Au fost actualizate {0} randuri", updatedRows);

                    //stergerea datelor

                    SqlCommand deleteCommand = new SqlCommand("Delete from Produse where nume = @nume", con);
                    deleteCommand.Parameters.AddWithValue("@nume", "burete");
                    int deletedRows = deleteCommand.ExecuteNonQuery();
                    Console.WriteLine("Au fost sterse {0} randuri", deletedRows);
                    // citirea si afisarea datelor dupa actualizare si stergere

                    Console.WriteLine("Citirea si afisarea datelor dupa actualizare si stergere");
                    reader = selectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Nume: {0}, Pret: {1}, Producator: {2}",
                                reader.GetString(0), reader.GetDouble(1), reader.GetString(2));
                        }
                    }
                    reader.Close();

                }

            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red; // in caz de eroare fontul se face rosu
                Console.WriteLine("Mesajul exceptiei: {0}", e.Message);
            }
            

        }
    }
}
