using Firma.Bl;
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
    public class KorisnikViewModel : ObservableModel
    {
        public Korisnik LoggedInUser { get; set; } = new Korisnik();
        public KorisnikViewModel()
        {
            newKorisnik = new Korisnik();
            currentPosition = -1;
            showDetails = false;
            isLoading = true;
        }

        public ObservableCollection<Korisnik> KorisnikList { get; } = new ObservableCollection<Korisnik>();

        public async Task<string> Load()
        {
            ShowDetails = false;
            KorisnikList.Clear();
            BlKorisnik blKorisnik = new BlKorisnik();
            var response = await Task.Run(() => blKorisnik.FetchAll());
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                return response.ErrorMessage;
            }
            foreach(var korisnik in response.Value)
            {
                KorisnikList.Add(korisnik);
            }
            IsLoading = false;
            return string.Empty;
        }

        private Korisnik oldKorisnik;

        public Korisnik CurrentKorisnik
        {
            get
            {
                if (IsLoading) return newKorisnik;
                else return KorisnikList[currentPosition];
            }
        }

        private Korisnik newKorisnik;

        private int currentPosition;
        public int CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                if (value < 0)
                {
                    ShowDetails = false;
                } else if (value >= KorisnikList.Count)
                {
                    currentPosition = KorisnikList.Count - 1;
                    ShowDetails = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentKorisnik));
                } else
                {
                    currentPosition = value;
                    ShowDetails = true;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentKorisnik));
                }
            }
        }

        #region UI Properties
        private bool showDetails;
        public bool ShowDetails
        {
            get { return showDetails; }
            set
            {
                showDetails = value;
                OnPropertyChanged();
            }
        }

        private bool isLoading;

        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

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

        #region Password Change Properties

        private string newPassword1;

        public string NewPassword1
        {
            get { return newPassword1; }
            set { newPassword1 = value; OnPropertyChanged(); }
        }

        private string newPassword2;

        public string NewPassword2
        {
            get { return newPassword2; }
            set { newPassword2 = value; OnPropertyChanged(); }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged(); }
        }


        #endregion

        #region Actions

        public void Edit()
        {
            InEditMode = true;
            oldKorisnik = new Korisnik
            {
                Username = CurrentKorisnik.Username,
                IsAdmin = CurrentKorisnik.IsAdmin
            };
        }
        
        public void New(Korisnik korisnik)
        {
            KorisnikList.Add(korisnik);
            CurrentPosition = KorisnikList.IndexOf(korisnik);
        }

        public void Cancel()
        {
            CurrentKorisnik.Username = oldKorisnik.Username;
            CurrentKorisnik.IsAdmin = oldKorisnik.IsAdmin;
            ErrorMessage = string.Empty;
            InEditMode = false;
        }

        public void Save()
        {
            ErrorMessage = string.Empty;
            BlKorisnik blKorisnik = new BlKorisnik();
            string response = string.Empty;
            if (string.IsNullOrEmpty(newPassword1) && string.IsNullOrEmpty(newPassword2))
            {
                
                response = blKorisnik.UpdateItemWithoutPassword(CurrentKorisnik, oldKorisnik.Username);
            }
            else
            {
                if (string.IsNullOrEmpty(newPassword1))
                {
                    ErrorMessage = "Potrebno navesti lozinku.";
                }
                else if (string.IsNullOrEmpty(newPassword2))
                {
                    ErrorMessage = "Potrebno ponoviti lozinku.";
                }
                else if (!newPassword1.Equals(newPassword2))
                {
                    ErrorMessage = "Navedene lozinke nisu jednake.";
                } else
                {
                    CurrentKorisnik.Password = newPassword1;
                    response = blKorisnik.UpdateItem(CurrentKorisnik, oldKorisnik.Username);
                }
            }
            if (!string.IsNullOrEmpty(response))
            {
                ErrorMessage = response;
            } else if (string.IsNullOrEmpty(errorMessage))
            {
                InEditMode = false;
            }
        }

        public string Delete()
        {
            BlKorisnik blKorisnik = new BlKorisnik();
            var result = blKorisnik.DeleteItem(CurrentKorisnik, LoggedInUser);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return result.ErrorMessage;
            }
            KorisnikList.Remove(CurrentKorisnik);
            return string.Empty;
        }

        #endregion
    }
}
