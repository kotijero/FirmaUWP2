using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Model
{ 
    public class Dokument : ListableModel
    {
        public Dokument()
        {
            vrDokumenta = string.Empty;
            datDokumenta = DateTime.Now;
            
            partnerLookup = Defaults.PartnerLookup;
            prethodniDokumentLookup = Defaults.DokumentLookup;
        }

        public Dokument(DTO.Dokument dokument)
        {
            idDokumenta = dokument.IdDokumenta;
            vrDokumenta = dokument.VrDokumenta;
            brDokumenta = dokument.BrDokumenta;
            datDokumenta = dokument.DatDokumenta;
            idPartnera = dokument.IdPartnera;
            idPrethDokumenta = dokument.IdPrethDokumenta;
            postoPorez = dokument.PostoPorez;
            iznosDokumenta = dokument.IznosDokumenta;
        }

        public Dokument(DTO.Dokument dokument, List<Stavka> stavkaList)
            :this(dokument)
        {
            foreach (var stavka in stavkaList)
            {
                Stavke.Add(stavka);
            }
        }

        #region Private properties

        private int idDokumenta;
        private string vrDokumenta;
        private int brDokumenta;
        private DateTime datDokumenta;
        private int idPartnera;
        private Nullable<int> idPrethDokumenta;
        private decimal postoPorez;
        private decimal iznosDokumenta;

        private LookupModel prethodniDokumentLookup;
        private LookupModel partnerLookup;
        #endregion

        #region Public members

        public int IdDokumenta
        {
            get { return idDokumenta; }
            set
            {
                idDokumenta = value;
                OnPropertyChanged();
            }
        }
        public string VrDokumenta
        {
            get { return vrDokumenta; }
            set
            {
                vrDokumenta = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
            }
        }
        public int BrDokumenta
        {
            get { return brDokumenta; }
            set
            {
                brDokumenta = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
            }
        }
        public DateTime DatDokumenta
        {
            get { return datDokumenta; }
            set
            {
                datDokumenta = value;
                OnPropertyChanged();
            }
        }
        public int IdPartnera
        {
            get { return idPartnera; }
            set
            {
                idPartnera = value;
                OnPropertyChanged();
            }
        }
        public Nullable<int> IdPrethDokumenta
        {
            get { return idPrethDokumenta; }
            set
            {
                idPrethDokumenta = value;
                OnPropertyChanged();
            }
        }
        public decimal PostoPorez
        {
            get { return postoPorez; }
            set
            {
                postoPorez = value;
                OnPropertyChanged();
            }
        }
        public decimal IznosDokumenta
        {
            get
            {
                return iznosDokumenta;
            }
            set
            {
                iznosDokumenta = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Subsubtitle));
            }
        }

        public virtual List<Stavka> Stavke { get; set; } = new List<Stavka>();

        public LookupModel PartnerLookup
        {
            get { return partnerLookup; }
            set
            {
                if (partnerLookup != null ? !partnerLookup.Equals(value) : true)
                {
                    partnerLookup = value;
                    if (value != null) idPartnera = value.Key;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Subtitle));
                }
            }
        }

        public LookupModel PrethodniDokumentLookup
        {
            get { return prethodniDokumentLookup; }
            set
            {
                if (prethodniDokumentLookup != null ? !prethodniDokumentLookup.Equals(value) : true)
                {
                    prethodniDokumentLookup = value;
                    if (value != null) idPrethDokumenta = value.Key;
                    OnPropertyChanged();
                }
            }
        }

        public string LookupData
        {
            get
            {
                return BrDokumenta + " - " + PartnerLookup.Value + " (" + DatDokumenta.ToShortDateString() + ")";
            }
        }



        #endregion

        public override string Title => $"{brDokumenta} ({vrDokumenta})";

        public override string Subtitle => PartnerLookup.Value;

        public override string Subsubtitle => $"{iznosDokumenta.ToString("N")} kn";


        public void AddStavkeFromList(List<Stavka> stavkaList, bool emptyCurrent = false)
        {
            if (emptyCurrent) Stavke.Clear();
            foreach(var stavka in stavkaList)
            {
                Stavke.Add(stavka);
            }
            OnPropertyChanged(nameof(Stavke));
        }

        public DTO.Dokument ToDTO()
        {
            return new DTO.Dokument
            {
                IdDokumenta = idDokumenta,
                VrDokumenta = vrDokumenta,
                BrDokumenta = brDokumenta,
                DatDokumenta = datDokumenta,
                IdPartnera = idPartnera,
                IdPrethDokumenta = IdPrethDokumenta.HasValue ? (IdPrethDokumenta.Value == -1 ? null : IdPrethDokumenta) : null,
                PostoPorez = postoPorez,
                IznosDokumenta = iznosDokumenta
            };
        }
    }
}
