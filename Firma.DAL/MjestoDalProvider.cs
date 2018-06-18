using Firma.DTO;
using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL
{
    public class MjestoDalProvider
    {
        public Mjesto Fetch(int Id)
        {
            string query = "SELECT * FROM Mjesto WHERE Id = @Id";
            DataTable result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@Id", Id) });
            if (result.Rows.Count < 1)
            {
                return null;
            } else
            {
                DataRow row = result.Rows[0];
                Mjesto mjesto = new Mjesto
                {
                    IdMjesta = (int)row[nameof(Mjesto.IdMjesta)],
                    OznDrzave = (string)row[nameof(Mjesto.OznDrzave)],
                    NazMjesta = (string)row[nameof(Mjesto.NazMjesta)],
                    PostBrMjesta = (int)row[nameof(Mjesto.PostBrMjesta)],
                    PostNazMjesta = (string)row[nameof(Mjesto.PostNazMjesta)]
                };
                return mjesto;
            }
        }

        public List<Mjesto> FetchAll()
        {
            string query = "SELECT * FROM Mjesto";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result == null)
            {
                return null;
            } else if (result.Rows.Count < 1)
            {
                return new List<Mjesto>();
            } else
            {
                List<Mjesto> mjestoList = new List<Mjesto>();
                foreach(DataRow row in result.Rows)
                {
                    Mjesto mjesto = new Mjesto
                    {
                        IdMjesta = (int)row[nameof(Mjesto.IdMjesta)],
                        OznDrzave = (string)row[nameof(Mjesto.OznDrzave)],
                        NazMjesta = (string)row[nameof(Mjesto.NazMjesta)],
                        PostBrMjesta = (int)row[nameof(Mjesto.PostBrMjesta)],
                        PostNazMjesta = (string)row[nameof(Mjesto.PostNazMjesta)]
                    };
                    mjestoList.Add(mjesto);
                }
                return mjestoList;
            }
        }

        public Mjesto AddItem(Mjesto item)
        {
            string query = @"INSERT INTO Mjesto (OznDrzave, NazMjesta, PostBrMjesta, PostNazMjesta)
                                   OUTPUT Inserted.IdMjesta
                                    SET (@OznDrzave, @NazMjesta, @PostBrMjesta, @PostNazMjesta)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@OznDrzave", item.OznDrzave),
                new SqlParameter("@NazMjesta", item.NazMjesta),
                new SqlParameter("@PostBrMjesta", item.PostBrMjesta),
                new SqlParameter("@PostNazMjesta", item.PostNazMjesta)
            };
            DataTable res = QueryExecutor.ExecuteQuery(query, sqlParameters);
            if (res != null && res.Rows.Count > 1)
            {
                item.IdMjesta = (int)(res.Rows[0])[nameof(Mjesto.IdMjesta)];
            }
            return item;
        }

        public void UpdateItem(Mjesto item)
        {
            string query = @"UPDATE Mjesto
                                SET OznDrzave = @OznDrzave,
                                    NazMjesta = @NazMjesta,
                                    PostBrMjesta = @PostBrMjesta,
                                    PostNazMjesta = @PostNazMjesta
                              WHERE IdMjesta = @IdMjesta";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@OznDrzave", item.OznDrzave),
                new SqlParameter("@NazMjesta", item.NazMjesta),
                new SqlParameter("@PostBrMjesta", item.PostBrMjesta),
                new SqlParameter("@PostNazMjesta", item.PostNazMjesta),
                new SqlParameter("@IdMjesta", item.IdMjesta)
            };
            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
        }

        public void DeleteItem(Mjesto item)
        {
            string query = "DELETE FROM Mjesto WHERE IdMjesta = @IdMjesta";
            QueryExecutor.ExecuteNonQuery(query, new List<SqlParameter> { new SqlParameter("@IdMjesta", item.IdMjesta) });
            
        }

        public LookupModel FetchSingleLookup(int Id)
        {
            string query = $"SELECT IdMjesta, NazMjesta FROM Mjesto WHERE IdMjesta = @Id";
            DataTable result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@Id", Id) });
            return new LookupModel(Id, (string)result.Rows[0][1]);
        }
        public List<LookupModel> FetchLookup()
        {
            string query = "SELECT IdMjesta, NazMjesta FROM Mjesto";
            DataTable result = QueryExecutor.ExecuteQuery(query);

            List<LookupModel> res = new List<LookupModel>();
            foreach (DataRow row in result.Rows)
            {
                res.Add(new LookupModel((int)row["IdMjesta"], (string)row["NazMjesta"]));
            }
            res.Add(Defaults.MjestoLookup);
            return res;
        }

        public bool CheckMjestoForDrzava(string oznDrzave)
        {
            string query = "SELECT COUNT(*) FROM Mjesto WHERE OznDrzave = @OznDrzave";
            var result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@OznDrzave", oznDrzave) });
            if (result != null)
            {
                return (int)(result.Rows[0])[0] > 0;
            }
            else return false;
        }
    }
}
