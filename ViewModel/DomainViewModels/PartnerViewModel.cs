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

namespace ViewModel.DomainViewModels
{
    public class PartnerViewModel : DetailsViewModelBase<Partner>
    {
        public PartnerViewModel()
            :base((t, s) => t.Naziv.ToLower().Contains(s.ToLower()))
        {

        }

        BlPartner BlPartner = new BlPartner();

        public async override Task<string> Load()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    Loading = true;
                    ShowDetails = false;
                    FilteredListViewModel.ItemsList.Clear();
                });
            
            BlMjesto blMjesto = new BlMjesto();
            var mjestoResponse = blMjesto.FetchLookup();
            if (!string.IsNullOrEmpty(mjestoResponse.ErrorMessage))
            {
                return mjestoResponse.ErrorMessage;
            }
            var mjestoLookupList = mjestoResponse.Value;
            mjestoLookupList.Add(Defaults.MjestoLookup);

            var partnerResponse = BlPartner.FetchAll();
            if (!string.IsNullOrEmpty(partnerResponse.ErrorMessage))
            {
                return partnerResponse.ErrorMessage;
            }
            itemsList = partnerResponse.Value.OrderBy(t => t.Naziv).ToList();
            foreach (var partner in itemsList)
            {
                partner.MjestoIsporukeLookup = mjestoLookupList.FirstOrDefault(t => t.Key.Equals(partner.IdMjestaIsporuke.Value));
                partner.MjestoSjedistaLookup = mjestoLookupList.FirstOrDefault(t => t.Key.Equals(partner.IdMjestaPartnera.Value));

            }

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (var mjesto in mjestoLookupList)
                    {
                        MjestoLookupList.Add(mjesto);
                    }

                    FilteredListViewModel.SetItemsList(itemsList);

                    Loading = false;
                });
            return string.Empty;
        }

        #region Lookups

        public ObservableCollection<LookupModel> MjestoLookupList { get; set; } = new ObservableCollection<LookupModel>();

        #endregion

        #region Position handling override

        public override void PositionChanged()
        {
            base.PositionChanged();
            MjestoPartneraAutoSuggestText = CurrentItem.MjestoSjedistaLookup.Value;
            MjestoIsporukeAutoSuggestText = CurrentItem.MjestoIsporukeLookup.Value;
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
                CurrentItem.IdPartnera = oldItem.IdPartnera;
                CurrentItem.TipPartnera = oldItem.TipPartnera;
                CurrentItem.OIB = oldItem.OIB;
                CurrentItem.ImeOsobe = oldItem.ImeOsobe;
                CurrentItem.PrezimeOsobe = oldItem.PrezimeOsobe;
                CurrentItem.MatBrTvrtke = oldItem.MatBrTvrtke;
                CurrentItem.NazivTvrtke = oldItem.NazivTvrtke;
                CurrentItem.AdrPartnera = oldItem.AdrPartnera;
                CurrentItem.MjestoIsporukeLookup = oldItem.MjestoIsporukeLookup;
                CurrentItem.AdrIsporuke = oldItem.AdrIsporuke;
                CurrentItem.MjestoSjedistaLookup = oldItem.MjestoSjedistaLookup;
            }

            CreateNewPartner();
            ClearErrors(true);
        }

        public override string Delete()
        {
            Partner toDelete = CurrentItem;
            var response = BlPartner.DeleteItem(toDelete);
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
                InEditMode = true;
                oldItem = new Partner
                {
                    IdPartnera = CurrentItem.IdPartnera,
                    TipPartnera = CurrentItem.TipPartnera,
                    OIB = CurrentItem.OIB,
                    ImeOsobe = CurrentItem.ImeOsobe,
                    PrezimeOsobe = CurrentItem.PrezimeOsobe,
                    MatBrTvrtke = CurrentItem.MatBrTvrtke,
                    NazivTvrtke = CurrentItem.NazivTvrtke,
                    AdrPartnera = CurrentItem.AdrPartnera,
                    MjestoIsporukeLookup = CurrentItem.MjestoIsporukeLookup,
                    AdrIsporuke = CurrentItem.AdrIsporuke,
                    MjestoSjedistaLookup = CurrentItem.MjestoSjedistaLookup
                };
            }
            return string.Empty;
        }

        public override string New()
        {
            CreateNewPartner();
            IsNewItem = true;

            InEditMode = true;
            OnPropertyChanged(nameof(CurrentItem));
            ClearErrors(true);
            ShowDetails = true;
            return string.Empty;
        }

        public override string Save()
        {
            
            if (Errors.CheckDirty())
            {
                BlPartner.ValidateModel(CurrentItem, Errors);
                if (Errors.IsDirty)
                {
                    return "Unos podataka nije korektan. Potrebno ispraviti prije spremanja.";
                }
            }
            InEditMode = false;
            if (IsNewItem)
            {
                var response = BlPartner.AddItem(newItem);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    return response.ErrorMessage;
                }
                newItem = response.Value;
                itemsList.Add(newItem);
                FilteredListViewModel.Filter = string.Empty;
                FilteredListViewModel.CurrentPosition = FilteredListViewModel.ItemsList.IndexOf(newItem);
                IsNewItem = false;
                newItem = new Partner();
            }
            else
            {
                BlPartner.UpdateItem(CurrentItem);
            }
            return string.Empty;
        }

        private void CreateNewPartner()
        {
            newItem = new Partner();
            if (loading)
            {
                newItem.MjestoIsporukeLookup = MjestoLookupList.FirstOrDefault(t => t.Key.Equals(-1));
                newItem.MjestoSjedistaLookup = MjestoLookupList.FirstOrDefault(t => t.Key.Equals(-1));
            }
            else
            {
                // Ako se ucitavaju podaci onda vjerojatno lista mjesta nije
                // popunjena, pa se dodaju defaultne vrijedosti.
                newItem.MjestoIsporukeLookup = Defaults.MjestoLookup;
                newItem.MjestoSjedistaLookup = Defaults.MjestoLookup;
            }
        }

        #endregion


        #region Validation

        public override void ClearErrors(bool notify)
        {
            Errors.ClearErrors(notify);
        }

        public PartnerValidationModel Errors = new PartnerValidationModel();

        public override void ValidateProperty(string propertyName)
        {
            if (inEditMode)
                BlPartner.ValidateProperty(CurrentItem, Errors, propertyName);
        }

        #endregion

        #region AutoSuggest

        public List<LookupModel> QueryMjesto(string query)
        {
            if (string.IsNullOrEmpty(query)) return new List<LookupModel>();
            return MjestoLookupList.Where(t => t.Value.ToLower().Contains(query.ToLower())).ToList();
        }

        private string mjestoPartneraAutoSuggestText;
        private string mjestoSjedistaAutoSuggestText;

        public string MjestoPartneraAutoSuggestText
        {
            get { return mjestoPartneraAutoSuggestText; }
            set
            {
                mjestoPartneraAutoSuggestText = value;
                OnPropertyChanged();
            }
        }

        public string MjestoIsporukeAutoSuggestText
        {
            get { return mjestoSjedistaAutoSuggestText; }
            set
            {
                mjestoSjedistaAutoSuggestText = value;
                OnPropertyChanged();
            }
        }

        public void SubmitMjPartnera(LookupModel mjestoPartneraLookup)
        {
            CurrentItem.MjestoSjedistaLookup = mjestoPartneraLookup;
            MjestoPartneraAutoSuggestText = mjestoPartneraLookup.Value;
        }

        public void SubmitMjIsporuke(LookupModel mjestoIsporukeLookup)
        {
            CurrentItem.MjestoIsporukeLookup = mjestoIsporukeLookup;
            MjestoIsporukeAutoSuggestText = mjestoIsporukeLookup.Value;
        }

        #endregion
    }
}
