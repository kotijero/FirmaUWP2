using Firma.DTO;
using Firma.Helpers;
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
            // TODO: SLIKA!
            string query = String.Format(@"INSERT INTO Artikl (NazArtikla, JedMjere, CijArtikla, ZastUsluga, TekstArtikla)
                                            OUTPUT Inserted.SifArtikla
                                            VALUES ('{0}', '{1}', {2}, {3}, '{4}')",
                                            item.NazArtikla,
                                            item.JedMjere,
                                            item.CijArtikla,
                                            item.ZastUsluga ? 1 : 0,
                                            item.TekstArtikla);
            var result = QueryExecutor.ExecuteQuery(query);
            item.SifArtikla = (int)result.Rows[0].ItemArray[0];
            return item;
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
                                                     item.CijArtikla,
                                                     item.ZastUsluga ? 1 : 0,
                                                     item.TekstArtikla,
                                                     item.SifArtikla);
            QueryExecutor.ExecuteNonQuery(query);
        }

        public string DeleteItem(Artikl item)
        {
            string query = $"SELECT COUNT(*) FROM Stavka WHERE SifArtikla = {item.SifArtikla}";
            var res = QueryExecutor.ExecuteQuery(query);
            if ((int)(res.Rows)[0][0] > 0)
                return "Nije moguće obrisati artikl jer baza sadrži stavke sa ovim artiklom!";

            query = String.Format(@"DELETE FROM Artikl WHERE SifArtikla = {0}", item.SifArtikla);
            if (QueryExecutor.ExecuteNonQuery(query) < 1)
                return "Nije moguće obrisati!";

            return string.Empty;
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
