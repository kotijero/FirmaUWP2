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
    public class BlPartner
    {
        private PartnerDalProvider partnerDalProvider = new PartnerDalProvider();
        public Partner Fetch(int Id)
        {
            var partnerDto = partnerDalProvider.Fetch(Id);

            MjestoDalProvider mjestoDalProvider = new MjestoDalProvider();
            LookupModel mjestoPartneraLookup = null;
            LookupModel mjestoIsporukeLookup = null;

            if (!partnerDto.IdMjestaPartnera.Equals(-1)) mjestoPartneraLookup = mjestoDalProvider.FetchSingleLookup(partnerDto.IdMjestaPartnera.Value);
            else mjestoPartneraLookup = Defaults.MjestoLookup;

            if (!partnerDto.IdMjestaIsporuke.Equals(-1)) mjestoIsporukeLookup = mjestoDalProvider.FetchSingleLookup(partnerDto.IdMjestaIsporuke.Value);
            else mjestoPartneraLookup = Defaults.MjestoLookup;

            return new Partner(partnerDto, mjestoPartneraLookup, mjestoIsporukeLookup);
        }

        public List<Partner> FetchAll()
        {
            var partnerDtoList = partnerDalProvider.FetchAll();

            MjestoDalProvider mjestoDalProvider = new MjestoDalProvider();
            var mjestoLookupList = mjestoDalProvider.FetchLookup();

            List<Partner> partnerList = new List<Partner>();
            foreach(var dto in partnerDtoList)
            {
                partnerList.Add(new Partner(dto, mjestoLookupList));
            }
            return partnerList;
        }

        public Partner AddItem(Partner item)
        {
            DTO.Partner partnerDto = partnerDalProvider.AddItem(item.ToDTO());
            item.IdPartnera = partnerDto.IdPartnera;
            return item;
        }

        public void UpdateItem(Partner item)
        {
            partnerDalProvider.UpdateItem(item.ToDTO());
        }

        public string DeleteItem(Partner item)
        {
            return partnerDalProvider.DeleteItem(item.ToDTO());
        }

        public List<LookupModel> FetchLookup()
        {
            return partnerDalProvider.FetchLookup();
        }
    }
}
