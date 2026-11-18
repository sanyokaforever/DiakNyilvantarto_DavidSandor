using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace DiakNyilvantarto_DavidSandor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool otos = false;
            string connectionString = "server=localhost;user=root;password=;database=diak_gyak;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                while (!otos)
                {
                    try
                    {
                        conn.Open();
                        Console.WriteLine("✅ Kapcsolódás sikeres!");
                        string sql = "SELECT * FROM diakok;";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        Console.WriteLine("===== DIÁKNYILVÁNTARTÓ =====");
                        Console.WriteLine("1. Diákok listázása\r\n2. Új diák felvétele\r\n3. Diák adatainak módosítása\r\n4. Diák törlése\r\n5. Kilépés");
                        Console.Write("Választás: ");
                        conn.Close();
                        bool sikeresval = false;
                        string val = Console.ReadLine();
                        while (!sikeresval)
                        {
                            if (!int.TryParse(val?.Trim(), out int number))
                            {
                                Console.WriteLine("Adj meg egy egész számot kérlek.: ");
                                Console.WriteLine("1. Diákok listázása\r\n2. Új diák felvétele\r\n3. Diák adatainak módosítása\r\n4. Diák törlése\r\n5. Kilépés");
                                val = Console.ReadLine();
                            }
                            else if (int.Parse(val.Trim()) > 5 || int.Parse(val.Trim()) < 1)
                            {
                                Console.WriteLine("A szám csak a felsorotak közül legyen.: ");
                                Console.WriteLine("1. Diákok listázása\r\n2. Új diák felvétele\r\n3. Diák adatainak módosítása\r\n4. Diák törlése\r\n5. Kilépés");
                                val = Console.ReadLine();
                            }
                            else
                            {
                                sikeresval = true;
                                break;
                            }

                        }
                        switch (int.Parse(val.Trim()))
                        {
                            case 1:
                                conn.Open();
                                sql = "SELECT * FROM diakok;";
                                cmd = new MySqlCommand(sql, conn);
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["id"]} | {reader["nev"]} | {reader["eletkor"]} | {reader["osztaly"]}");
                                }
                                reader.Close();
                                conn.Close();
                                break;
                            case 2:
                                Console.Write("Név: ");
                                string nev = Console.ReadLine()?.Trim();
                                Console.Write("Életkor: ");
                                string eletkorStr = Console.ReadLine()?.Trim();
                                Console.Write("Osztály: ");
                                string osztaly = Console.ReadLine()?.Trim();
                                if (string.IsNullOrWhiteSpace(nev) || string.IsNullOrWhiteSpace(osztaly))
                                {
                                    Console.WriteLine("A név és az osztály nem lehet üres.");
                                    break;
                                }
                                if (!int.TryParse(eletkorStr, out int eletkor) || eletkor < 1)
                                {
                                    Console.WriteLine("Az életkor csak pozitív egész szám lehet.");
                                    break;
                                }
                                conn.Open();
                                sql = "INSERT INTO diakok (nev, eletkor, osztaly) VALUES (@nev, @eletkor, @osztaly);";
                                cmd = new MySqlCommand(sql, conn);
                                cmd.Parameters.AddWithValue("@nev", nev);
                                cmd.Parameters.AddWithValue("@eletkor", eletkor);
                                cmd.Parameters.AddWithValue("@osztaly", osztaly);
                                int erintett = cmd.ExecuteNonQuery();
                                conn.Close();
                                Console.WriteLine(erintett > 0 ? "Diák sikeresen felvéve!" : "Nem sikerült felvenni a diákot.");
                                break;
                            case 3:
                                conn.Open();
                                sql = "SELECT * FROM diakok;";
                                cmd = new MySqlCommand(sql, conn);
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["id"]} | {reader["nev"]} | {reader["eletkor"]} | {reader["osztaly"]}");
                                }
                                reader.Close();
                                conn.Close();
                                Console.Write("Add meg a módosítandó diák ID-ját: ");
                                if (!int.TryParse(Console.ReadLine()?.Trim(), out int id))
                                {
                                    Console.WriteLine("Az ID csak egész szám lehet.");
                                    break;
                                }
                                Console.Write("Új név (üresen hagyva marad a régi): ");
                                string ujNev = Console.ReadLine()?.Trim();
                                Console.Write("Új életkor (üresen hagyva marad a régi): ");
                                string ujEletkorStr = Console.ReadLine()?.Trim();
                                Console.Write("Új osztály (üresen hagyva marad a régi): ");
                                string ujOsztaly = Console.ReadLine()?.Trim();

                                object ujEletkor = DBNull.Value;
                                if (!string.IsNullOrWhiteSpace(ujEletkorStr))
                                {
                                    if (!int.TryParse(ujEletkorStr, out int parsedEletkor) || parsedEletkor < 1)
                                    {
                                        Console.WriteLine("Az életkor csak pozitív egész szám lehet.");
                                        break;
                                    }
                                    ujEletkor = parsedEletkor;
                                }

                                conn.Open();
                                sql = "UPDATE diakok SET nev = COALESCE(@nev, nev), eletkor = COALESCE(@eletkor, eletkor), osztaly = COALESCE(@osztaly, osztaly) WHERE id = @id;";
                                cmd = new MySqlCommand(sql, conn);
                                cmd.Parameters.AddWithValue("@nev", string.IsNullOrWhiteSpace(ujNev) ? (object)DBNull.Value : ujNev);
                                cmd.Parameters.AddWithValue("@eletkor", ujEletkor);
                                cmd.Parameters.AddWithValue("@osztaly", string.IsNullOrWhiteSpace(ujOsztaly) ? (object)DBNull.Value : ujOsztaly);
                                cmd.Parameters.AddWithValue("@id", id);
                                erintett = cmd.ExecuteNonQuery();
                                conn.Close();
                                Console.WriteLine(erintett > 0 ? "Diák adatai módosítva!" : "Nincs ilyen ID-jú diák.");
                                break;
                            case 4:
                                conn.Open();
                                sql = "SELECT * FROM diakok;";
                                cmd = new MySqlCommand(sql, conn);
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["id"]} | {reader["nev"]} | {reader["eletkor"]} | {reader["osztaly"]}");
                                }
                                reader.Close();
                                conn.Close();
                                Console.Write("Add meg a törlendő diák ID-ját: ");
                                if (!int.TryParse(Console.ReadLine()?.Trim(), out id))
                                {
                                    Console.WriteLine("Az ID csak egész szám lehet.");
                                    break;
                                }
                                conn.Open();
                                sql = "DELETE FROM diakok WHERE id = @id;";
                                cmd = new MySqlCommand(sql, conn);
                                cmd.Parameters.AddWithValue("@id", id);
                                erintett = cmd.ExecuteNonQuery();
                                conn.Close();
                                Console.WriteLine(erintett > 0 ? "Diák törölve!" : "Nincs ilyen ID-jú diák.");
                                break;
                            case 5:
                                Console.WriteLine("Kilépés folyamatban.............");
                                otos = true;
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Hiba: " + ex.Message);
                    }
                }
                Console.ReadKey();
            }
        }
    }
}