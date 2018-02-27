using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL
{
    public class QueryExecutor
    {
        public static string ConnectionString
        {
            get
            {
                return "Data Source=DESKTOP-JT1KEIM;Initial Catalog=Firma;User ID=rpppuwp;Password=rpppuwp";
            }
        }

        public static DataTable ExecuteQuery(string queryText)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = queryText;
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable result = new DataTable();
                            adapter.Fill(result);
                            return result;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return null;
            }
        }

        public static int ExecuteNonQuery(string command)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = command;
                            return cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return -1;
            }
        }
    }
}
