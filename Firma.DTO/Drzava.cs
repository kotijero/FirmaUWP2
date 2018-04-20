using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DTO
{
    public class Drzava
    {
        public Drzava()
        {
            this.Mjesta = new HashSet<Mjesto>();
        }

        public string OznDrzave { get; set; }
        public string NazDrzave { get; set; }
        public string ISO3Drzave { get; set; }
        public Nullable<int> SifDrzave { get; set; }

        public virtual ICollection<Mjesto> Mjesta { get; set; }
    }
}
