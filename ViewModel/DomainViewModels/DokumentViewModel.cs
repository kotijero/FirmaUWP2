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

namespace ViewModel.DomainViewModels
{
    public class DokumentViewModel : DetailsViewModelBase<Dokument>
    {
        public DokumentViewModel()
            : base((t, s) => t.BrDokumenta.ToString().Contains(s) || t.VrDokumenta.ToLower().Contains(s.ToLower()))
        {
            ArtiklList = new List<Artikl>();
        }

        BlDokument BlDokument = new BlDokument();

        #region Lookups

        private List<Artikl> ArtiklList;

        public ObservableCollection<LookupModel> PartnerLookupList { get; } = new ObservableCollection<LookupModel>();
        public ObservableCollection<LookupModel> DokumentLookupList { get; } = new ObservableCollection<LookupModel>();

        #endregion

        public async override Task<string> Load()
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
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Loading = false );
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

            var dokumentReponse = await Task.Run(() => BlDokument.FetchAll(ArtiklList));
            if (!string.IsNullOrEmpty(dokumentReponse.ErrorMessage))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Loading = false);
                return dokumentReponse.ErrorMessage;
            }
            itemsList = dokumentReponse.Value;

            // dokumenti Lookup
            List<LookupModel> dokumentLookupList = new List<LookupModel>();
            dokumentLookupList.Add(Defaults.DokumentLookup);
            foreach (var dokument in itemsList)
            {
                dokument.PartnerLookup = partnerLookupList.FirstOrDefault(t => t.Key.Equals(dokument.IdPartnera));
                dokumentLookupList.Add(new LookupModel(dokument.IdDokumenta, dokument.LookupData));
            }

            // dokumenti normalni:
            foreach (var dokument2 in itemsList)
            {
                if (dokument2.IdPrethDokumenta.HasValue)
                    dokument2.PrethodniDokumentLookup = DokumentLookupList.FirstOrDefault(t => t.Key.Equals(dokument2.IdPrethDokumenta.Value));
                else
                    dokument2.PrethodniDokumentLookup = Defaults.DokumentLookup;
            }
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (var partner in partnerLookupList)
                    {
                        PartnerLookupList.Add(partner);
                    }

                    foreach (var dokumentLookup in dokumentLookupList)
                    {
                        DokumentLookupList.Add(dokumentLookup);
                    }

                    FilteredListViewModel.SetItemsList(itemsList);
                    Loading = false;
                });
            return string.Empty;
        }

        #region Position handling override

        public override void PositionChanged()
        {
            base.PositionChanged();
            PartnerAutoSuggestText = CurrentItem.PartnerLookup.Value;
            LoadStavkeFromCurrentDokument();
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

        #region InEditModeHandling

        public override bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                base.InEditMode = value;
                SetStavkasEditMode();
            }
        }

        #endregion

        #region CRUD

        public override void Cancel()
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

        public override string Delete()
        {
            Dokument toDelete = CurrentItem;
            var response = BlDokument.DeleteItem(toDelete);
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                itemsList.Remove(toDelete);

                FilteredListViewModel.Filter = string.Empty;
                return string.Empty;
            }
            else
            {
                return response.ErrorMessage;
            }
        }

        public override string Edit()
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

        public override string New()
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

        public override string Save()
        {
            if (Errors.IsDirty || CheckStavkeIsDirty())
            {
                return "Detektirane greške unosa.";
            }
            InEditMode = false;
            if (IsNewItem)
            {
                var response = BlDokument.AddItem(newItem);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    return response.ErrorMessage;
                }
                newItem = response.Value;
                itemsList.Add(newItem);
                FilteredListViewModel.Filter = string.Empty;
                FilteredListViewModel.CurrentPosition = FilteredListViewModel.ItemsList.IndexOf(newItem);
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
            return string.Empty;
        }

        #endregion

        #region Stavke

        public ObservableCollection<StavkaViewModel> StavkeList { get; } = new ObservableCollection<StavkaViewModel>();

        public void LoadStavkeFromCurrentDokument()
        {
            StavkeList.Clear();
            foreach (var stavka in CurrentItem.Stavke)
            {
                StavkeList.Add(new StavkaViewModel(stavka, ArtiklList, () => OnStavkaArtiklChanged()));
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
                if (stavka.Errors.IsDirty) return true;
            }
            return false;
        }

        public StavkaViewModel NewStavka()
        {
            if (inEditMode)
            {
                var newStavka = new StavkaViewModel(new Stavka { IdDokumenta = CurrentItem.IdDokumenta }, ArtiklList, () => OnStavkaArtiklChanged(), inEditMode);
                StavkeList.Add(newStavka);
                return newStavka;
            }
            return null;
        }

        public void RemoveStavka(StavkaViewModel stavka)
        {
            StavkeList.Remove(stavka);
        }

        #endregion

        #region CustomControls VM Generators

        public DokumentPickerViewModel GenerateDokumentPickerViewModel()
        {
            return new DokumentPickerViewModel(itemsList);
        }

        #endregion

        #region Validation

        public override void ClearErrors(bool notify)
        {
            Errors.ClearErrors(notify);
        }

        public DokumentValidationModel Errors { get; } = new DokumentValidationModel();

        public override void ValidateProperty(string propertyName)
        {
            if (inEditMode)
                BlDokument.ValidateProperty(CurrentItem, Errors, propertyName);
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
                CurrentItem.PrethodniDokumentLookup = DokumentLookupList.First(t => t.Key.Equals(dokumentPickerViewModel.ItemsList[dokumentPickerViewModel.CurrentPosition].IdDokumenta));
            else
                CurrentItem.PrethodniDokumentLookup = DokumentLookupList.First(t => t.Key.Equals(-1));
            return string.Empty;
        }

        #endregion
    }
}
