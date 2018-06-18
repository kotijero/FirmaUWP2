using Firma.DTO;
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
                    IznosDokumenta = (decimal)row[nameof(dokument.PostoPorez)]
                };
                return dokument;
            }
        }

        public List<Dokument> FetchDokumentsWithIdPrethDokumenta(int idPrethDokumenta)
        {
            string query = $"SELECT * FROM Dokument WHERE IdPrethDokumenta = {idPrethDokumenta}";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                List<Dokument> dokumentList = new List<Dokument>();
                foreach(DataRow row in result.Rows)
                {
                    dokumentList.Add(new Dokument
                    {
                        IdDokumenta = (int)row[nameof(Dokument.IdDokumenta)],
                        VrDokumenta = (string)row[nameof(Dokument.VrDokumenta)],
                        BrDokumenta = (int)row[nameof(Dokument.BrDokumenta)],
                        DatDokumenta = (DateTime)row[nameof(Dokument.DatDokumenta)],
                        IdPartnera = (int)row[nameof(Dokument.IdPartnera)],
                        IdPrethDokumenta = (int)row[nameof(Dokument.IdPartnera)],
                        PostoPorez = (decimal)row[nameof(Dokument.PostoPorez)],
                        IznosDokumenta = (decimal)row[nameof(Dokument.PostoPorez)]
                    });
                }
                
                return dokumentList;
            }
        }

        public List<Dokument> FetchAll()
        {
            string query = "SELECT * FROM Dokument";
            DataTable documentResult = QueryExecutor.ExecuteQuery(query);
            if (documentResult.Rows.Count < 1)
            {
                return new List<Dokument>();
            }
            else
            {
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
                    dokumentList.Add(dokument);
                }
                return dokumentList;
            }
        }

        public Dokument AddItem(Dokument item)
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
            item.IdDokumenta = result;
            return item;
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
        }

        public string DeleteItem(Dokument dokument)
        {
            string query = String.Format("DELETE FROM Dokument WHERE IdDokumenta = {0}", dokument.IdDokumenta);
            QueryExecutor.ExecuteNonQuery(query);

            return string.Empty;
        }

        public bool CheckDokumentForMjesto(int idMjesta)
        {
            string query = "SELECT COUNT(*) FROM Dokument WHERE IdMjestaPartnera = @IdMjesta OR IdMjestaIsporuke = @IdMjesta";
            var result = QueryExecutor.ExecuteQuery(query, new List<System.Data.SqlClient.SqlParameter> { new System.Data.SqlClient.SqlParameter("@IdMjesta", idMjesta) });
            if (result != null)
            {
                return (int)(result.Rows[0])[0] > 0;
            }
            else return false;
        }

        public bool CheckDokumentForPartner(int idPartnera)
        {
            string query = "SELECT COUNT(*) FROM Dokument WHERE IdPartnera = @IdPartnera";
            var result = QueryExecutor.ExecuteQuery(query, new List<System.Data.SqlClient.SqlParameter> { new System.Data.SqlClient.SqlParameter("@IdPartnera", idPartnera) });
            if (result != null)
            {
                return (int)(result.Rows[0])[0] > 0;
            }
            else return true;
        }
    }
}
