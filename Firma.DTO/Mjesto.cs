using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DTO
{
    public class Mjesto
    {
        public Mjesto()
        {
            this.Partner = new HashSet<Partner>();
            this.Partner1 = new HashSet<Partner>();
        }

        public int IdMjesta { get; set; }
        public string OznDrzave { get; set; }
        public string NazMjesta { get; set; }
        public int PostBrMjesta { get; set; }
        public string PostNazMjesta { get; set; }

        public virtual Drzava Drzava { get; set; }
        public virtual ICollection<Partner> Partner { get; set; }
        public virtual ICollection<Partner> Partner1 { get; set; }
    }
}
