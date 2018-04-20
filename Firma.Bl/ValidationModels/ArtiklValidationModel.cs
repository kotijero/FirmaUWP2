using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl.ValidationModels
{
    public class ArtiklValidationModel : ObservableModel
    {
        private string sifArtikla;
        private string nazArtikla;
        private string jedMjere;
        private string cijArtikla;
        private string zastUsluga;
        private string tekstArtikla;

        public string SifArtikla
        {
            get { return sifArtikla; }
            set
            {
                if (string.IsNullOrEmpty(sifArtikla))
                    sifArtikla = value;
                else
                    sifArtikla += $"\n{value}";
                OnPropertyChanged();
            }
        }

        public string NazArtikla
        {
            get { return nazArtikla; }
            set
            {
                if (string.IsNullOrEmpty(nazArtikla))
                    nazArtikla = value;
                else
                    nazArtikla += $"\n{value}";
                OnPropertyChanged();
            }
        }

        public string JedMjere
        {
            get { return jedMjere; }
            set
            {
                if (string.IsNullOrEmpty(jedMjere))
                    jedMjere = value;
                else
                    jedMjere += $"\n{value}";
                OnPropertyChanged();
            }
        }

        public string CijArtikla
        {
            get { return cijArtikla; }
            set
            {
                if (string.IsNullOrEmpty(cijArtikla))
                    cijArtikla = value;
                else
                    cijArtikla += $"\n{value}";
                OnPropertyChanged();
            }
        }

        public string ZastUsluga
        {
            get { return zastUsluga; }
            set
            {
                if (string.IsNullOrEmpty(zastUsluga))
                    zastUsluga = value;
                else
                    zastUsluga += $"\n{value}";
                OnPropertyChanged();
            }
        }

        public string TekstArtikla
        {
            get { return tekstArtikla; }
            set
            {
                if (string.IsNullOrEmpty(tekstArtikla))
                    tekstArtikla = value;
                else
                    tekstArtikla += $"\n{value}";
                OnPropertyChanged();
            }
        }
    }
}
