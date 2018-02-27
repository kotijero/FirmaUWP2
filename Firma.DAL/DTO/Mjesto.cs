using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL.DTO
{
    public class Mjesto
    {
        public int IdMjesta { get; set; }
        public string OznDrzave { get; set; }
        public string NazMjesta { get; set; }
        public int PostBrMjesta { get; set; }
        public string PostNazMjesta { get; set; }

        public virtual Drzava Drzava { get; set; }
    }
}
