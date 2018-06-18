using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CustomControlViewModels
{
    public class FilteredListViewModel : ObservableModel
    {
        public FilteredListViewModel()
        {
            currentPosition = -1;
        }

        #region Edit Mode & Show Details

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

        #region Loading

        private bool loading;
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

        #region List Control

        public ObservableCollection<ListableModel> ItemsList { get; set; } = new ObservableCollection<ListableModel>();

        public void SetItemsList<T>(List<T> itemsList) where T : ListableModel
        {
            ItemsList.Clear();
            foreach(var item in itemsList)
            {
                ItemsList.Add(item);
            }
        }

        #endregion

        #region Position

        public Action ChangePositionAction { get; set; }

        private int currentPosition;

        public int CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged();
                ChangePositionAction?.Invoke();
            }
        }

        public ListableModel CurrentItem
        {
            get
            {
                if (currentPosition < 0 || currentPosition >= ItemsList.Count) return default(ListableModel);
                else return ItemsList[currentPosition];
            }
        }

        #endregion

        #region Filter

        private string filter = string.Empty;
        
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                FilterChangedAction?.Invoke(filter);
                OnPropertyChanged();
            }
        }

        private Func<ListableModel, string, bool> filterPredicate;
        public Func<ListableModel, string, bool> FilterPredicate
        {
            get
            {
                if (filterPredicate == null) return (t, s) => true; // ako nije definiran vacati ce uvije true
                else return filterPredicate;
            }
            set
            {
                filterPredicate = value;
            }
        }

        public Action<string> FilterChangedAction { get; set; }
        

        #endregion
    }
}
