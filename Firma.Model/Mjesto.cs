using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Model
{
    public class Mjesto : ListableModel 
    {
        public Mjesto()
        {
            oznDrzave = string.Empty;
            nazMjesta = string.Empty;
            postNazMjesta = string.Empty;
        }

        public Mjesto(DTO.Mjesto mjesto)
        {
            IdMjesta = mjesto.IdMjesta;
            OznDrzave = mjesto.OznDrzave;
            NazMjesta = mjesto.NazMjesta;
            PostBrMjesta = mjesto.PostBrMjesta;
            PostNazMjesta = mjesto.PostNazMjesta;
        }

        private int idMjesta;
        private string oznDrzave;
        private string nazMjesta;
        private int? postBrMjesta;
        private string postNazMjesta;

        public int IdMjesta
        {
            get { return idMjesta; }
            set
            {
                if (!idMjesta.Equals(value))
                {
                    idMjesta = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public string NazMjesta
        {
            get { return nazMjesta; }
            set
            {
                if (!nazMjesta.Equals(value))
                {
                    nazMjesta = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? PostBrMjesta
        {
            get { return postBrMjesta; }
            set
            {
                if (!postBrMjesta.Equals(value))
                {
                    postBrMjesta = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PostNazMjesta
        {
            get { return postNazMjesta; }
            set
            {
                if (!postNazMjesta.Equals(value))
                {
                    postNazMjesta = value;
                }
            }
        }


        public override string Title => NazMjesta;

        public override string Subtitle => PostBrMjesta == null ? string.Empty : PostBrMjesta.Value.ToString();

        public override string Subsubtitle => string.Empty;

        public DTO.Mjesto ToDTO()
        {
            return new DTO.Mjesto
            {
                IdMjesta = IdMjesta,
                OznDrzave = OznDrzave,
                NazMjesta = NazMjesta,
                PostBrMjesta = PostBrMjesta,
                PostNazMjesta = PostNazMjesta
            };
        }
    }
}
