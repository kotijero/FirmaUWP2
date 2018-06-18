using Firma.Bl;
using Firma.Bl.ValidationModels;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CustomControlViewModels;

namespace ViewModel.DomainViewModels
{
    public class StavkaViewModel : ObservableModel
    {
        public StavkaViewModel(Stavka stavka, List<Artikl> artiklList, Action onArtiklChanged, bool inEditMode = false)
        {
            this.stavka = stavka;
            this.ArtiklList = artiklList;
            this.inEditMode = inEditMode;
            Edited = false;
            OnArtiklChanged = onArtiklChanged;
        }

        private Stavka stavka;
        public Stavka Stavka
        {
            get
            {
                stavka.SifArtikla = stavka.Artikl.SifArtikla;
                return stavka;
            }
        }

        private bool Edited;

        private bool inEditMode;

        public bool InEditMode
        {
            get { return inEditMode; }
            set { inEditMode = value; OnPropertyChanged(); }
        }

        #region Public Encapsulated Properties
        
        public decimal KolArtikla
        {
            get { return stavka.KolArtikla; }
            set
            {
                stavka.KolArtikla = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Ukupno));
                OnArtiklChanged?.Invoke();
            }
        }

        public decimal PostoRabat
        {
            get { return stavka.PostoRabat; }
            set
            {
                stavka.PostoRabat = value;
                OnPropertyChanged();
            }
        }
        

        public decimal JedCijArtikla
        {
            get
            {
                if (Edited) return stavka.Artikl.CijArtikla;
                else return stavka.JedCijArtikla;
            }
        }

        public Artikl Artikl
        {
            get { return stavka.Artikl; }
            set
            {
                stavka.Artikl = value;
                if (value != null)
                {
                    stavka.SifArtikla = value.SifArtikla;
                }
                else
                {
                    stavka.SifArtikla = -1;
                }
                Edited = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(JedCijArtikla));
                OnPropertyChanged(nameof(JedMjere));
                OnPropertyChanged(nameof(Ukupno));
                OnArtiklChanged?.Invoke();
            }
        }

        public string JedMjere { get { return stavka.Artikl != null ? stavka.Artikl.JedMjere : string.Empty; } }

        public decimal Ukupno { get { return JedCijArtikla * stavka.KolArtikla; } }

        #endregion

        #region Lookup
        public Action OnArtiklChanged { get; private set; }
        public List<Artikl> ArtiklList { get; private set; }

        #endregion

        #region Validation

        public StavkaValidationModel Errors { get; } = new StavkaValidationModel();
        public void ValidateProperty(string propertyName)
        {
            if (inEditMode)
            {
                BlDokument blDokument = new BlDokument();
                blDokument.ValidateStavkaProperty(stavka, Errors, propertyName);
            }
        }
        public Action<string> ValidatePropertyAction(string propertyName)
        {
            return (o) => ValidateProperty(propertyName);
        }

        #endregion

        #region CustomControls VM Generators

        public ArtiklPickerViewModel GenerateArtiklPickerViewModel()
        {
            return new ArtiklPickerViewModel(ArtiklList);
        }

        #endregion

        #region AutoSuggest

        private string nazArtiklaAutoSuggestText;
        public string NazArtiklaAutoSuggestText
        {
            get { return nazArtiklaAutoSuggestText; }
            set
            {
                nazArtiklaAutoSuggestText = value;
                OnArtiklChanged();
            }
        }

        public bool SubmitArtikl(ArtiklPickerViewModel artiklPickerViewModel)
        {
            if (artiklPickerViewModel.Selected)
            {
                Artikl = artiklPickerViewModel.ItemsList[artiklPickerViewModel.CurrentPosition];
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
