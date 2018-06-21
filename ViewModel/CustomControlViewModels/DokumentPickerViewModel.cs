using Firma.Bl;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CustomControlViewModels
{
    public class DokumentPickerViewModel : ObservableModel
    {
        public DokumentPickerViewModel(List<Dokument> itemsList, BlDokument blDokument)
        {
            this.itemsList = itemsList;
            SelectionMade = false;
            currentPosition = -1;
            filter = string.Empty;
            
            if (itemsList == null)
            {
                loading = true;
                Load(blDokument);
            }
            else
            {
                loading = false;
                ApplyFilter();
            }
        }

        private async void Load(BlDokument blDokument)
        {
            await Task.Run(() =>
            {
                itemsList = blDokument.FetchAll().Value;
                BlPartner blPartner = new BlPartner();
                var partnerLookups = blPartner.FetchLookup();
                foreach(var dokument in itemsList)
                {
                    dokument.PartnerLookup = partnerLookups.Value.First(t => t.Key == dokument.IdPartnera);
                }
            });
            Loading = false;
            Filter = string.Empty;
        }

        private List<Dokument> itemsList;

        public ObservableCollection<Dokument> ItemsList { get; } = new ObservableCollection<Dokument>();

        private int currentPosition;
        public int CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged();
            }
        }

        public bool SelectionMade { get; set; }

        public string VerifySelection()
        {
            if (currentPosition < 0) return "Niste odabrali dokument!";
            SelectionMade = true;
            return string.Empty;
        }

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

        private bool loading;
        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowDetails));
            }
        }

        public bool ShowDetails
        {
            get { return !loading; }
        }

        private async void ApplyFilter()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    ItemsList.Clear();
                    foreach (var dokument in itemsList.Where(t => t.PartnerLookup.Value.ToLower().Contains(filter.ToLower())
                                                                 || t.BrDokumenta.ToString().Contains(filter)
                                                                 || t.VrDokumenta.ToLower().Contains(filter.ToLower())))
                    {
                        ItemsList.Add(dokument);
                    }
                });
        }
    }
}
