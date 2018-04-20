using Firma.Bl;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Firma.ViewModel
{
    public class ArtiklViewModel : ObservableModel
    {
        public ArtiklViewModel()
        {
            inEditMode = false;
            artiklList = blArtikl.FetchAll();
        }

        #region Properties

        private BlArtikl blArtikl = new BlArtikl();

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
                    OnPropertyChanged("IsNotFirst");
                    OnPropertyChanged("IsNotLast");
                }
            }
        }

        public bool IsNotFirst
        {
            get { return currentPosition > 0; }
        }

        public bool IsNotLast
        {
            get { return currentPosition + 1 < ItemsCount; }
        }

        public Artikl CurrentArtikl
        {
            get
            {
                if (isNewItem) return newArtikl;
                else return artiklList[currentPosition];
            }
        }

        private Artikl newArtikl;
        private Artikl originalArtikl;

        public int ItemsCount { get { return artiklList == null ? 0 : artiklList.Count; } }

        private bool isNewItem = false;

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

        #endregion

        #region View Binding Actions

        #region Navigation

        public void Previous()
        {
            if (!inEditMode && currentPosition > 0)
                CurrentPosition--;

        }

        public void Next()
        {
            if (!inEditMode && currentPosition + 1 < ItemsCount)
                CurrentPosition++;
        }

        #endregion

        #region CRUD

        public void Edit()
        {
            if (!inEditMode)
            {
                InEditMode = true;
                originalArtikl = new Artikl
                {
                    NazArtikla = CurrentArtikl.NazArtikla,
                    JedMjere = CurrentArtikl.JedMjere,
                    CijArtikla = CurrentArtikl.CijArtikla,
                    ZastUsluga = CurrentArtikl.ZastUsluga,
                    SlikaArtikla = CurrentArtikl.SlikaArtikla,
                    TekstArtikla = CurrentArtikl.TekstArtikla
                };
            }
        }

        public async void Save()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Potvrda spremanja",
                Content = "Spremiti zapis artikla " + CurrentArtikl.NazArtikla + "?",
                PrimaryButtonText = "Da, spremi",
                SecondaryButtonText = "Ne, izađi bez spremanja",
                CloseButtonText = "Natrag na uređivanje"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (isNewItem)
                {
                    blArtikl.AddItem(newArtikl);
                    artiklList.Add(newArtikl);
                    CurrentPosition = artiklList.IndexOf(newArtikl) + 1;
                    isNewItem = false;
                    OnPropertyChanged("ItemsCount");
                }
                else
                {
                    blArtikl.UpdateItem(CurrentArtikl);
                }
                InEditMode = false;
            }
            else if (result == ContentDialogResult.Secondary)
            {
                Cancel();
            }
        }

        public void Cancel()
        {
            InEditMode = false;
            if (isNewItem)
            {
                isNewItem = false;
            }
            else
            {
                CurrentArtikl.NazArtikla = originalArtikl.NazArtikla;
                CurrentArtikl.JedMjere = originalArtikl.JedMjere;
                CurrentArtikl.CijArtikla = originalArtikl.CijArtikla;
                CurrentArtikl.ZastUsluga = originalArtikl.ZastUsluga;
                CurrentArtikl.SlikaArtikla = originalArtikl.SlikaArtikla;
                CurrentArtikl.TekstArtikla = originalArtikl.TekstArtikla;
            }
            OnPropertyChanged(nameof(CurrentArtikl));
        }

        public void New()
        {
            newArtikl = new Artikl();
            isNewItem = true;
            InEditMode = true;
            OnPropertyChanged(nameof(CurrentArtikl));
        }

        public async void Delete()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Brisanje zapisa",
                Content = "Jeste li sigurni da želite izbrisati zapis artikla " + CurrentArtikl.NazArtikla + "?",
                PrimaryButtonText = "Da",
                CloseButtonText = "Ne"
            };
            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Artikl artiklToDelete = CurrentArtikl;
                blArtikl.DeleteItem(artiklToDelete);
                CurrentPosition = 1;
                artiklList.Remove(artiklToDelete);
                OnPropertyChanged("ItemsCount");
            }
        }

        #endregion

        #endregion


    }
}
