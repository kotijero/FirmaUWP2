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
    public class PartnerDalProvider
    {
        public Partner Fetch(int Id)
        {
            string query = "SELECT * FROM Partner WHERE Id = " + Id;
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                if (row["TipPartnera"].Equals(Constants.TvrtkaTip))
                {
                    DataRow tvrtkaRow = QueryExecutor.ExecuteQuery("SELECT * FROM Tvrtka WHERE IdTvrtke = " + Id).Rows[0];
                    Tvrtka tvrtka = new Tvrtka
                    {
                        NazivTvrtke = (string)tvrtkaRow["NazivTvrtke"],
                        MatBrTvrtke = (string)tvrtkaRow["MatBrTvrtke"],
                        IdPartnera = (int)row["IdPartnera"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int)row["IdMjestaPartnera"],
                        AdrPartnera = (string)row["AdrPartnera"],
                        IdMjestaIsporuke = (int)row["IdMjestaIsporuke"],
                        AdrIsporuke = (string)row["AdrIsporuke"]
                    };
                    return tvrtka;
                }
                else
                {
                    DataRow osobaRow = QueryExecutor.ExecuteQuery("SELECT * FROM Osoba WHERE IdOsobe = " + Id).Rows[0];
                    Osoba osoba = new Osoba
                    {
                        ImeOsobe = (string)osobaRow["ImeOsobe"],
                        PrezimeOsobe = (string)osobaRow["PrezimeOsobe"],
                        IdPartnera = (int)row["IdPartnera"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int)row["IdMjestaPartnera"],
                        AdrPartnera = (string)row["AdrPartnera"],
                        IdMjestaIsporuke = (int)row["IdMjestaIsporuke"],
                        AdrIsporuke = (string)row["AdrIsporuke"]
                    };
                    return osoba;
                }
            }
        }

        public List<Partner> FetchAll()
        {
            string osobaQuery = @"SELECT PrezimeOsobe, 
		                                 ImeOsobe, 
	                                     IdPartnera, 
	                                     TipPartnera, 
	                                     OIB, 
	                                     IdMjestaPartnera, 
	                                     AdrPartnera, 
	                                     IdMjestaIsporuke, 
	                                     AdrIsporuke, 
	                                     MjIsporuke.OznDrzave AS OznDrzaveIsporuke,
	                                     MjIsporuke.NazMjesta AS NazMjestaIsporuke,
	                                     MjIsporuke.PostBrMjesta AS PostBrMjestaIsporuke,
	                                     MjIsporuke.PostNazMjesta AS PostNazMjestaIsporuke,
	                                     MjSjedista.OznDrzave AS OznDrzaveSjedista,
	                                     MjSjedista.NazMjesta AS NazMjestaSjedista,
	                                     MjSjedista.PostBrMjesta AS PostBrMjestaSjedista,
	                                     MjSjedista.PostNazMjesta AS PostNazMjestaSjedista
		                        FROM Osoba JOIN Partner ON Osoba.IdOsobe = Partner.IdPartnera 
                                LEFT JOIN Mjesto AS MjIsporuke ON  Partner.IdMjestaIsporuke = MjIsporuke.IdMjesta
                                LEFT JOIN Mjesto AS MjSjedista ON Partner.IdMjestaPartnera = MjSjedista.IdMjesta";
            DataTable osobaResult = QueryExecutor.ExecuteQuery(osobaQuery);

            string tvrtkaQuery = @"SELECT NazivTvrtke,
	                                      MatBrTvrtke,
	                                      IdPartnera, 
	                                      TipPartnera, 
	                                      OIB, 
	                                      IdMjestaPartnera, 
	                                      AdrPartnera, 
	                                      IdMjestaIsporuke, 
	                                      AdrIsporuke, 
	                                      MjIsporuke.OznDrzave AS OznDrzaveIsporuke,
	                                      MjIsporuke.NazMjesta AS NazMjestaIsporuke,
	                                      MjIsporuke.PostBrMjesta AS PostBrMjestaIsporuke,
	                                      MjIsporuke.PostNazMjesta AS PostNazMjestaIsporuke,
	                                      MjSjedista.OznDrzave AS OznDrzaveSjedista,
	                                      MjSjedista.NazMjesta AS NazMjestaSjedista,
	                                      MjSjedista.PostBrMjesta AS PostBrMjestaSjedista,
	                                      MjSjedista.PostNazMjesta AS PostNazMjestaSjedista
	                                FROM Tvrtka JOIN Partner ON Tvrtka.IdTvrtke = Partner.IdPartnera
	                                LEFT JOIN Mjesto AS MjIsporuke ON Partner.IdMjestaIsporuke = MjIsporuke.IdMjesta
	                                LEFT JOIN Mjesto AS MjSjedista On Partner.IdMjestaPartnera = MjSjedista.IdMjesta;";
            DataTable tvrtkaResult = QueryExecutor.ExecuteQuery(tvrtkaQuery);


            if (osobaResult.Rows.Count < 1 && tvrtkaResult.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                List<Partner> partnerList = new List<Partner>();

                foreach (DataRow row in tvrtkaResult.Rows)
                {
                    Tvrtka tvrtka = new Tvrtka
                    {
                        IdPartnera = (int)row["IdPartnera"],
                        NazivTvrtke = (string)row["NazivTvrtke"],
                        MatBrTvrtke = (string)row["MatBrTvrtke"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int?)(row["IdMjestaPartnera"] == DBNull.Value ? null : row["IdMjestaPartnera"]),
                        AdrPartnera = (string)(row["AdrPartnera"] == DBNull.Value ? string.Empty : row["AdrPartnera"]),
                        IdMjestaIsporuke = (int?)(row["IdMjestaIsporuke"] == DBNull.Value ? null : row["IdMjestaIsporuke"]),
                        AdrIsporuke = (string)(row["AdrIsporuke"] == DBNull.Value ? string.Empty : row["AdrIsporuke"])
                    };
                    if (tvrtka.IdMjestaIsporuke == null)
                        tvrtka.IdMjestaIsporuke = -1;
                    if (tvrtka.IdMjestaPartnera == null)
                        tvrtka.IdMjestaPartnera = -1;
                    partnerList.Add(tvrtka);
                }
                foreach (DataRow row in osobaResult.Rows)
                {
                    Osoba osoba = new Osoba
                    {

                        ImeOsobe = (string)row["ImeOsobe"],
                        PrezimeOsobe = (string)row["PrezimeOsobe"],
                        IdPartnera = (int)row["IdPartnera"],
                        TipPartnera = (string)row["TipPartnera"],
                        OIB = (string)row["OIB"],
                        IdMjestaPartnera = (int?)(row["IdMjestaPartnera"] == DBNull.Value ? null : row["IdMjestaPartnera"]),
                        AdrPartnera = (string)(row["AdrPartnera"] == DBNull.Value ? string.Empty : row["AdrPartnera"]),
                        IdMjestaIsporuke = (int?)(row["IdMjestaIsporuke"] == DBNull.Value ? null : row["IdMjestaIsporuke"]),
                        AdrIsporuke = (string)(row["AdrIsporuke"] == DBNull.Value ? string.Empty : row["AdrIsporuke"])
                    };
                    if (osoba.IdMjestaIsporuke == null) osoba.IdMjestaIsporuke = -1;
                    if (osoba.IdMjestaPartnera == null) osoba.IdMjestaPartnera = -1;
                    partnerList.Add(osoba);
                }
                return partnerList;
            }
        }

        public List<LookupModel> FetchLookup()
        {
            string query = String.Format(@"SELECT IdPartnera, ImeOsobe, PrezimeOsobe, NazivTvrtke, TipPartnera FROM Partner LEFT JOIN Osoba ON Partner.IdPartnera = Osoba.IdOsobe
                                                                                                               LEFT JOIN Tvrtka ON Partner.IdPartnera = Tvrtka.IdTvrtke");
            DataTable result = QueryExecutor.ExecuteQuery(query);
            List<LookupModel> res = new List<LookupModel>();
            foreach (DataRow row in result.Rows)
            {
                if (row["TipPartnera"].Equals(Constants.TvrtkaTip))
                {
                    res.Add(new LookupModel((int)row["IdPartnera"], (string)row["NazivTvrtke"]));
                }
                else
                {
                    res.Add(new LookupModel((int)row["IdPartnera"], (string)row["ImeOsobe"] + " " + (string)row["PrezimeOsobe"]));
                }
            }
            return res;
        }

        public Partner AddItem(Partner item)
        {
            string query = String.Format(@"INSERT INTO Partner (TipPartnera, OIB, IdMjestaPartnera, AdrPartnera, IdMjestaIsporuke, AdrIsporuke)
                                                OUTPUT Inserted.IdPartnera
                                                VALUES ('{0}', '{1}', {2}, '{3}', {4}, '{5}')",
                                                item.TipPartnera,
                                                item.OIB,
                                                item.IdMjestaIsporuke,
                                                item.AdrPartnera,
                                                item.IdMjestaIsporuke,
                                                item.AdrIsporuke);
            DataTable res = QueryExecutor.ExecuteQuery(query);
            item.IdPartnera = (int)(res.Rows[0])["IdPartnera"];
            if (item.TipPartnera.Equals(Constants.OsobaTip))
            {
                Osoba osoba = (Osoba)item;
                query = $@"INSERT INTO Osoba (ImeOsobe, PrezimeOsobe)
                                VALUES ('{osoba.ImeOsobe}', '{osoba.PrezimeOsobe}')";
                QueryExecutor.ExecuteNonQuery(query);
            }
            else
            {
                Tvrtka tvrtka = (Tvrtka)item;
                query = $@"INSERT INTO Tvrtka (NazivTvrtke, MatBrTvrtke)
                                VALUES ('{tvrtka.NazivTvrtke}', '{tvrtka.MatBrTvrtke}')";
            }
            return item;
        }

        public void UpdateItem(Partner item)
        {
            string query = String.Format(@"UPDATE Partner 
                                              SET TipPartnera = '{1}',
                                                  OIB = {2},
                                                  IdMjestaPartnera = {3},
                                                  AdrPartnera = '{4}',
                                                  IdMjestaIsporuke = {5}, 
                                                  AdrIsporuke = '{6}'
                                                WHERE IdPartnera = {0}",
                                                item.IdPartnera,
                                                item.TipPartnera,
                                                item.OIB,
                                                item.IdMjestaPartnera,
                                                item.AdrPartnera,
                                                item.IdMjestaIsporuke,
                                                item.AdrIsporuke);
            QueryExecutor.ExecuteNonQuery(query);
            if (item.TipPartnera.Equals(Constants.OsobaTip))
            {
                query = $@"UPDATE Osoba
                              SET ImeOsobe = '{((Osoba)item).ImeOsobe}'
                                  PrezimeOsobe '{((Osoba)item).PrezimeOsobe}
                            WHERE IdOsobe = {item.IdPartnera}";
            }
            else
            {
                query = $@"UPDATE Tvrtka
                              SET MatBrTvrtke = '{((Tvrtka)item).MatBrTvrtke}'
                                  NazivTvrtke = '{((Tvrtka)item).MatBrTvrtke}'
                            WHERE IdTvrtke = {item.IdPartnera}";
            }
            QueryExecutor.ExecuteNonQuery(query);
        }

        public string DeleteItem(Partner item)
        {
            string query = $"SELECT COUNT(*) FROM Dokument WHERE IdPartnera = {item.IdPartnera}";
            var res = QueryExecutor.ExecuteQuery(query);
            if ((int)(res.Rows)[0][0] > 0)
            {
                return "Nije moguće obrisati partnera jer baza sadrži dokumente vezane za njega.";
            }

            if (item.TipPartnera.Equals(Constants.OsobaTip))
            {
                query = $"DELETE FROM Osoba WHERE IdOsobe = {item.IdPartnera}";
                if (QueryExecutor.ExecuteNonQuery(query) < 1) return "Nije moguće obrisati.";
            }
            else
            {
                query = $"DELETE FROM Tvrtka WHERE IdTvrtke = {item.IdPartnera}";
                if (QueryExecutor.ExecuteNonQuery(query) < 1) return "Nije moguće obrisati.";
            }

            query = $"DELETE FROM Partner WHERE IdPartnera = {item.IdPartnera}";
            if (QueryExecutor.ExecuteNonQuery(query) < 1) return "Nije moguće obrisati";
            
            return string.Empty;
        }
    }
}
