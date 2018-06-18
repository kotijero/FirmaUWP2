﻿using Firma.DTO;
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
            string query = @"SELECT IdPartnera, ImeOsobe, PrezimeOsobe, NazivTvrtke, TipPartnera 
                               FROM Partner LEFT JOIN Osoba ON Partner.IdPartnera = Osoba.IdOsobe
                                            LEFT JOIN Tvrtka ON Partner.IdPartnera = Tvrtka.IdTvrtke";
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
            string query = @"INSERT INTO Partner (TipPartnera, OIB, IdMjestaPartnera, AdrPartnera, IdMjestaIsporuke, AdrIsporuke)
                                  OUTPUT Inserted.IdPartnera
                                  VALUES (@TipPartnera, @OIB, @IdMjestaPartnera, @AdrPartnera, @IdMjestaIsporuke, @AdrIsporuke)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@TipPartnera", item.TipPartnera),
                new SqlParameter("@OIB", item.OIB),
                new SqlParameter("@IdMjestaPartnera", (object)item.IdMjestaPartnera ?? DBNull.Value),
                new SqlParameter("@AdrPartnera", item.AdrPartnera),
                new SqlParameter("@IdMjestaIsporuke", (object)item.IdMjestaIsporuke ?? DBNull.Value),
                new SqlParameter("@AdrIsporuke", item.AdrIsporuke)
            };
            DataTable res = QueryExecutor.ExecuteQuery(query, sqlParameters);
            item.IdPartnera = (int)(res.Rows[0])["IdPartnera"];
            if (item.TipPartnera.Equals(Constants.OsobaTip))
            {
                Osoba osoba = (Osoba)item;
                List<SqlParameter> osobaParameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdOsobe", item.IdPartnera),
                    new SqlParameter("@ImeOsobe", osoba.ImeOsobe),
                    new SqlParameter("@PrezimeOsobe", osoba.PrezimeOsobe)
                };
                query = @"INSERT INTO Osoba (IdOsobe, ImeOsobe, PrezimeOsobe)
                               VALUES (@IdOsobe, @ImeOsobe, @PrezimeOsobe)";
                QueryExecutor.ExecuteNonQuery(query, osobaParameters);
            }
            else
            {
                Tvrtka tvrtka = (Tvrtka)item;
                List<SqlParameter> tvrtkaParamters = new List<SqlParameter>
                {
                    new SqlParameter("@IdTvrtke", item.IdPartnera),
                    new SqlParameter("@NazivTvrtke", tvrtka.NazivTvrtke),
                    new SqlParameter("@MatBrTvrtke", tvrtka.MatBrTvrtke)
                };
                query = @"INSERT INTO Tvrtka (IdTvrtke, NazivTvrtke, MatBrTvrtke)
                               VALUES (@IdTvrtke, @NazivTvrtke, @MatBrTvrtke)";
                QueryExecutor.ExecuteNonQuery(query, tvrtkaParamters);
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
                Osoba osoba = (Osoba)item;
                List<SqlParameter> osobaParameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdOsobe", item.IdPartnera),
                    new SqlParameter("@ImeOsobe", osoba.ImeOsobe),
                    new SqlParameter("@PrezimeOsobe", osoba.PrezimeOsobe)
                };
                query = $@"UPDATE Osoba
                              SET ImeOsobe = @ImeOsobe
                                  PrezimeOsobe @PrezimeOsobe
                            WHERE IdOsobe = @IdOsobe";
            }
            else
            {
                Tvrtka tvrtka = (Tvrtka)item;
                List<SqlParameter> tvrtkaParamters = new List<SqlParameter>
                {
                    new SqlParameter("@IdTvrtke", item.IdPartnera),
                    new SqlParameter("@NazivTvrtke", tvrtka.NazivTvrtke),
                    new SqlParameter("@MatBrTvrtke", tvrtka.MatBrTvrtke)
                };
                query = $@"UPDATE Tvrtka
                              SET MatBrTvrtke = @MatBrTvrtke
                                  NazivTvrtke = @NazivTvrtke
                            WHERE IdTvrtke = @IdTvrtke";
            }
            QueryExecutor.ExecuteNonQuery(query);
        }

        public bool DeleteItem(Partner item)
        {
            string query = string.Empty;
            if (item.TipPartnera.Equals(Constants.OsobaTip))
            {
                query = $"DELETE FROM Osoba WHERE IdOsobe = {item.IdPartnera}";
                if (QueryExecutor.ExecuteNonQuery(query) < 1) return false;
            }
            else
            {
                query = $"DELETE FROM Tvrtka WHERE IdTvrtke = {item.IdPartnera}";
                if (QueryExecutor.ExecuteNonQuery(query) < 1) return false;
            }

            query = $"DELETE FROM Partner WHERE IdPartnera = {item.IdPartnera}";
            if (QueryExecutor.ExecuteNonQuery(query) < 1) return false;
            
            return true;
        }
    }
}
