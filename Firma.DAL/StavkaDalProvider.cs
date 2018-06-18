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
    public class StavkaDalProvider
    {
        public Stavka Fetch(int id)
        {
            string query = "SELECT * FROM Stavka WHERE Id = @Id";
            DataTable result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@Id", id) });
            if (result.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                DataRow row = result.Rows[0];
                Stavka stavka = new Stavka
                {
                    IdStavke = (int)row[nameof(Stavka.IdStavke)],
                    IdDokumenta = (int)row[nameof(Stavka.IdDokumenta)],
                    SifArtikla = (int)row[nameof(Stavka.SifArtikla)],
                    KolArtikla = (decimal)row[nameof(Stavka.KolArtikla)],
                    JedCijArtikla = (decimal)row[nameof(Stavka.JedCijArtikla)],
                    PostoRabat = (decimal)row[nameof(Stavka)]
                };
                return stavka;
            }
        }

        public List<Stavka> FetchAll()
        {
            string query = "SELECT * FROM Stavka";
            DataTable result = QueryExecutor.ExecuteQuery(query);
            if (result.Rows.Count < 1)
            {
                return null;
            } 
            else
            {
                List<Stavka> stavkaList = new List<Stavka>();
                foreach(DataRow row in result.Rows)
                {
                    stavkaList.Add(new Stavka
                    {
                        IdStavke = (int)row[nameof(Stavka.IdStavke)],
                        IdDokumenta = (int)row[nameof(Stavka.IdDokumenta)],
                        SifArtikla = (int)row[nameof(Stavka.SifArtikla)],
                        KolArtikla = (decimal)row[nameof(Stavka.KolArtikla)],
                        JedCijArtikla = (decimal)row[nameof(Stavka.JedCijArtikla)],
                        PostoRabat = (decimal)row[nameof(Stavka.PostoRabat)]
                    });
                }
                return stavkaList;
            }
        }

        public List<Stavka> FetchForDokumentId(int idDokumenta)
        {
            string query = "SELECT * FROM Stavka WHERE IdDokumenta = @IdDokumenta";
            DataTable result = QueryExecutor.ExecuteQuery(query, new List<SqlParameter> { new SqlParameter("@IdDokumenta", idDokumenta) });
            if (result == null)
            {
                return null;
            } else if (result.Rows.Count < 1)
            {
                return new List<Stavka>();
            } else
            {
                List<Stavka> stavkaList = new List<Stavka>();
                foreach(DataRow row in result.Rows)
                {
                    stavkaList.Add(new Stavka
                    {
                        IdStavke = (int)row[nameof(Stavka.IdStavke)],
                        IdDokumenta = (int)row[nameof(Stavka.IdDokumenta)],
                        SifArtikla = (int)row[nameof(Stavka.SifArtikla)],
                        KolArtikla = (decimal)row[nameof(Stavka.KolArtikla)],
                        JedCijArtikla = (decimal)row[nameof(Stavka.JedCijArtikla)],
                        PostoRabat = (decimal)row[nameof(Stavka)]
                    });
                }
                return stavkaList;
            }
        }

        public Stavka AddItem(Stavka stavka)
        {
            string query = @"INSERT INTO Stavka (IdDokumenta, SifArtikla, KolArtikla, JedCijArtikla, PostoRabat)
                                  OUTPUT inserted.IdStavke
                                  VALUES (@IdDokumenta, @SifArtikla, @KolArtikla, @JedCijArtikla, @PostoRabat)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@IdDokumenta", stavka.IdDokumenta),
                new SqlParameter("@SifArtikla", stavka.SifArtikla),
                new SqlParameter("@KolArtikla", stavka.KolArtikla),
                new SqlParameter("@JedCijArtikla", stavka.JedCijArtikla),
                new SqlParameter("@PostoRabat", stavka.PostoRabat)
            };
            DataTable res = QueryExecutor.ExecuteQuery(query, sqlParameters);
            if (res == null) return null;
            else if (res.Rows.Count < 1) return null;
            else
            {
                stavka.IdStavke = (int)(res.Rows[0])[nameof(Stavka.IdStavke)];
                return stavka;
            }
        }

        public void UpdateItem(Stavka stavka)
        {
            string query = @"UPDATE Stavka
                                SET IdDokumenta = @IdDokumenta,
                                    SifArtikla = @SifArtikla,
                                    KolArtikla = @KolArtikla,
                                    JedCijArtikla = @JedCijArtikla,
                                    PostoRabat = @PostoRabat
                              WHERE IdStavke = @IdStavke";
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@IdDokumenta", stavka.IdDokumenta),
                new SqlParameter("@SifArtikla", stavka.SifArtikla),
                new SqlParameter("@KolArtikla", stavka.KolArtikla),
                new SqlParameter("@JedCijArtikla", stavka.JedCijArtikla),
                new SqlParameter("@PostoRabat", stavka.PostoRabat),
                new SqlParameter("@IdStavke", stavka.IdStavke)
            };
            QueryExecutor.ExecuteNonQuery(query, sqlParameters);
        }

        public bool DeleteItem(Stavka stavka)
        {
            string query = "DELETE FROM Stavka WHERE IdStavke = @IdStavke";
            if (QueryExecutor.ExecuteNonQuery(query, new List<SqlParameter> { new SqlParameter("@IdStavke", stavka.IdStavke) }) < 1) return false;
            return true;
        }

        public bool CheckStavkeForSifArtikla(int sifArtikla)
        {
            string query = $"SELECT COUNT(*) FROM Stavka WHERE SifArtikla = {sifArtikla}";
            var res = QueryExecutor.ExecuteQuery(query);
            return (int)(res.Rows)[0][0] > 0;
        }
    }
}
