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
    public class DrzavaDalProvider
    {
        public Drzava Fetch(string oznDrzave)
        {
            string query = "SELECT * FROM Drzava WHERE OznDrzave = @OznDrzave";
            DataTable result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@OznDrzave", oznDrzave) });
            if (result == null || result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                Drzava drzava = new Drzava
                {
                    OznDrzave = (string)row[nameof(Drzava.OznDrzave)],
                    NazDrzave = (string)row[nameof(Drzava.NazDrzave)],
                    ISO3Drzave = (string)row[nameof(Drzava.ISO3Drzave)],
                    SifDrzave = row[nameof(Drzava.SifDrzave)].GetType() == typeof(DBNull) ? default(int?) : (int)row[nameof(Drzava.SifDrzave)]
                };
                return drzava;
            }
        }

        public List<Drzava> FetchAll()
        {
            string query = "SELECT * FROM Drzava";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result == null)
            {
                return null;
            }
            else if(result.Rows.Count < 1)
            {
                return new List<Drzava>();
            }
            else
            {
                List<Drzava> drzavaList = new List<Drzava>();
                foreach(DataRow row in result.Rows)
                {
                    Drzava drzava = new Drzava
                    {
                        OznDrzave = (string)row[nameof(Drzava.OznDrzave)],
                        NazDrzave = (string)row[nameof(Drzava.NazDrzave)],
                        ISO3Drzave = (string)row[nameof(Drzava.ISO3Drzave)],
                        SifDrzave = row[nameof(Drzava.SifDrzave)].GetType() == typeof(DBNull) ? default(int?) : (int)row[nameof(Drzava.SifDrzave)]
                    };
                }
                return drzavaList;
            }
        }

        public void AddItem(Drzava item)
        {
            string query = string.Empty;
            if (item.SifDrzave.HasValue)
            {
                query = @"INSERT INTO Drzava (OznDrzave, NazDrzave, ISO3Drzave, SifDrzave)
                                    VALUES (@OznDrzave, @NazDrzave, @ISO3Drzave, @SifDrzave)";
            } else
            {
                query = @"INSERT INTO Drzava (OznDrzave, NazDrzave, ISO3Drzave)
                                    VALUES (@OznDrzave, @NazDrzave, @ISO3Drzave)";
            }
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@OznDrzave", item.OznDrzave),
                new SqlParameter("@NazDrzave", item.NazDrzave),
                new SqlParameter("@ISO3Drzave", item.ISO3Drzave)
            };
            if (item.SifDrzave.HasValue) sqlParameters.Add(new SqlParameter("@SifDrzave", item.SifDrzave.Value));
            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
        }

        public void UpdateItem(Drzava item, string oldOznDrzave)
        {
            string query = @"UPDATE Drzava
                                SET OznDrzave = @OznDrzave,
                                    NazDrzave = @NazDrzave,
                                    ISO3Drzave = @ISO3Drzave,
                                    SifDrzave = @SifDrzave
                                WHERE OznDrzave = @OldOznDrzave";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@OznDrzave", item.OznDrzave),
                new SqlParameter("@NazDrzave", item.NazDrzave),
                new SqlParameter("@ISO3Drzave", item.ISO3Drzave),
                new SqlParameter("@SifDrzave", item.SifDrzave),
                new SqlParameter("@OldOznDrzave", oldOznDrzave)
            };
            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
        }

        public void DeleteItem(Drzava item)
        {
            string query = "DELETE FROM Drzava WHERE OznDrzave = @OznDrzave";
            QueryExecutor.ExecuteNonQuery(query, new List<SqlParameter> { new SqlParameter("@OznDrzave", item.OznDrzave) });
        }
    }
}
