using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CustomControlViewModels;
using ViewModel.CustomDataSources;

namespace ViewModel
{
    public abstract class DetailsVirtualizationViewModelBase<T> : ObservableModel where T : ListableModel, new()
    {


        #region Nested ViewModels

        public CrudControlsViewModel CrudControlsViewModel;

        #endregion

        #region DataSource

        DokumentDataSource DokumentDataSource { get; set; }

        #endregion

        #region Position Handling

        public T CurrentItem
        {
            get
            {
                if (loading || !showDetails || currentPosition < 0 || isNewItem) return newItem;
                else return (T)DokumentDataSource[currentPosition];
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
