using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Firma.Model
{
    public class Artikl : ListableModel
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

        private BitmapImage slikaArtiklaImage;
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
                    OnPropertyChanged(nameof(Title));
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
                    OnPropertyChanged(nameof(Subtitle));
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
                    OnPropertyChanged(nameof(Subtitle));
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
                if (slikaArtikla == null || !slikaArtikla.Equals(value))
                {
                    slikaArtikla = value;
                }
            }
        }
        
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

        #region Slika Artikla
        public BitmapImage SlikaArtiklaImage
        {
            get
            {
                if (slikaArtiklaImage == null)
                {
                    SetSlikaArtiklaImageFromByteArray();
                }
                return slikaArtiklaImage;
            }
            set
            {
                slikaArtiklaImage = value;
                OnPropertyChanged();
            }
        }

        

        private void SetSlikaArtiklaImageFromByteArray()
        {
            if (slikaArtikla != null && slikaArtikla.Length > 0)
            {
                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(slikaArtikla);
                        writer.StoreAsync().GetResults();
                    }
                    if (slikaArtiklaImage == null) slikaArtiklaImage = new BitmapImage();
                    slikaArtiklaImage.SetSource(ms);
                }
            }
            else
            {
                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                {
                    using (var inputStream = ms.GetOutputStreamAt(0))
                    using (DataWriter writer = new DataWriter(inputStream))
                    {
                        writer.WriteBytes(new byte[0]);
                        writer.StoreAsync().GetResults();
                    }
                    slikaArtiklaImage = new BitmapImage();
                    SlikaArtiklaImage.SetSource(ms);
                    OnPropertyChanged(nameof(SlikaArtiklaImage));
                }
            }
        }

        public void ResetImageSource()
        {
            SetSlikaArtiklaImageFromByteArray();
        }

        public async void SetSlikaArtiklaFromFile(Windows.Storage.StorageFile file)
        {
            if (file == null)
            {
                SlikaArtikla = new byte[0];
                SetSlikaArtiklaImageFromByteArray();
            }
            else
            {
                // set byte[]
                using (var inputStream = await file.OpenSequentialReadAsync())
                {
                    var readStream = inputStream.AsStreamForRead();
                    SlikaArtikla = new byte[readStream.Length];
                    await readStream.ReadAsync(SlikaArtikla, 0, SlikaArtikla.Length);
                }
                // set BitmapImage
                SetSlikaArtiklaImageFromByteArray();
            }
        }

        #endregion
        #endregion

        #region DTO

        public override string Title => nazArtikla;

        public override string Subtitle => $"{CijArtikla.ToString("N")} kn/{JedMjere}";

        public override string Subsubtitle => string.Empty;

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
