using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl.ValidationModels
{
    public class PartnerValidationModel : ValidationModelBase
    {
        public PartnerValidationModel()
            : base()
        {
            
        }

        public override void ClearErrors(bool notify)
        {
            idPartnera = string.Empty;
            tipPartnera = string.Empty;
            adrPartnera = string.Empty;
            adrIsporuke = string.Empty;
            oib = string.Empty;

            imeOsobe = string.Empty;
            prezimeOsobe = string.Empty;
            nazivTvrtke = string.Empty;
            matBrTvrtke = string.Empty;

            if (notify)
            {
                OnPropertyChanged(nameof(IdPartnera));
                OnPropertyChanged(nameof(TipPartnera));
                OnPropertyChanged(nameof(AdrPartnera));
                OnPropertyChanged(nameof(AdrIsporuke));
                OnPropertyChanged(nameof(OIB));
                OnPropertyChanged(nameof(ImeOsobe));
                OnPropertyChanged(nameof(PrezimeOsobe));
                OnPropertyChanged(nameof(NazivTvrtke));
                OnPropertyChanged(nameof(MatBrTvrtke));
            }
        }

        public void ClearProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdPartnera):
                    idPartnera = string.Empty;
                    break;
                case nameof(TipPartnera):
                    tipPartnera = string.Empty;
                    break;
                case nameof(AdrPartnera):
                    adrPartnera = string.Empty;
                    break;
                case nameof(AdrIsporuke):
                    adrIsporuke = string.Empty;
                    break;
                case nameof(OIB):
                    oib = string.Empty;
                    break;
                case nameof(ImeOsobe):
                    imeOsobe = string.Empty;
                    break;
                case nameof(PrezimeOsobe):
                    prezimeOsobe = string.Empty;
                    break;
                case nameof(NazivTvrtke):
                    nazivTvrtke = string.Empty;
                    break;
                case nameof(MatBrTvrtke):
                    matBrTvrtke = string.Empty;
                    break;
                default:
                    return;
            }
            OnPropertyChanged(propertyName);
        }

        #region Base

        public override bool CheckDirty()
        {
            if (!string.IsNullOrEmpty(idPartnera)
                || !string.IsNullOrEmpty(tipPartnera)
                || !string.IsNullOrEmpty(adrPartnera)
                || !string.IsNullOrEmpty(adrIsporuke)
                || !string.IsNullOrEmpty(oib)
                || !string.IsNullOrEmpty(imeOsobe)
                || !string.IsNullOrEmpty(prezimeOsobe)
                || !string.IsNullOrEmpty(nazivTvrtke)
                || !string.IsNullOrEmpty(matBrTvrtke))
                IsDirty = true;
            else
                IsDirty = false;
            return isDirty;
        }

        #endregion

        #region Private
        private string idPartnera;
        private string tipPartnera;
        private string adrPartnera;
        private string adrIsporuke;
        private string oib;

        private string imeOsobe;
        private string prezimeOsobe;
        private string nazivTvrtke;
        private string matBrTvrtke;
        #endregion

        #region Public

        public string IdPartnera
        {
            get { return idPartnera; }
            set
            {
                if (string.IsNullOrEmpty(idPartnera))
                    idPartnera = value;
                else
                    idPartnera += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string TipPartnera
        {
            get { return tipPartnera; }
            set
            {
                if (string.IsNullOrEmpty(tipPartnera))
                    tipPartnera = value;
                else
                    tipPartnera += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string AdrPartnera
        {
            get { return adrPartnera; }
            set
            {
                if (string.IsNullOrEmpty(adrPartnera))
                    adrPartnera = value;
                else
                    adrPartnera += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string AdrIsporuke
        {
            get { return adrIsporuke; }
            set
            {
                if (string.IsNullOrEmpty(adrIsporuke))
                    adrIsporuke = value;
                else
                    adrIsporuke += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string OIB
        {
            get { return oib; }
            set
            {
                if (string.IsNullOrEmpty(oib))
                    oib = value;
                else
                    oib += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string ImeOsobe
        {
            get { return imeOsobe; }
            set
            {
                if (string.IsNullOrEmpty(imeOsobe))
                    imeOsobe = value;
                else
                    imeOsobe += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string PrezimeOsobe
        {
            get { return prezimeOsobe; }
            set
            {
                if (string.IsNullOrEmpty(prezimeOsobe))
                    prezimeOsobe = value;
                else
                    prezimeOsobe += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string NazivTvrtke
        {
            get { return nazivTvrtke; } 
            set
            {
                if (string.IsNullOrEmpty(nazivTvrtke))
                    nazivTvrtke = value;
                else
                    nazivTvrtke += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string MatBrTvrtke
        {
            get { return matBrTvrtke; }
            set
            {
                if (string.IsNullOrEmpty(matBrTvrtke))
                    matBrTvrtke = value;
                else
                    matBrTvrtke += $"\n{value}";
                CheckDirty();
                OnPropertyChanged();
            }
        }

        #endregion


    }
}
