using Firma.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL
{
    public class DokumentDalProvider
    {
        public Dokument Fetch(int Id)
        {
            string query = $"SELECT * FROM Dokument WHERE IdDokumenta = {Id}";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                Dokument dokument = new Dokument
                {
                    IdDokumenta = (int)row[nameof(dokument.IdDokumenta)],
                    VrDokumenta = (string)row[nameof(dokument.VrDokumenta)],
                    BrDokumenta = (int)row[nameof(dokument.BrDokumenta)],
                    DatDokumenta = (DateTime)row[nameof(dokument.DatDokumenta)],
                    IdPartnera = (int)row[nameof(dokument.IdPartnera)],
                    IdPrethDokumenta = (int)row[nameof(dokument.IdPartnera)],
                    PostoPorez = (decimal)row[nameof(dokument.PostoPorez)],
                    IznosDokumenta = (decimal)row[nameof(dokument.PostoPorez)],
                    Stavke = new List<Stavka>()
                };
                
                // preskociti? fetchati samo lookup i ubaciti u model jedan referentni lookup property
                if (dokument.IdPrethDokumenta != null)
                {
                    query = $"SELECT * FROM Dokument WHERE IdDokumenta = {dokument.IdPrethDokumenta.Value}";
                }

                string stavkeQuery = $@"SELECT IdStavke
                                            , IdDokumenta
                                            , Savka.SifArtikla
                                            , KolArtikla
                                            , JedCijArtikla
                                            , PostoRabat
                                            , NazArtikla
                                            , JedMjere
                                         FROM Stavka JOIN Artikl ON Stavka.SifArtikla = Artikl.SifArtkla
                                        WHERE IdDokumenta = {Id}";
                result = QueryExecutor.ExecuteQuery(stavkeQuery);
                foreach(DataRow stRow in result.Rows)
                {
                    Stavka stavka = new Stavka
                    {
                        IdStavke = (int)row[nameof(Stavka.IdStavke)],
                        IdDokumenta = (int)row[nameof(Stavka.IdDokumenta)],
                        SifArtikla = (int)row[nameof(Stavka.SifArtikla)],
                        KolArtikla = (int)row[nameof(Stavka.KolArtikla)],
                        JedCijArtikla = (decimal)row[nameof(Stavka.JedCijArtikla)],
                        PostoRabat = (decimal)row[nameof(Stavka.PostoRabat)],
                        Artikl = new Artikl
                        {
                            SifArtikla = (int)row[nameof(Artikl.SifArtikla)],
                            NazArtikla = (string)row[nameof(Artikl.SifArtikla)],
                            JedMjere = (string)row[nameof(Artikl.SifArtikla)]
                        }
                    };
                    dokument.Stavke.Add(stavka);
                }
                return dokument;
            }
        }

        public List<Dokument> FetchAll()
        {
            string query = "SELECT * FROM Dokumet";
            DataTable documentResult = QueryExecutor.ExecuteQuery(query);
            if (documentResult.Rows.Count < 1)
            {
                return new List<Dokument>();
            }
            else
            {
                // Priprema - stavke:
                query = @"SELECT IdStavke
                               , IdDokumenta
                               , Stavka.SifArtikla
                               , KolArtikla
                               , JedCijArtikla
                               , PostoRabat
                               , NazArtikla
                               , JedMjere
                            FROM Stavka JOIN Artikl ON Stavka.SifArtikla = Artikl.SifArikla";
                DataTable stavkeResult = QueryExecutor.ExecuteQuery(query);
                List<Stavka> stavkaList = new List<Stavka>();
                foreach(DataRow row in stavkeResult.Rows)
                {
                    Stavka stavka = new Stavka
                    {
                        IdStavke = (int)row[nameof(Stavka.IdStavke)],
                        IdDokumenta = (int)row[nameof(Stavka.IdDokumenta)],
                        SifArtikla = (int)row[nameof(Stavka.SifArtikla)],
                        KolArtikla = (decimal)row[nameof(Stavka.KolArtikla)],
                        JedCijArtikla = (decimal)row[nameof(Stavka.JedCijArtikla)],
                        PostoRabat = (decimal)row[nameof(Stavka.PostoRabat)],
                        Artikl = new Artikl
                        {
                            SifArtikla = (int)row[nameof(Artikl.SifArtikla)],
                            NazArtikla = (string)row[nameof(Artikl.NazArtikla)],
                            JedMjere = (string)row[nameof(Artikl.JedMjere)]
                        }
                    };
                }

                // Priprema - Partneri:
                PartnerDalProvider partnerDalProvider = new PartnerDalProvider();
                List<Partner> partnerList = partnerDalProvider.FetchAll();

                // Dokument:
                List<Dokument> dokumentList = new List<Dokument>();
                foreach(DataRow row in documentResult.Rows)
                {
                    Dokument dokument = new Dokument
                    {
                        IdDokumenta = (int)row[nameof(Dokument.IdDokumenta)],
                        VrDokumenta = (string)row[nameof(Dokument.VrDokumenta)],
                        BrDokumenta = (int)row[nameof(Dokument.BrDokumenta)],
                        DatDokumenta = (DateTime)row[nameof(Dokument.DatDokumenta)],
                        IdPartnera = (int)row[nameof(Dokument.IdPartnera)],
                        IdPrethDokumenta = (int)(row[nameof(Dokument.IdPrethDokumenta)] == DBNull.Value ? -1 : row[nameof(Dokument.IdPrethDokumenta)]),
                        PostoPorez = (decimal)row[nameof(Dokument.PostoPorez)],
                        IznosDokumenta = (decimal)row[nameof(Dokument.IznosDokumenta)]
                    };
                    dokument.Stavke = stavkaList.Where(t => t.IdDokumenta == dokument.IdDokumenta).ToList();
                    dokument.Partner = partnerList.Where(t => t.IdPartnera == dokument.IdPartnera).FirstOrDefault();
                    dokumentList.Add(dokument);
                }

                // PrethDokument:
                foreach(Dokument dokument in dokumentList)
                {
                    if (dokument.IdPrethDokumenta != null)
                    {
                        dokument.PrethodniDokument = dokumentList.Where(t => t.IdDokumenta == dokument.IdPrethDokumenta).FirstOrDefault();
                    }
                }
                return dokumentList;
            }
        }

        public void AddItem(Dokument item)
        {
            string query = String.Format(@"INSERT INTO Dokument (VrDokumenta, BrDokumenta, DatDokumenta, IdPartnera, IdPrethDokumenta, PostoPorez, IznosDokumenta)
                                            OUTPUT inserted.IdDokumenta VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                                            item.VrDokumenta,
                                            item.BrDokumenta,
                                            item.DatDokumenta,
                                            item.IdPartnera,
                                            item.IdPrethDokumenta,
                                            item.PostoPorez,
                                            item.IznosDokumenta);
            int result = (int)QueryExecutor.ExecuteQuery(query).Rows[0].ItemArray[0];
            foreach (Stavka stavka in item.Stavke)
            {
                query = String.Format(@"INSERT INTO Stavka (IdDokumenta, SifArtikla, KolArtikla, JedCijArtikla, PostoRabat)
                                                    VALUES ({0}, {1}, {2}, {3}, {4})",
                                                    result, stavka.SifArtikla, stavka.KolArtikla, stavka.JedCijArtikla, stavka.PostoRabat);
                QueryExecutor.ExecuteNonQuery(query);
            }
        }

        public void UpdateItem(Dokument item)
        {
            string query;
            if (item.IdPrethDokumenta != null)
            {
                query = String.Format(@"UPDATE Dokument
                                              SET VrDokumenta = '{0}',
                                                  BrDokumenta = {1},
                                                  DatDokumenta = '{2}',
                                                  IdPartnera = {3},
                                                  IdPrethDokumenta = {4},
                                                  PostoPorez = {5},
                                                  IznosDokumenta = {6}
                                            WHERE IdDokumenta = {7}",
                                            item.VrDokumenta,
                                            item.BrDokumenta,
                                            item.DatDokumenta.ToString(),
                                            item.IdPartnera,
                                            item.IdPrethDokumenta,
                                            item.PostoPorez,
                                            item.IznosDokumenta,
                                            item.IdDokumenta);
                QueryExecutor.ExecuteNonQuery(query);
            }
            else
            {
                query = String.Format(@"UPDATE Dokument
                                              SET VrDokumenta = '{0}',
                                                  BrDokumenta = {1},
                                                  DatDokumenta = '{2}',
                                                  IdPartnera = {3},
                                                  PostoPorez = {4},
                                                  IznosDokumenta = {5}
                                            WHERE IdDokumenta = {6}",
                                            item.VrDokumenta,
                                            item.BrDokumenta,
                                            item.DatDokumenta.ToString(),
                                            item.IdPartnera,
                                            item.PostoPorez,
                                            item.IznosDokumenta,
                                            item.IdDokumenta);
                QueryExecutor.ExecuteNonQuery(query);
            }
            foreach (Stavka stavka in item.Stavke)
            {
                query = String.Format(@"UPDATE Stavka
                                           SET IdDokumenta = {0},
                                               SifArtikla = {1},
                                               KolArtikla = {2},
                                               JedCijArtikla = {3},
                                               PostoRabat = {4}
                                         WHERE IdStavke = {5}",
                                         stavka.IdDokumenta,
                                         stavka.SifArtikla,
                                         stavka.KolArtikla,
                                         stavka.JedCijArtikla,
                                         stavka.PostoRabat,
                                         stavka.IdStavke);
                QueryExecutor.ExecuteNonQuery(query);
            }
        }

        public void DeleteItem(Dokument dokument)
        {
            string query = String.Format("DELETE FROM Stavka WHERE IdDokumenta = {0}", dokument.IdDokumenta);
            QueryExecutor.ExecuteNonQuery(query);
            query = String.Format("DELETE FROM Dokument WHERE IdDokumenta = {0}", dokument.IdDokumenta);
            QueryExecutor.ExecuteNonQuery(query);
        }
    }
}
