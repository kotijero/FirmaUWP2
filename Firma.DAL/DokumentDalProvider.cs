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
                    IdPrethDokumenta = (row[nameof(dokument.IdPrethDokumenta)] is DBNull) ? null : (int?)row[nameof(dokument.IdPrethDokumenta)],
                    PostoPorez = (decimal)row[nameof(dokument.PostoPorez)],
                    IznosDokumenta = (decimal)row[nameof(dokument.IznosDokumenta)]
                };
                return dokument;
            }
        }

        // TODO: Izmjeni u CheckForIdPrethDokumenta(int idPrethDokumenta)
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

        public List<int> FetchAllIds()
        {
            string query = "SELECT IdDokumenta FROM Dokument ORDER BY BrDokumenta";
            var result = QueryExecutor.ExecuteQuery(query);
            List<int> idList = new List<int>();
            foreach(DataRow row in result.Rows)
            {
                idList.Add((int)row["IdDokumenta"]);
            }
            return idList;
        }

        public Dokument AddItem(Dokument item)
        {
            string query = @"INSERT INTO Dokument (VrDokumenta, BrDokumenta, DatDokumenta, IdPartnera, IdPrethDokumenta, PostoPorez, IznosDokumenta)
                                  OUTPUT inserted.IdDokumenta VALUES (@VrDokumenta, @BrDokumenta, @DatDokumenta, @IdPartnera, @IdPrethDokumenta, @PostoPorez, @IznosDokumenta)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@VrDokumenta", item.VrDokumenta),
                new SqlParameter("@BrDokumenta", item.BrDokumenta),
                new SqlParameter("@DatDokumenta", item.DatDokumenta),
                new SqlParameter("@IdPartnera", item.IdPartnera),
                new SqlParameter("@IdPrethDokumenta", (object)item.IdPrethDokumenta ?? DBNull.Value),
                new SqlParameter("@PostoPorez", item.PostoPorez),
                new SqlParameter("@IznosDokumenta", item.IznosDokumenta)
            };
            int result = (int)QueryExecutor.ExecuteQuery(query, sqlParameters).Rows[0].ItemArray[0];
            item.IdDokumenta = result;
            return item;
        }

        public void UpdateItem(Dokument item)
        {
            string query;
            query = @"UPDATE Dokument
                         SET VrDokumenta = @VrDokumenta,
                             BrDokumenta = @BrDokumenta,
                             DatDokumenta = @DatDokumenta,
                             IdPartnera = @IdPartnera,
                             IdPrethDokumenta = @IdPrethDokumenta,
                             PostoPorez = @PostoPorez,
                             IznosDokumenta = @IznosDokumenta
                       WHERE IdDokumenta = @IdDokumenta";
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@VrDokumenta", item.VrDokumenta),
                    new SqlParameter("@BrDokumenta", item.BrDokumenta),
                    new SqlParameter("@DatDokumenta", item.DatDokumenta),
                    new SqlParameter("@IdPartnera", item.IdPartnera),
                    new SqlParameter("@IdPrethDokumenta", (object)item.IdPrethDokumenta ?? DBNull.Value),
                    new SqlParameter("@PostoPorez", item.PostoPorez),
                    new SqlParameter("@IznosDokumenta", item.IznosDokumenta),
                    new SqlParameter("@IdDokumenta", item.IdDokumenta)
                };
            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
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
            var result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@IdMjesta", idMjesta) });
            if (result != null)
            {
                return (int)(result.Rows[0])[0] > 0;
            }
            else return false;
        }

        public bool CheckDokumentForPartner(int idPartnera)
        {
            string query = "SELECT COUNT(*) FROM Dokument WHERE IdPartnera = @IdPartnera";
            var result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@IdPartnera", idPartnera) });
            if (result != null)
            {
                return (int)(result.Rows[0])[0] > 0;
            }
            else return true;
        }
    }
}
