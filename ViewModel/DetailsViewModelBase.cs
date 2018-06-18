using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CustomControlViewModels;

namespace ViewModel
{
    public abstract class DetailsViewModelBase<T> : ObservableModel where T : ListableModel, new()
    {
        public DetailsViewModelBase()
        {
            loading = true;
            showDetails = false;
            inEditMode = false;
            isNewItem = false;
            newItem = new T();

            CrudControlsViewModel = new CrudControlsViewModel()
            {
                NewAction = () => New(),
                EditAction = () => Edit(),
                SaveAction = () => Save(),
                CancelAction = () => Cancel(),
                DeleteAction = () => Delete()
            };
        }

        public DetailsViewModelBase(Func<T, string, bool> filterCondition)
            :this()
        {
            FilterCondition = filterCondition;
            FilteredListViewModel = new FilteredListViewModel()
            {
                ChangePositionAction = () => PositionChanged(),
                FilterChangedAction = (s) => OnFilterChanged(s),
                Loading = true
            };
        }

        #region Nested ViewModels

        public CrudControlsViewModel CrudControlsViewModel;
        public FilteredListViewModel FilteredListViewModel;

        #endregion

        #region List and Filter

        public List<T> itemsList = new List<T>();

        Func<T, string, bool> FilterCondition;

        public void OnFilterChanged(string filter)
        {
            List<T> newList = new List<T>();
            foreach(var item in itemsList)
            {
                if (FilterCondition(item, filter)) newList.Add(item);
            }
            FilteredListViewModel.SetItemsList(newList);
        }

        #endregion

        #region Position handling

        public T CurrentItem
        {
            get
            {   // prva tri uvjeta su za slucajeve kada nije predvidjeno prikazivanje modela, a zadnji kada se radi novi
                if (loading || !showDetails || FilteredListViewModel.CurrentPosition < 0 || isNewItem) return newItem;
                else return (T)FilteredListViewModel.CurrentItem;
            }
        }

        public virtual void PositionChanged()
        {
            ClearErrors(true);
            if (FilteredListViewModel.CurrentPosition < 0)
            {
                ShowDetails = false;
            }
            else
            {
                ShowDetails = true;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        #endregion

        #region Loading

        public abstract Task<string> Load();

        protected bool loading;
        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                OnPropertyChanged();
                FilteredListViewModel.Loading = loading;
            }
        }

        #endregion

        #region Backup, new

        protected T oldItem;
        protected T newItem;

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
                OnPropertyChanged(nameof(NotInEditMode));
                CrudControlsViewModel.InEditMode = value;
                FilteredListViewModel.InEditMode = value;
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

        public abstract string Edit();
        public abstract string New();
        public abstract string Save();
        public abstract void Cancel();
        public abstract string Delete();

        #endregion

        #region Validation

        public abstract void ClearErrors(bool notify);

        public abstract void ValidateProperty(string propertyName);

        public Action<string> ValidatePropertyAction(string propertyName)
        {
            return (o) => ValidateProperty(propertyName);
        }

        #endregion
    }
}
