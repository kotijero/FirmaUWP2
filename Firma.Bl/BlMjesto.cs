using Firma.DAL;
using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public class BlMjesto
    {
        MjestoDalProvider mjestoDalProvider = new MjestoDalProvider();
        public LookupModel FetchSingleLookup(int Id)
        {
            return mjestoDalProvider.FetchSingleLookup(Id);
        }

        public List<LookupModel> FetchLookup()
        {
            return mjestoDalProvider.FetchLookup();
        }
    }
}
