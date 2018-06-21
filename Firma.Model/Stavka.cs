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
    public class Stavka : ListableModel
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
        }

        public Stavka(DTO.Stavka stavka, Artikl artikl)
            :this(stavka)
        {
            this.artikl = artikl;
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
            }
        }
        public int IdDokumenta
        {
            get { return idDokumenta; }
            set
            {
                idDokumenta = value;
            }
        }
        public int SifArtikla
        {
            get { return sifArtikla; }
            set
            {
                sifArtikla = value;
            }
        }
        public decimal KolArtikla
        {
            get { return kolArtikla; }
            set
            {
                kolArtikla = value;
            }
        }
        public decimal JedCijArtikla
        {
            get
            {
                if (!(jedCijArtikla > 0))
                {
                    if (artikl == null)
                    {
                        return 0.0M;
                        
                    }
                    else
                    {
                        jedCijArtikla = artikl.CijArtikla;
                    }
                }
                return jedCijArtikla;
            }
            set
            {
                jedCijArtikla = value;
            }
        }

        public decimal PostoRabat
        {
            get { return postoRabat; }
            set
            {
                postoRabat = value;
            }
        }

        public Artikl Artikl
        {
            get { return artikl; }
            set
            {
                artikl = value;
            }
        }

        public override string Title => Artikl.NazArtikla;

        public override string Subtitle => KolArtikla + " " + Artikl.JedMjere;

        public override string Subsubtitle => $"{kolArtikla * jedCijArtikla}";

        #endregion



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
