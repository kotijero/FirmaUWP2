using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public class ResultWrapper<T> where T : new()
    {
        public ResultWrapper(T value, string errorMessage)
        {
            Value = value;
            ErrorMessage = errorMessage;
        }

        public T Value { get; set; }
        public string ErrorMessage { get; set; }
    }
}
