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
        public DokumentPickerViewModel(List<Dokument> itemsList)
        {
            this.itemsList = itemsList;
            SelectionMade = false;
            currentPosition = -1;
            filter = string.Empty;
            ApplyFilter();
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

        public string CanSelect()
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

        private void ApplyFilter()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
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
