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
    public class Artikl : ObservableModel
    {
        #region Constructors
        public Artikl()
        {
            nazArtikla = string.Empty;
            jedMjere = string.Empty;
            cijArtikla = 0.0M;
            tekstArtikla = string.Empty;
        }

        public Artikl(DTO.Artikl artikl)
        {
            sifArtikla = artikl.SifArtikla;
            nazArtikla = artikl.NazArtikla;
            jedMjere = artikl.JedMjere;
            cijArtikla = artikl.CijArtikla;
            zastUsluga = artikl.ZastUsluga;
            slikaArtikla = artikl.SlikaArtikla;
            tekstArtikla = artikl.TekstArtikla;
        }

        #endregion

        #region Private properties

        private int sifArtikla;
        private string nazArtikla;
        private string jedMjere;
        private decimal cijArtikla;
        private bool zastUsluga;
        private byte[] slikaArtikla;
        private string tekstArtikla;

        #endregion

        #region Public Properties

        public int SifArtikla
        {
            get { return sifArtikla; }
            set
            {
                if (!sifArtikla.Equals(value))
                {
                    sifArtikla = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NazArtikla
        {
            get { return nazArtikla; }
            set
            {
                if (!nazArtikla.Equals(value))
                {
                    nazArtikla = value;
                    OnPropertyChanged();
                }
            }
        }
        public string JedMjere
        {
            get { return jedMjere; }
            set
            {
                if (!jedMjere.Equals(value))
                {
                    jedMjere = value;
                    OnPropertyChanged();
                }
            }
        }
        public decimal CijArtikla
        {
            get { return cijArtikla; }
            set
            {
                if (!cijArtikla.Equals(value))
                {
                    cijArtikla = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool ZastUsluga
        {
            get { return zastUsluga; }
            set
            {
                if (!zastUsluga.Equals(value))
                {
                    zastUsluga = value;
                    OnPropertyChanged();
                }
            }
        }
        public byte[] SlikaArtikla
        {
            get { return slikaArtikla; }
            set
            {
                //if (!slikaArtikla.Equals(value))
                //{

                //}
            }
        }
        public byte[] SlikaArtiklaImage { get; set; }
        public string TekstArtikla
        {
            get { return tekstArtikla; }
            set
            {
                if (!tekstArtikla.Equals(value))
                {
                    tekstArtikla = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Validation

        public void Validate(string propertyName)
        {
            if (propertyName.Equals(nameof(SifArtikla)))
            {

            } else if (propertyName.Equals(nameof(NazArtikla)))
            {

            } else if (propertyName.Equals(nameof(JedMjere)))
            {

            } else if (propertyName.Equals(nameof(CijArtikla)))
            {

            } else if (propertyName.Equals(nameof(ZastUsluga)))
            {

            } else if (propertyName.Equals(nameof(TekstArtikla)))
            {

            }
        }

        #endregion

        #region DTO

        public DTO.Artikl ToDTO()
        {
            return new DTO.Artikl
            {
                SifArtikla = sifArtikla,
                NazArtikla = nazArtikla,
                JedMjere = jedMjere,
                CijArtikla = cijArtikla,
                ZastUsluga = zastUsluga,
                SlikaArtikla = slikaArtikla,
                TekstArtikla = tekstArtikla
            };
        }

        #endregion
    }
}
