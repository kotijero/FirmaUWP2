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
    public class ArtiklPickerViewModel : ObservableModel
    {
        public ArtiklPickerViewModel(List<Artikl> artiklList)
        {
            filter = string.Empty;
            itemsList = artiklList;
            ApplyFilter();
        }
        private List<Artikl> itemsList;
        public ObservableCollection<Artikl> ItemsList { get; } = new ObservableCollection<Artikl>();

        public bool Selected { get; set; }

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

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
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
            }
        }

        public void ApplyFilter()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    ItemsList.Clear();
                    foreach (var item in itemsList.Where(t => t.NazArtikla.ToLower().Contains(filter.ToLower())))
                    {
                        ItemsList.Add(item);
                    }
                });
        }

        public bool CanSelect(bool selectIfCan)
        {
            if (currentPosition < 0)
            {
                ErrorMessage = "Niste odabrali artikl!";
                return false;
            }
            else if (selectIfCan)
            {
                Selected = true;
            }
            return true;
        }
    }
}
