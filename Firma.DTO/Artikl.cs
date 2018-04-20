using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DTO
{
    public class Artikl
    {
        public Artikl()
        {
            this.Stavka = new HashSet<Stavka>();
        }
        public int SifArtikla { get; set; }
        public string NazArtikla { get; set; }
        public string JedMjere { get; set; }
        public decimal CijArtikla { get; set; }
        public bool ZastUsluga { get; set; }
        public byte[] SlikaArtikla { get; set; }
        public string TekstArtikla { get; set; }

        public virtual ICollection<Stavka> Stavka { get; set; }
    }
}
