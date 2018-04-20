using Firma.DAL;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public class BlArtikl
    {
        ArtiklDalProvider artiklDalProvider = new ArtiklDalProvider();
        public Artikl Fetch(int Id)
        {
            return new Artikl(artiklDalProvider.Fetch(Id));
        }

        public List<Artikl> FetchAll()
        {
            var artiklDtoList = artiklDalProvider.FetchAll();
            List<Artikl> artiklList = new List<Artikl>();
            foreach (var artikl in artiklDtoList)
            {
                artiklList.Add(new Artikl(artikl));
            }
            return artiklList;
        }

        public Artikl AddItem(Artikl item)
        {
            return new Artikl(artiklDalProvider.AddItem(item.ToDTO()));
        }

        public void UpdateItem(Artikl item)
        {
            artiklDalProvider.UpdateItem(item.ToDTO());
        }

        public string DeleteItem(Artikl item)
        {
            return artiklDalProvider.DeleteItem(item.ToDTO());
        }

        public List<LookupModel> FetchLookup()
        {
            return artiklDalProvider.FetchLookup();
        }
    }
}
