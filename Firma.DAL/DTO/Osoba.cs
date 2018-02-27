using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL.DTO
{
    public class Osoba : Partner
    {
        public string PrezimeOsobe { get; set; }
        public string ImeOsobe { get; set; }


        public override string Naziv
        {
            get
            {
                return ImeOsobe + " " + PrezimeOsobe;
            }
        }
    }
}
