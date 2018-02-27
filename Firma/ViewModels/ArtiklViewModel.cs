using Firma.DAL;
using Firma.DAL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Firma.ViewModels
{
    public class ArtiklViewModel : INotifyPropertyChanged
    {
        public ArtiklViewModel()
        {
            artiklList = artiklDalProvider.FetchAll();
        }
        #region NotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        private ArtiklDalProvider artiklDalProvider = new ArtiklDalProvider();

        private List<Artikl> artiklList;

        private int currentPosition;

        public int CurrentPosition
        {
            get { return currentPosition + 1; }
            set
            {
                if (!currentPosition.Equals(value - 1))
                {
                    currentPosition = value - 1;
                    OnPropertyChanged();
                    OnPropertyChanged("CurrentArtikl");
                }
            }
        }

        public Artikl CurrentArtikl { get { return artiklList[currentPosition]; } }

        public int ItemsCount { get { return artiklList == null ? 0 : artiklList.Count; } }

        #endregion

        #region View Binding Actions

        #region Navigation

        public void Previous()
        {

        }

        public void Next()
        {

        }

        #endregion

        #region CRUD

        public void Edit()
        {

        }

        public void Save()
        {

        }

        public void Cancel()
        {

        }

        public void New()
        {

        }

        public void Delete()
        {

        }

        #endregion

        #endregion

        #region EditMode

        private bool inEditMode;

        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                inEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged("NotInEditMode");
            }
        }

        public bool NotInEditMode
        {
            get { return !inEditMode; }
        }

        #endregion
    }
}
