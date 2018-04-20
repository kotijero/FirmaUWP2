using Firma.DAL;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public class BlDokument
    {
        DokumentDalProvider dokumentDalProvider = new DokumentDalProvider();

        public Dokument Fetch(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Dokument> FetchAll()
        {
            List<Dokument> dokumentList = new List<Dokument>();
            foreach(var dokument in dokumentDalProvider.FetchAll())
            {
                dokumentList.Add(new Dokument(dokument));
            }
            return dokumentList.OrderBy(t => t.BrDokumenta).ToList();
        }

        public List<Dokument> FetchAll(List<Artikl> artiklList)
        {
            List<Dokument> dokumentList = new List<Dokument>();
            foreach (var dokument in dokumentDalProvider.FetchAll())
            {
                dokumentList.Add(new Dokument(dokument, artiklList));
            }
            return dokumentList.OrderBy(t => t.BrDokumenta).ToList();
        }

        public Dokument AddItem(Dokument item)
        {
            DTO.Dokument dokumentDto = dokumentDalProvider.AddItem(item.ToDTO());
            item.IdDokumenta = dokumentDto.IdDokumenta;
            foreach(var stavka in item.Stavke)
            {
                stavka.IdDokumenta = item.IdDokumenta;
                stavka.IdStavke = dokumentDto.Stavke.FirstOrDefault(t => t.SifArtikla.Equals(stavka.SifArtikla)).IdStavke;
            }

            return item;
        }

        public void UpdateItem(Dokument item)
        {
            dokumentDalProvider.UpdateItem(item.ToDTO());
        }

        public string DeleteItem(Dokument item)
        {
            return dokumentDalProvider.DeleteItem(item.ToDTO());
        }
    }
}
