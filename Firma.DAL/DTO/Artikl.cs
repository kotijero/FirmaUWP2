using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DAL.DTO
{
    public class Artikl : INotifyPropertyChanged
    {
        public Artikl()
        {
            nazArtikla = string.Empty;
            jedMjere = string.Empty;
            cijArtikla = 0.0M;
            tekstArtikla = string.Empty;
        }

        #region NotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        public decimal CijArtkila
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
    }
}
