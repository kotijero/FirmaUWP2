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
    public class MjestoDalProvider
    {
        public LookupModel FetchSingleLookup(int Id)
        {
            string query = $"SELECT IdMjesta, NazMjesta FROM Mjesto WHERE IdMjesta = {Id}";
            DataTable result = QueryExecutor.ExecuteQuery(query);
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
    }
}
