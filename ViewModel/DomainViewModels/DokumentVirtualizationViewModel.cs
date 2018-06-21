using Firma.Bl;
using Firma.Bl.ValidationModels;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CustomControlViewModels;
using ViewModel.CustomDataSources;

namespace ViewModel.DomainViewModels
{
    public class DokumentVirtualizationViewModel : ObservableModel
    {

        public DokumentVirtualizationViewModel()
        {
            loading = true;
            showDetails = false;
            inEditMode = false;
            isNewItem = false;
            newItem = new Dokument();
            currentPosition = -1;
            itemLoaded = false;

            CrudControlsViewModel = new CrudControlsViewModel()
            {
                NewAction = () => New(),
                EditAction = () => Edit(),
                SaveAction = () => Save(),
                CancelAction = () => Cancel(),
                DeleteAction = () => Delete()
            };

            ArtiklList = new List<Artikl>();
        }

        BlDokument BlDokument = new BlDokument();
        #region Nested ViewModels

        public CrudControlsViewModel CrudControlsViewModel;

        #endregion

        #region DataSource

        public DokumentDataSource DokumentDataSource { get; set; }

        #region Lookups

        private List<Artikl> ArtiklList;

        public ObservableCollection<LookupModel> PartnerLookupList { get; } = new ObservableCollection<LookupModel>();
        public ObservableCollection<LookupModel> DokumentLookupList { get; } = new ObservableCollection<LookupModel>();

        #endregion

        #endregion

        #region Position Handling

        public Dokument CurrentItem
        {
            get
            {
                if (loading || !showDetails || currentPosition < 0 || isNewItem || !itemLoaded) return newItem;
                else return (Dokument)DokumentDataSource[currentPosition];
            }
        }

        private int currentPosition;

