using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public abstract class BlBase
    {
        protected string HandleException(Exception exc)
        {
            LogException(exc);
            if (exc is SqlException)
            {
                return $"Došlo je do problema sa bazom podataka, akcija nije izvršena.";
            }
            else if (exc is ArgumentException)
            {
                return $"Krivo uneseni podaci.";
            }
            else
            {
                return $"Dogodila se neočekivana pogreška.";
            }
        }

        private void LogException(Exception exc)
        {
            string errorMessage = string.Empty;
            errorMessage += $"---- EXCEPTION: {exc.GetType()} {Environment.NewLine}";
            errorMessage += $"     Message: {exc.Message} {Environment.NewLine}";
            errorMessage += $"     StackTrace: {exc.StackTrace}";
            Debug.WriteLine(errorMessage);
        }
    }
}
