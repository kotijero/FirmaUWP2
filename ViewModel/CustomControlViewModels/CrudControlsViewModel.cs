using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CustomControlViewModels
{
    public class CrudControlsViewModel : ObservableModel
    {
        public CrudControlsViewModel()
        {
            inEditMode = false;
        }

        #region Public Methods

        public string New()
        {
            return NewAction?.Invoke();
        }
        public string Edit()
        {
            return EditAction?.Invoke();
        }
        public string Save()
        {
            return SaveAction?.Invoke();
        }
        public void Cancel()
        {
            CancelAction?.Invoke();
        }
        public string Delete()
        {
            return DeleteAction?.Invoke();
        }

        #endregion

        #region EditMode

        protected bool inEditMode;
        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                inEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NotInEditMode));
                OnPropertyChanged(nameof(ShowDetailsAndNotInEditMode));
            }
        }

        public bool NotInEditMode
        {
            get { return !inEditMode; }
        }

        private bool showDetails;
        public bool ShowDetails
        {
            get { return showDetails; }
            set
            {
                showDetails = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowDetailsAndNotInEditMode));
            }
        }

        public bool ShowDetailsAndNotInEditMode
        {
            get { return showDetails && !inEditMode; }
        }

        #endregion

        #region Actions

        public Func<string> NewAction { get; set; }
        public Func<string> EditAction { get; set; }
        public Func<string> SaveAction { get; set; }
        public Action CancelAction { get; set; }
        public Func<string> DeleteAction { get; set; }

        #endregion
    }
}