        public int CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged();
                PositionChanged();
            }
        }

        public void PositionChanged()
        {
            ClearErrors(true);
            // Zbog sporog (virtualnog) ucitavanja postoji slucaj kada trenutno odabrani
            // dokument jos uvijek nije ucitan, pa da ne baci exception:
            if (CurrentItem != null)
            {
                if (currentPosition < 0)
                {
                    ShowDetails = false;
                }
                else
                {
                    ShowDetails = true;
                    OnPropertyChanged(nameof(CurrentItem));
                    PartnerAutoSuggestText = CurrentItem.PartnerLookup.Value;
                    LoadStavkeFromCurrentDokument();
                }
            }
            else
            {
                partnerAutoSuggestText = string.Empty;
                ItemLoaded = false;
            }
            
            
        }



        #endregion

        #region Loading

        public async Task<string> Load()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    Loading = true;
                    ShowDetails = false;
                });

            // artikli
            BlArtikl blArtikl = new BlArtikl();
            var artiklResponse = await Task.Run(() => blArtikl.FetchAll());
            if (!string.IsNullOrEmpty(artiklResponse.ErrorMessage))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Loading = false);
                return artiklResponse.ErrorMessage;
            }
            ArtiklList = artiklResponse.Value;

            // partneri
            BlPartner blPartner = new BlPartner();
            var partnerReponse = await Task.Run(() => blPartner.FetchLookup());
            if (!string.IsNullOrEmpty(partnerReponse.ErrorMessage))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Loading = false);
                return partnerReponse.ErrorMessage;
            }
            var partnerLookupList = partnerReponse.Value;
            partnerLookupList.Add(Defaults.PartnerLookup);


            DokumentDataSource = await DokumentDataSource.GetDataSourceAsync(BlDokument, ItemCache_CacheChanged);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (var partner in partnerLookupList)
                    {
                        PartnerLookupList.Add(partner);
                    }
                    Loading = false;
                });
            return string.Empty;
        }

        protected bool loading;
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

        #region Deffered Virtual Loading

        private bool itemLoaded;

        public bool ItemLoaded
        {
            get { return itemLoaded; }
            set
            {
                itemLoaded = value;
                OnPropertyChanged();
            }
        }

        private void ItemCache_CacheChanged(object sender, Firma.Helpers.DataVirtualization.CacheChangedEventArgs<Dokument> args)
        {
            if (!itemLoaded)
            {
                if (CurrentItem != null)
                {
                    ItemLoaded = true;
                    PositionChanged();
                }
            }
        }

        #endregion

        #region Backup, new

        protected Dokument oldItem;
        protected Dokument newItem;

        #endregion

        #region Show Details

        protected bool showDetails;

        public bool ShowDetails
        {
            get { return showDetails; }
            set
            {
                showDetails = value;
                CrudControlsViewModel.ShowDetails = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Edit Mode
        protected bool inEditMode;

        public virtual bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                inEditMode = value;
                OnPropertyChanged();
                SetStavkasEditMode();
                OnPropertyChanged(nameof(NotInEditMode));
                CrudControlsViewModel.InEditMode = value;
            }
        }

        public bool NotInEditMode
        {
            get { return !inEditMode; }
        }

        #endregion

        #region IsNewItem

        protected bool isNewItem;
        public bool IsNewItem
        {
            get { return isNewItem; }
            set
            {
                isNewItem = value;
                if (value) ShowDetails = true;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Actions

        public void Cancel()
        {
            InEditMode = false;
            if (IsNewItem)
            {
                IsNewItem = false;
                ShowDetails = false;
                OnPropertyChanged(nameof(IsNewItem));
            }
            else
            {
                CurrentItem.IdDokumenta = oldItem.IdDokumenta;
                CurrentItem.IdPrethDokumenta = oldItem.IdPrethDokumenta;
                CurrentItem.PartnerLookup = oldItem.PartnerLookup;
                CurrentItem.BrDokumenta = oldItem.BrDokumenta;
                CurrentItem.VrDokumenta = oldItem.VrDokumenta;
                CurrentItem.DatDokumenta = oldItem.DatDokumenta;
                CurrentItem.PostoPorez = oldItem.PostoPorez;
                CurrentItem.PrethodniDokumentLookup = oldItem.PrethodniDokumentLookup;
                CurrentItem.AddStavkeFromList(oldItem.Stavke.ToList(), true);
                LoadStavkeFromCurrentDokument();
                PartnerAutoSuggestText = CurrentItem.PartnerLookup.Value;
            }
        }

        public string Delete()
        {
            Dokument toDelete = CurrentItem;
            var response = BlDokument.DeleteItem(toDelete);
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                // reload?
                // itemsList.Remove(toDelete);
                DokumentDataSource.Remove(toDelete);
                return string.Empty;
            }
            else
            {
                return response.ErrorMessage;
            }
        }

        public string Edit()
        {
            if (!inEditMode)
            {
                oldItem = new Dokument
                {
                    IdDokumenta = CurrentItem.IdDokumenta,
                    IdPrethDokumenta = CurrentItem.IdPrethDokumenta,
                    PartnerLookup = CurrentItem.PartnerLookup,
                    IdPartnera = CurrentItem.IdPartnera,
                    BrDokumenta = CurrentItem.BrDokumenta,
                    VrDokumenta = CurrentItem.VrDokumenta,
                    DatDokumenta = CurrentItem.DatDokumenta,
                    PostoPorez = CurrentItem.PostoPorez,
                    PrethodniDokumentLookup = CurrentItem.PrethodniDokumentLookup
                };

                List<Stavka> stavkeList = new List<Stavka>();
                foreach (var stavka in CurrentItem.Stavke)
                {
                    stavkeList.Add(new Stavka
                    {
                        IdStavke = stavka.IdStavke,
                        Artikl = stavka.Artikl,
                        KolArtikla = stavka.KolArtikla,
                        PostoRabat = stavka.PostoRabat,
                        IdDokumenta = stavka.IdDokumenta,
                        SifArtikla = stavka.SifArtikla
                    });
                }
                oldItem.AddStavkeFromList(stavkeList, true);
                InEditMode = true;
            }
            return string.Empty;
        }

        public string New()
        {
            newItem = new Dokument();

            IsNewItem = true;
            ShowDetails = true;
            OnPropertyChanged(nameof(IsNewItem));
            OnPropertyChanged(nameof(CurrentItem));
            LoadStavkeFromCurrentDokument();
            InEditMode = true;
            ClearErrors(true);
            return string.Empty;
        }

        public string Save()
        {
            BlDokument.ValidateModel(CurrentItem, Errors);
            if (Errors.CheckDirty() || CheckStavkeIsDirty())
            {
                return "Detektirane greške unosa.";
            }
            if (IsNewItem)
            {
                var response = BlDokument.AddItem(newItem);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    return response.ErrorMessage;
                }
                newItem = response.Value;
                // refresh?
                // itemsList.Add(newItem);
                DokumentDataSource.Add(newItem);
                CurrentPosition = DokumentDataSource.IndexOf(newItem);
                IsNewItem = false;
            }
            else
            {
                var response = BlDokument.UpdateItem(CurrentItem, oldItem);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    return response.ErrorMessage;
                }
            }
            InEditMode = false;
            return string.Empty;
        }

        #endregion

        #region Stavke

        private bool stavkeLoading;
        public bool StavkeLoading
        {
            get { return stavkeLoading; }
            set
            {
                stavkeLoading = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StavkaViewModel> StavkeList { get; } = new ObservableCollection<StavkaViewModel>();

        public async void LoadStavkeFromCurrentDokument()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    StavkeLoading = true;
                    StavkeList.Clear();
                });
            if (CurrentItem != null )
            {
                if (CurrentItem.Stavke == null || CurrentItem.Stavke.Count < 1)
                {
                    var stavkeResponse = BlDokument.FetchStavkeFor(CurrentItem.IdDokumenta, ArtiklList);
                    if (string.IsNullOrEmpty(stavkeResponse.ErrorMessage))
                    {
                        CurrentItem.Stavke = stavkeResponse.Value;
                    }
                }

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (var stavka in CurrentItem.Stavke)
                    {
                        StavkeList.Add(new StavkaViewModel(stavka, ArtiklList, () => OnStavkaArtiklChanged()));
                    }
                    StavkeLoading = false;
                });
            }
        }

        private void SetStavkasEditMode()
        {
            foreach (var stavka in StavkeList)
            {
                stavka.InEditMode = inEditMode;
            }
        }

        private bool CheckStavkeIsDirty()
        {
            foreach (var stavka in StavkeList)
            {
                if (stavka.Errors.CheckDirty()) return true;
            }
            return false;
        }

        public StavkaViewModel NewStavka()
        {
            if (inEditMode)
            {
                Stavka newStavka = new Stavka { IdDokumenta = CurrentItem.IdDokumenta };
                CurrentItem.Stavke.Add(newStavka);
                var newStavkaVM = new StavkaViewModel(newStavka, ArtiklList, () => OnStavkaArtiklChanged(), inEditMode);
                StavkeList.Add(newStavkaVM);
                return newStavkaVM;
            }
            return null;
        }

        public void RemoveStavka(StavkaViewModel stavka)
        {
            CurrentItem.Stavke.Remove(stavka.Stavka);
            StavkeList.Remove(stavka);
        }

        public void OnStavkaArtiklChanged()
        {
            if (inEditMode)
            {
                decimal total = 0.0M;
                foreach (var stavka in StavkeList)
                {
                    if (stavka.Artikl != null)
                    {
                        total += stavka.Ukupno;
                    }
                }
                CurrentItem.IznosDokumenta = total;
            }
        }

        #endregion

        #region CustomControls VM Generators

        public DokumentPickerViewModel GenerateDokumentPickerViewModel()
        {
            return new DokumentPickerViewModel(null, BlDokument);
        }

        #endregion

        #region Validation

        public void ClearErrors(bool notify)
        {
            Errors.ClearErrors(notify);
        }

        public DokumentValidationModel Errors { get; } = new DokumentValidationModel();

        public void ValidateProperty(string propertyName)
        {
            if (inEditMode)
                BlDokument.ValidateProperty(CurrentItem, Errors, propertyName);
        }

        public Action<string> ValidatePropertyAction(string propertyName)
        {
            return (o) => ValidateProperty(propertyName);
        }

        #endregion

        #region AutoSuggestion

        private string partnerAutoSuggestText;
        public string PartnerAutoSuggestText
        {
            get { return partnerAutoSuggestText; }
            set
            {
                partnerAutoSuggestText = value;
                if (!CurrentItem.PartnerLookup.Value.Equals(partnerAutoSuggestText))
                {
                    CurrentItem.PartnerLookup = Defaults.PartnerLookup;
                }
                OnPropertyChanged();
            }
        }

        public List<LookupModel> QueryPartner(string query)
        {
            if (string.IsNullOrEmpty(query)) return new List<LookupModel>();
            return PartnerLookupList.Where(t => t.Value.ToLower().Contains(query.ToLower())).ToList();
        }

        public string SubmitPartner(int id)
        {
            if (PartnerLookupList.Where(t => t.Key == id).Count() > 0)
            {
                CurrentItem.PartnerLookup = PartnerLookupList.First(t => t.Key == id);
                return string.Empty;
            }
            else
            {
                return "Odabrali ste nepostojećeg partnera";
            }
        }

        private string prethodniDokumentTekst;
        public string PrethodniDokumentTest
        {
            get { return prethodniDokumentTekst; }
            set
            {
                prethodniDokumentTekst = value;
                if (inEditMode)
                {
                    ValidateProperty(nameof(Dokument.IdPartnera));
                }
                OnPropertyChanged();
            }
        }

        public string SubmitPrethodniDokument(DokumentPickerViewModel dokumentPickerViewModel)
        {
            if (dokumentPickerViewModel.SelectionMade)
            {
                Dokument picked = dokumentPickerViewModel.ItemsList[dokumentPickerViewModel.CurrentPosition];
                CurrentItem.PrethodniDokumentLookup = new LookupModel(picked.IdDokumenta, picked.LookupData);
            }
            else
                CurrentItem.PrethodniDokumentLookup = Defaults.DokumentLookup;
            return string.Empty;
        }

        #endregion
    }
}
