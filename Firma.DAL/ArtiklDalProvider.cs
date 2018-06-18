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
    public class ArtiklDalProvider
    {
        public Artikl Fetch(int Id)
        {
            string query = "SELECT * FROM Artikl WHERE Id = @Id";
            
            DataTable result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@Id", Id) });
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                Artikl artikl = new Artikl
                {
                    SifArtikla = (int)row["SifArtikla"],
                    NazArtikla = (string)row["NazArtikla"],
                    JedMjere = (string)row["JedMjere"],
                    CijArtikla = (decimal)row["CijArtikla"],
                    ZastUsluga = (bool)row["ZastUsluga"],
                    SlikaArtikla = (byte[])row["SlikaArtikla"],
                    TekstArtikla = (string)row["TekstArtikla"]
                };
                return artikl;
            }
        }

        public List<Artikl> FetchAll()
        {
            string query = "SELECT * FROM Artikl";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                List<Artikl> artiklList = new List<Artikl>();

                foreach (DataRow row in result.Rows)
                {
                    Artikl artikl = new Artikl
                    {
                        SifArtikla = row["SifArtikla"].GetType() == typeof(DBNull) ? default(int) : (int)row["SifArtikla"],
                        NazArtikla = row["NazArtikla"].GetType() == typeof(DBNull) ? string.Empty : (string)row["NazArtikla"],
                        JedMjere = row["JedMjere"].GetType() == typeof(DBNull) ? string.Empty : (string)row["JedMjere"],
                        CijArtikla = row["CijArtikla"].GetType() == typeof(DBNull) ? default(decimal) : (decimal)row["CijArtikla"],
                        ZastUsluga = row["ZastUsluga"].GetType() == typeof(DBNull) ? default(bool) : (bool)row["ZastUsluga"],
                        SlikaArtikla = row["SlikaArtikla"].GetType() == typeof(DBNull) ? default(byte[]) : (byte[])row["SlikaArtikla"],
                        TekstArtikla = row["TekstArtikla"].GetType() == typeof(DBNull) ? string.Empty : (string)row["TekstArtikla"]
                    };
                    artiklList.Add(artikl);
                }
                return artiklList;
            }
        }

        public int ItemsCount()
        {
            string query = @"SELECT COUNT(*) FROM Artikl";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            return (int)result.Rows[0].ItemArray[0];
        }

        public Artikl AddItem(Artikl item)
        {
            string query = @"INSERT INTO Artikl (NazArtikla, JedMjere, CijArtikla, ZastUsluga, SlikaArtikla, TekstArtikla)
                                  OUTPUT Inserted.SifArtikla
                                  VALUES (@NazArtikla, @JedMjere, @CijArtikla, @ZastUsluga, @SlikaArtikla, @TekstArtikla)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@NazArtikla", item.NazArtikla),
                new SqlParameter("@JedMjere", item.JedMjere),
                new SqlParameter("@CijArtikla", item.CijArtikla),
                new SqlParameter("@ZastUsluga", item.ZastUsluga),
                new SqlParameter("@SlikaArtikla", SqlDbType.VarBinary)
                {
                    Direction = ParameterDirection.Input,
                    Size = item.SlikaArtikla.Length,
                    Value = item.SlikaArtikla
                },
                new SqlParameter("@TekstArtikla", item.TekstArtikla)
            };
            var result = QueryExecutor.ExecuteQuery(query, sqlParameters);
            item.SifArtikla = (int)result.Rows[0].ItemArray[0];
            return item;
        }

        public Artikl UpdateItem(Artikl item)
        {
            string query = @"UPDATE Artikl
                                SET NazArtikla = @NazArtikla,
                                    JedMjere = @JedMjere,
                                    CijArtikla = @CijArtikla,
                                    ZastUsluga = @ZastUsluga,
                                    SlikaArtikla = @SlikaArtikla,
                                    TekstArtikla = @TekstArtikla
                              WHERE SifArtikla = @SifArtikla";

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            sqlParameters.Add(new SqlParameter("@NazArtikla", item.NazArtikla));
            sqlParameters.Add(new SqlParameter("@JedMjere", item.JedMjere));
            sqlParameters.Add(new SqlParameter("@CijArtikla", item.CijArtikla));
            sqlParameters.Add(new SqlParameter("@ZastUsluga", item.ZastUsluga));
            sqlParameters.Add(new SqlParameter("@SlikaArtikla", SqlDbType.VarBinary)
            {
                Direction = ParameterDirection.Input,
                Size = item.SlikaArtikla.Length,
                Value = item.SlikaArtikla
            });
            sqlParameters.Add(new SqlParameter("@TekstArtikla", item.TekstArtikla));
            sqlParameters.Add(new SqlParameter("@SifArtikla", item.SifArtikla));

            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
            return item;
        }

        public bool DeleteItem(Artikl item)
        {
            string query = String.Format(@"DELETE FROM Artikl WHERE SifArtikla = {0}", item.SifArtikla);
            if (QueryExecutor.ExecuteNonQuery(query) < 1)
                return false;

            return true;
        }

        public List<LookupModel> FetchLookup()
        {
            string query = @"SELECT SifArtikla, NazArtikla FROM Artikl";
            var result = QueryExecutor.ExecuteQuery(query);
            List<LookupModel> lookupList = new List<LookupModel>();
            foreach (DataRow row in result.Rows)
            {
                lookupList.Add(new LookupModel((int)row["SifArtikla"], (string)row["NazArtikla"]));
            }

            return lookupList;
        }
    }
}
