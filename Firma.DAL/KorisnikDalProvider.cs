using Firma.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL
{
    public class KorisnikDalProvider
    {
        public Korisnik Fetch(string username)
        {
            string query = "SELECT * FROM Korisnik WHERE Username = @Username";

            DataTable result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@Username", username) });
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                Korisnik korisnik = new Korisnik
                {
                    Username = (string)row[nameof(Korisnik.Username)],
                    Password = (string)row[nameof(Korisnik.Password)],
                    IsAdmin = (bool)row[nameof(Korisnik.IsAdmin)]
                };
                return korisnik;
            }
        }

        public List<Korisnik> FetchAll()
        {
            string query = "SELECT Username, IsAdmin FROM Korisnik";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                List<Korisnik> korisniks = new List<Korisnik>();
                foreach(DataRow row in result.Rows)
                {
                    korisniks.Add(new Korisnik
                    {
                        Username = (string)row[nameof(Korisnik.Username)],
                        IsAdmin = (bool)row[nameof(Korisnik.IsAdmin)]
                    });
                }
                return korisniks;
            }
        }

        public void UpdateItem(Korisnik korisnik, string oldUsername = null)
        {
            string query = @"UPDATE Korisnik SET Password = @Password, IsAdmin = @IsAdmin WHERE Username = @OldUsername";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@OldUsername", string.IsNullOrEmpty(oldUsername) ? korisnik.Username : oldUsername));
            sqlParameters.Add(new SqlParameter("@Username", korisnik.Username));
            sqlParameters.Add(new SqlParameter("@Password", korisnik.Password));
            sqlParameters.Add(new SqlParameter("@IsAdmin", korisnik.IsAdmin));

            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
        }

        public void UpdateItemWithoutPassword(Korisnik korisnik, string oldUsername = null)
        {
            string query = "UPDATE Korisnik SET IsAdmin = @IsAdmin, Username = @Username WHERE Username = @OldUsername";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Username", korisnik.Username));
            sqlParameters.Add(new SqlParameter("@OldUsername", string.IsNullOrEmpty(oldUsername) ? korisnik.Username : oldUsername));
            sqlParameters.Add(new SqlParameter("@IsAdmin", korisnik.IsAdmin));

            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
        }

        public void AddItem(Korisnik korisnik)
        {
            string query = "INSERT INTO Korisnik (Username, Password, IsAdmin) VALUES (@Username, @Password, @IsAdmin)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Username", korisnik.Username));
            sqlParameters.Add(new SqlParameter("@Password", korisnik.Password));
            sqlParameters.Add(new SqlParameter("@IsAdmin", korisnik.IsAdmin));
            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
        }

        public void DeleteItem(Korisnik item)
        {
            string query = "DELETE FROM Korisnik WHERE Username = @Username";
            QueryExecutor.ExecuteNonQuery(query, new List<SqlParameter> { new SqlParameter("@Username", item.Username) });
        }
    }
}
