using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl.ValidationModels
{
    public class ArtiklValidationModel : ValidationModelBase
    {
        public ArtiklValidationModel()
            : base()
        {
        }

        public override void ClearErrors(bool notify)
        {
            sifArtikla = string.Empty;
            nazArtikla = string.Empty;
            jedMjere = string.Empty;
            cijArtikla = string.Empty;
            zastUsluga = string.Empty;
            tekstArtikla = string.Empty;
        }

        public void ClearProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(SifArtikla):
                    sifArtikla = string.Empty;
                    break;
                case nameof(NazArtikla):
                    nazArtikla = string.Empty;
                    break;
                case nameof(JedMjere):
                    jedMjere = string.Empty;
                    break;
                case nameof(CijArtikla):
                    cijArtikla = string.Empty;
                    break;
                case nameof(ZastUsluga):
                    zastUsluga = string.Empty;
                    break;
                case nameof(TekstArtikla):
                    tekstArtikla = string.Empty;
                    break;
                default:    // Ovaj default postoji za slucaj da se pozove
                    return; // za nepostojeci property da se ne poziva OnPropertyChanged().
            }
            OnPropertyChanged(propertyName);
        }

        #region Base

        public override bool CheckDirty()
        {
            if (!string.IsNullOrEmpty(sifArtikla)
                || !string.IsNullOrEmpty(nazArtikla)
                || !string.IsNullOrEmpty(jedMjere)
                || !string.IsNullOrEmpty(cijArtikla)
                || !string.IsNullOrEmpty(zastUsluga)
                || !string.IsNullOrEmpty(tekstArtikla))
                IsDirty = true;
            else
                IsDirty = false;
            return isDirty;
        }

        #endregion

        #region Private

        private string sifArtikla;
        private string nazArtikla;
        private string jedMjere;
        private string cijArtikla;
        private string zastUsluga;
        private string tekstArtikla;

        #endregion

        #region Public
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

        #endregion
    }
}
