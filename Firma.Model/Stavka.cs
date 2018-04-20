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
    public class Stavka : NotifyPropertyChanged
    {
        public Stavka()
        {
            kolArtikla = 1.0M;
            jedCijArtikla = 0.0M;
        }

        public Stavka(DTO.Stavka stavka)
        {
            idStavke = stavka.IdStavke;
            idDokumenta = stavka.IdDokumenta;
            sifArtikla = stavka.SifArtikla;
            kolArtikla = stavka.KolArtikla;
            jedCijArtikla = stavka.JedCijArtikla;
            postoRabat = stavka.PostoRabat;
            artikl = new Artikl(stavka.Artikl);
        }

        public Stavka(DTO.Stavka stavka, List<Artikl> artiklList)
        {
            idStavke = stavka.IdStavke;
            idDokumenta = stavka.IdDokumenta;
            sifArtikla = stavka.SifArtikla;
            kolArtikla = stavka.KolArtikla;
            jedCijArtikla = stavka.JedCijArtikla;
            postoRabat = stavka.PostoRabat;
            artikl = artiklList.FirstOrDefault(t => t.SifArtikla.Equals(sifArtikla));
        }

        #region Private properties

        private int idStavke;
        private int idDokumenta;
        private int sifArtikla;
        private decimal kolArtikla;
        private decimal jedCijArtikla;
        private decimal postoRabat;

        private Artikl artikl;

        #endregion

        #region Public properties
        public int IdStavke
        {
            get { return idStavke; }
            set
            {
                idStavke = value;
                OnPropertyChanged();
            }
        }
        public int IdDokumenta
        {
            get { return idDokumenta; }
            set
            {
                idDokumenta = value;
                OnPropertyChanged();
            }
        }
        public int SifArtikla
        {
            get { return sifArtikla; }
            set
            {
                sifArtikla = value;
                OnPropertyChanged();
            }
        }
        public decimal KolArtikla
        {
            get { return kolArtikla; }
            set
            {
                kolArtikla = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Ukupno));
            }
        }
        public decimal JedCijArtikla
        {
            get { return jedCijArtikla; }
            set
            {
                jedCijArtikla = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Ukupno));
            }
        }
        public decimal PostoRabat
        {
            get { return postoRabat; }
            set
            {
                postoRabat = value;
                OnPropertyChanged();
            }
        }

        public Artikl Artikl
        {
            get { return artikl; }
            set
            {
                artikl = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NazArtikla));
                OnPropertyChanged(nameof(JedMjere));
                OnPropertyChanged(nameof(Ukupno));
            }
        }

        #endregion

        public List<Artikl> ArtiklList { get; set; }

        private bool inEditMode;
        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                inEditMode = value;
                OnPropertyChanged();
            }
        }

        public string NazArtikla
        {
            get
            {
                if (Artikl == null)
                    return "-";
                else
                    return this.Artikl.NazArtikla;
            }
        }

        public string JedMjere
        {
            get
            {
                if (Artikl == null)
                    return "-";
                else
                    return this.Artikl.JedMjere;
            }
        }

        public decimal Ukupno
        {
            get
            {
                return KolArtikla * JedCijArtikla;
            }
        }



        public DTO.Stavka ToDTO()
        {
            return new DTO.Stavka
            {
                IdStavke = idStavke,
                IdDokumenta = idDokumenta,
                SifArtikla = sifArtikla,
                KolArtikla = kolArtikla,
                JedCijArtikla = jedCijArtikla,
                PostoRabat = postoRabat
            };
        }
    }
}
