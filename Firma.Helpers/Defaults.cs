using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Helpers
{
    public static class Defaults
    {
        public static readonly LookupModel MjestoLookup = new LookupModel(-1, "-Nije odabrano-");
        public static readonly LookupModel ArtiklLookup = new LookupModel(-1, "-Nije odabran-");
        public static readonly LookupModel DokumentLookup = new LookupModel(-1, "-Nije odabran-");
        public static readonly LookupModel PartnerLookup = new LookupModel(-1, "-Nije odabran-");
    }
}
