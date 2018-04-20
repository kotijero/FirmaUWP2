using Firma.Bl;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ViewModel
{
    public class PartnerViewModel : NotifyPropertyChanged
    {
        #region Constructor

        public PartnerViewModel()
        {
            CreateNewPartner();
            currentPosition = -1;
            inEditMode = false;
            Loading = true;
        }

        public async Task Load()
        {
            BlMjesto blMjesto = new BlMjesto();
            MjestoLookupList = await Task.Run(() => blMjesto.FetchLookup());
            // init list
            partnerList = blPartner.FetchAll();
            // init filter
            foreach (var partner in partnerList)
            {
                partner.MjestoIsporukeLookup = MjestoLookupList.FirstOrDefault(t => t.Key.Equals(partner.IdMjestaIsporuke.Value));
                partner.MjestoSjedistaLookup = MjestoLookupList.FirstOrDefault(t => t.Key.Equals(partner.IdMjestaPartnera.Value));
                FilteredPartnerList.Add(partner);
            }
            OnPropertyChanged(nameof(MjestoLookupList));
            Loading = false;
        }

        #endregion

        #region Properties

        private BlPartner blPartner = new BlPartner();

        private List<Partner> partnerList;

        public ObservableCollection<Partner> FilteredPartnerList { get; set; } = new ObservableCollection<Partner>();

        private int currentPosition;

        public int CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged();
                if (value.Equals(-1))
                    ShowDetails = false;
                else
                {
                    ShowDetails = true;
                    OnPropertyChanged(nameof(CurrentPartner));
                    OnPropertyChanged(nameof(TipPartnera));
                }
            }
        }

        public Partner CurrentPartner
        {
            get
            {
                if (isNewItem || loading) return newPartner;
                else return partnerList[currentPosition];
            }
        }

        private bool isNewItem = false;

        public bool IsNewItem
        {
            get { return isNewItem; }
        }

        private bool showDetails;

        public bool ShowDetails
        {
            get { return showDetails; }
            set
            {
                showDetails = value;
                OnPropertyChanged();
            }
        }

        #region Edit Mode

        private bool inEditMode;

        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                inEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NotInEditMode));
            }
        }

        public bool NotInEditMode
        {
            get { return !inEditMode; }
        }

        #endregion

        #region Loading Delay

        private bool loading = false;

        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region CRUD

        private Partner originalPartner;

        public void Edit()
        {
            if (!inEditMode)
            {
                InEditMode = true;
                originalPartner = new Partner
                {
                    IdPartnera = CurrentPartner.IdPartnera,
                    TipPartnera = CurrentPartner.TipPartnera,
                    OIB = CurrentPartner.OIB,
                    ImeOsobe = CurrentPartner.ImeOsobe,
                    PrezimeOsobe = CurrentPartner.PrezimeOsobe,
                    MatBrTvrtke = CurrentPartner.MatBrTvrtke,
                    NazivTvrtke = CurrentPartner.NazivTvrtke,
                    AdrPartnera = CurrentPartner.AdrPartnera,
                    MjestoIsporukeLookup = CurrentPartner.MjestoIsporukeLookup,
                    AdrIsporuke = CurrentPartner.AdrIsporuke,
                    MjestoSjedistaLookup = CurrentPartner.MjestoSjedistaLookup
                };
            }
        }

        public async void Save()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Potvrda spremanja",
                Content = $"Spremiti zapis artikla {CurrentPartner.Naziv}?",
                PrimaryButtonText = "Da, spremi",
                CloseButtonText = "Natrag na uređivanje"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                InEditMode = false;
                if (isNewItem)
                {
                    newPartner = blPartner.AddItem(newPartner);
                    partnerList.Add(newPartner);
                    Filter = string.Empty;
                    CurrentPosition = partnerList.IndexOf(newPartner);
                    isNewItem = false;
                    OnPropertyChanged(nameof(IsNewItem));
                }
                else
                {
                    blPartner.UpdateItem(CurrentPartner);
                }
            }
        }

        public void Cancel()
        {
            InEditMode = false;
            if (isNewItem)
            {
                isNewItem = false;
                OnPropertyChanged(nameof(IsNewItem));
            }
            else
            {
                CurrentPartner.IdPartnera = originalPartner.IdPartnera;
                TipPartnera = originalPartner.TipPartnera;
                CurrentPartner.OIB = originalPartner.OIB;
                CurrentPartner.ImeOsobe = originalPartner.ImeOsobe;
                CurrentPartner.PrezimeOsobe = originalPartner.PrezimeOsobe;
                CurrentPartner.MatBrTvrtke = originalPartner.MatBrTvrtke;
                CurrentPartner.NazivTvrtke = originalPartner.NazivTvrtke;
                CurrentPartner.AdrPartnera = originalPartner.AdrPartnera;
                CurrentPartner.MjestoIsporukeLookup = originalPartner.MjestoIsporukeLookup;
                CurrentPartner.AdrIsporuke = originalPartner.AdrIsporuke;
                CurrentPartner.MjestoSjedistaLookup = originalPartner.MjestoSjedistaLookup;
            }
            OnPropertyChanged(nameof(CurrentPartner));
            OnPropertyChanged(nameof(TipPartnera));
        }

        private Partner newPartner;

        private void CreateNewPartner()
        {
            newPartner = new Partner();
            if (loading)
            {
                newPartner.MjestoIsporukeLookup = MjestoLookupList.FirstOrDefault(t => t.Key.Equals(-1));
                newPartner.MjestoSjedistaLookup = MjestoLookupList.FirstOrDefault(t => t.Key.Equals(-1));
            }
            else
            {
                newPartner.MjestoIsporukeLookup = new LookupModel(-1, "-Nije odabrano-");
                newPartner.MjestoSjedistaLookup = new LookupModel(-1, "-Nije odabrano-");
            }
        }

        public void New()
        {
            CreateNewPartner();

            isNewItem = true;
            InEditMode = true;
            ShowDetails = true;
            OnPropertyChanged(nameof(IsNewItem));
            OnPropertyChanged(nameof(CurrentPartner));
            OnPropertyChanged(nameof(TipPartnera));
        }

        public async void Delete()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Brisanje",
                Content = $"Jeste li sigurni da žeite obrisati partnera {CurrentPartner.Naziv}?",
                PrimaryButtonText = "Da",
                SecondaryButtonText = "Odustani"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Partner toDelete = CurrentPartner;
                string response = blPartner.DeleteItem(toDelete);
                if (string.IsNullOrEmpty(response))
                {
                    partnerList.Remove(toDelete);
                    ApplyFilter();
                }
                else
                {
                    ContentDialog warningDialog = new ContentDialog
                    {
                        Title = "Upozorenje",
                        Content = response,
                        PrimaryButtonText = "U redu"
                    };
                    await warningDialog.ShowAsync();
                }
            }
        }

        #endregion

        #region Filter

        private string filter;

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public void ApplyFilter()
        {
            FilteredPartnerList.Clear();
            foreach(var partner in partnerList.Where(t => t.Naziv.ToLower().Contains(filter.ToLower())))
            {
                FilteredPartnerList.Add(partner);
            }
        }

        #endregion

        #region Lookups

        public List<LookupModel> MjestoLookupList;

        public string TipPartnera
        {
            get { return CurrentPartner.TipPartnera; }
            set
            {
                if (value.Equals(Constants.OsobaTip) || value.Equals(Constants.TvrtkaTip))
                {
                    CurrentPartner.TipPartnera = value;
                    OnPropertyChanged();
                }

            }
        }
        #endregion
    }
}
