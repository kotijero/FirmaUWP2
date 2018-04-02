using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Model
{
    public class Stavka
    {
        public int IdStavke { get; set; }
        public int IdDokumenta { get; set; }
        public int SifArtikla { get; set; }
        public decimal KolArtikla { get; set; }
        public decimal JedCijArtikla { get; set; }
        public decimal PostoRabat { get; set; }

        public virtual Artikl Artikl { get; set; }
        public virtual Dokument Dokument { get; set; }

        public string NazArtikla
        {
            get
            {
                return this.Artikl.NazArtikla;
            }
        }

        public string JedMjere
        {
            get
            {
                return this.Artikl.JedMjere;
            }
        }

        public decimal Ukupno
        {
            get
            {
                return KolArtikla * JedCijArtikla;
            }
        }
    }
}
