using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Model
{
    public class Partner : ListableModel
    {
        #region Constructors

        public Partner()
        {
            tipPartnera = Constants.Nedefinirano;
            adrPartnera = string.Empty;
            adrIsporuke = string.Empty;
            oib = string.Empty;

            // osoba
            imeOsobe = string.Empty;
            prezimeOsobe = string.Empty;

            // tvrtka
            nazivTvrtke = string.Empty;
            matBrTvrtke = string.Empty;

            mjestoIsporukeLookup = Defaults.MjestoLookup;
            mjestoPartneraLookup = Defaults.MjestoLookup;
        }

        public Partner(DTO.Partner partner, List<LookupModel> mjestoLookupList)
        {

            idPartnera = partner.IdPartnera;
            tipPartnera = partner.TipPartnera;
            oib = partner.OIB;

            idMjestaPartnera = partner.IdMjestaPartnera;
            adrPartnera = partner.AdrPartnera;
            mjestoPartneraLookup = mjestoLookupList.FirstOrDefault(t => t.Key.Equals(idMjestaPartnera));

            idMjestaIsporuke = partner.IdMjestaIsporuke;
            adrIsporuke = partner.AdrIsporuke;
            mjestoIsporukeLookup = mjestoLookupList.FirstOrDefault(t => t.Key.Equals(idMjestaIsporuke));
            
            if (tipPartnera.Equals(Constants.OsobaTip))
            {
                imeOsobe = ((DTO.Osoba)partner).ImeOsobe;
                prezimeOsobe = ((DTO.Osoba)partner).PrezimeOsobe;
                nazivTvrtke = string.Empty;
                matBrTvrtke = string.Empty;
            }
            else
            {
                imeOsobe = string.Empty;
                prezimeOsobe = string.Empty;
                nazivTvrtke = ((DTO.Tvrtka)partner).NazivTvrtke;
                matBrTvrtke = ((DTO.Tvrtka)partner).MatBrTvrtke;
            }
        }

        public Partner(DTO.Partner partner, LookupModel mjestoPartneraLookup, LookupModel mjestoIsporukeLookup)
        {
            idPartnera = partner.IdPartnera;
            tipPartnera = partner.TipPartnera;
            oib = partner.OIB;

            idMjestaPartnera = partner.IdMjestaPartnera;
            adrPartnera = partner.AdrPartnera;
            this.mjestoPartneraLookup = mjestoPartneraLookup;

            idMjestaIsporuke = partner.IdMjestaIsporuke;
            adrIsporuke = partner.AdrIsporuke;
            this.mjestoIsporukeLookup = mjestoIsporukeLookup;

            if (tipPartnera.Equals(Constants.OsobaTip))
            {
                imeOsobe = ((DTO.Osoba)partner).ImeOsobe;
                prezimeOsobe = ((DTO.Osoba)partner).PrezimeOsobe;
                nazivTvrtke = string.Empty;
                matBrTvrtke = string.Empty;
            }
            else
            {
                imeOsobe = string.Empty;
                prezimeOsobe = string.Empty;
                nazivTvrtke = ((DTO.Tvrtka)partner).NazivTvrtke;
                matBrTvrtke = ((DTO.Tvrtka)partner).MatBrTvrtke;
            }
        }

        #endregion

        #region Private properties

        private int idPartnera;
        private string tipPartnera;
        private Nullable<int> idMjestaPartnera;
        private string adrPartnera;
        private Nullable<int> idMjestaIsporuke;
        private string adrIsporuke;
        private string oib;

        // Osoba:
        private string imeOsobe;
        private string prezimeOsobe;

        // Tvrtka
        private string nazivTvrtke;
        private string matBrTvrtke;

        #endregion

        #region Public properties

        public int IdPartnera
        {
            get { return idPartnera; }
            set
            {
                if (!idPartnera.Equals(value))
                {
                    idPartnera = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TipPartnera
        {
            get { return tipPartnera; }
            set
            {
                tipPartnera = value;
                OnPropertyChanged();
            }
        }
        public Nullable<int> IdMjestaPartnera
        {
            get { return idMjestaPartnera; }
            set
            {
                idMjestaPartnera = value;
                OnPropertyChanged();
            }
        }
        public string AdrPartnera
        {
            get { return adrPartnera; }
            set
            {
                if (!adrPartnera.Equals(value))
                {
                    adrPartnera = value;
                    OnPropertyChanged();
                }
            }
        }
        public Nullable<int> IdMjestaIsporuke
        {
            get { return idMjestaIsporuke; }
            set
            {
                idMjestaIsporuke = value;
                OnPropertyChanged();
            }
        }
        public string AdrIsporuke
        {
            get { return adrIsporuke; }
            set
            {
                if (!adrIsporuke.Equals(value))
                {
                    adrIsporuke = value;
                    OnPropertyChanged();
                }
            }
        }
        public string OIB
        {
            get { return oib; }
            set
            {
                if (!oib.Equals(value))
                {
                    oib = value;
                    OnPropertyChanged();
                }
            }
        }

        // Osoba:
        public string ImeOsobe
        {
            get
            {
                if (TipPartnera.Equals(Constants.OsobaTip))
                {
                    return imeOsobe;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (TipPartnera.Equals(Constants.OsobaTip))
                {
                    imeOsobe = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string PrezimeOsobe
        {
            get
            {
                if (TipPartnera.Equals(Constants.OsobaTip))
                {
                    return prezimeOsobe;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (TipPartnera.Equals(Constants.OsobaTip))
                {
                    prezimeOsobe = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        // Tvrtka
        public string NazivTvrtke
        {
            get
            {
                if (TipPartnera.Equals(Constants.TvrtkaTip))
                {
                    return nazivTvrtke;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (TipPartnera.Equals(Constants.TvrtkaTip))
                {
                    nazivTvrtke = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string MatBrTvrtke
        {
            get
            {
                if (TipPartnera.Equals(Constants.TvrtkaTip))
                {
                    return matBrTvrtke;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (TipPartnera.Equals(Constants.TvrtkaTip))
                {
                    matBrTvrtke = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string Naziv
        {
            get
            {
                if (tipPartnera.Equals(Constants.OsobaTip))
                {
                    return $"{ImeOsobe} {PrezimeOsobe}";
                }
                else
                {
                    return $"{NazivTvrtke} ({MatBrTvrtke})";
                }
            }
        }

        #endregion

        #region Lookup

        private LookupModel mjestoIsporukeLookup;
        private LookupModel mjestoPartneraLookup;

        public LookupModel MjestoIsporukeLookup
        {
            get { return mjestoIsporukeLookup; }
            set
            {
                if (value != null)
                {
                    if (value.Key != mjestoIsporukeLookup.Key)
                    {
                        mjestoIsporukeLookup = value;
                        idMjestaIsporuke = value.Key;
                        OnPropertyChanged();
                    }
                }
                else
                {
                    mjestoIsporukeLookup = Defaults.MjestoLookup;
                    idMjestaIsporuke = -1;
                    OnPropertyChanged();
                }
                
            }
        }
        public LookupModel MjestoSjedistaLookup
        {
            get { return mjestoPartneraLookup; }
            set
            {
                if (value != null)
                {
                    if (value.Key != mjestoPartneraLookup.Key)
                    {
                        mjestoPartneraLookup = value;
                        idMjestaPartnera = value.Key;
                        OnPropertyChanged();
                        OnPropertyChanged(nameof(Subtitle));
                    }
                }
                else
                {
                    idMjestaPartnera = -1;
                    mjestoPartneraLookup = Defaults.MjestoLookup;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Subtitle));
                }
            }
        }

        public override string Title => Naziv;

        public override string Subtitle => MjestoSjedistaLookup.Value;

        public override string Subsubtitle => string.Empty;

        #endregion

        #region DTO
        public DTO.Partner ToDTO()
        {
            if (tipPartnera.Equals(Constants.OsobaTip))
            {
                DTO.Osoba osoba = new DTO.Osoba
                {
                    IdPartnera = idPartnera,
                    OIB = oib,
                    TipPartnera = tipPartnera,
                    IdMjestaPartnera = idMjestaPartnera,
                    AdrPartnera = adrPartnera,
                    IdMjestaIsporuke = idMjestaIsporuke,
                    AdrIsporuke = adrIsporuke,
                    ImeOsobe = imeOsobe,
                    PrezimeOsobe = prezimeOsobe
                };
                return osoba;
            }
            else
            {
                DTO.Tvrtka tvrtka = new DTO.Tvrtka
                {
                    IdPartnera = idPartnera,
                    OIB = oib,
                    TipPartnera = tipPartnera,
                    IdMjestaPartnera = idMjestaPartnera,
                    AdrPartnera = adrPartnera,
                    IdMjestaIsporuke = idMjestaIsporuke,
                    AdrIsporuke = adrIsporuke,
                    NazivTvrtke = nazivTvrtke,
                    MatBrTvrtke = matBrTvrtke
                };
                return tvrtka;
            }
        }

        #endregion
    }
}
