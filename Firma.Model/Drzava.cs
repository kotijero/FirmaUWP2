using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Model
{
    public class Drzava : ListableModel
    {
        public Drzava()
        {
            Mjesta = new List<Mjesto>();
            oznDrzave = string.Empty;
            nazDrzave = string.Empty;
            iso3Drzave = string.Empty;
        }

        public Drzava (DTO.Drzava drzava)
            :this()
        {
            oznDrzave = drzava.OznDrzave;
            nazDrzave = drzava.NazDrzave;
            iso3Drzave = drzava.ISO3Drzave;
            sifDrzave = drzava.SifDrzave;
        }

        public Drzava (DTO.Drzava drzava, List<DTO.Mjesto> mjesta)
            : this(drzava)
        {
            foreach(var mjesto in mjesta)
            {
                Mjesta.Add(new Mjesto(mjesto));
            }
        }

        private string oznDrzave;
        private string nazDrzave;
        private string iso3Drzave;
        private int? sifDrzave;



        public string OznDrzave
        {
            get { return oznDrzave; }
            set
            {
                if (!oznDrzave.Equals(value))
                {
                    oznDrzave = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NazDrzave
        {
            get { return nazDrzave; }
            set
            {
                if (!nazDrzave.Equals(value))
                {
                    nazDrzave = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ISO3Drzave
        {
            get { return iso3Drzave; }
            set
            {
                if (!iso3Drzave.Equals(value))
                {
                    iso3Drzave = value;
                    OnPropertyChanged();
                }
            }
        }
        public int? SifDrzave
        {
            get { return sifDrzave; }
            set
            {
                if (!sifDrzave.Equals(value))
                {
                    sifDrzave = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Mjesto> Mjesta { get; set; }



        #region IListableModel Implementation
        public override string Title => NazDrzave;

        public override string Subtitle => OznDrzave;

        public override string Subsubtitle => ISO3Drzave;

        #endregion

        public DTO.Drzava ToDTO()
        {
            return new DTO.Drzava
            {
                OznDrzave = OznDrzave,
                NazDrzave = NazDrzave,
                ISO3Drzave = ISO3Drzave,
                SifDrzave = SifDrzave
            };
        }
    }
}
