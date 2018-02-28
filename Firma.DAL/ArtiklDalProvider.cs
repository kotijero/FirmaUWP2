using Firma.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL
{
    public class ArtiklDalProvider
    {
        public Artikl Fetch(int Id)
        {
            string query = "SELECT * FROM Artikl WHERE Id = " + Id;
            DataTable result = QueryExecutor.ExecuteQuery(query);
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
                    CijArtkila = (decimal)row["CijArtikla"],
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
                        CijArtkila = row["CijArtikla"].GetType() == typeof(DBNull) ? default(decimal) : (decimal)row["CijArtikla"],
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

        public void AddItem(Artikl item)
        {
            // TODO: SLIKA!
            string query = String.Format(@"INSERT INTO Artikl (NazArtikla, JedMjere, CijArtikla, ZastUsluga, TekstArtikla)
                                            VALUES ('{0}', '{1}', {2}, {3}, '{4}')",
                                            item.NazArtikla,
                                            item.JedMjere,
                                            item.CijArtkila,
                                            item.ZastUsluga ? 1 : 0,
                                            item.TekstArtikla);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public void UpdateItem(Artikl item)
        {
            string query = String.Format(@"UPDATE Artikl
                                              SET NazArtikla = '{0}',
                                                  JedMjere = '{1}',
                                                  CijArtikla = {2},
                                                  ZastUsluga = {3},
                                                  TekstArtikla = '{4}'
                                            WHERE SifArtikla = {5}",
                                                     item.NazArtikla,
                                                     item.JedMjere,
                                                     item.CijArtkila,
                                                     item.ZastUsluga ? 1 : 0,
                                                     item.TekstArtikla,
                                                     item.SifArtikla);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public void DeleteItem(Artikl item)
        {
            string query = String.Format(@"DELETE FROM Artikl WHERE SifArtikla = {0}", item.SifArtikla);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public Dictionary<int, string> FetchLookup()
        {
            string query = @"SELECT SifArtikla, NazArtikla FROM Artikl";
            var result = QueryExecutor.ExecuteQuery(query);
            Dictionary<int, string> dict = new Dictionary<int, string>();
            foreach (DataRow row in result.Rows)
            {
                dict.Add((int)row["SifArtikla"], (string)row["NazArtikla"]);
            }
            return dict;
        }
    }
}
